using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class AddressableEditorWindow : BaseEditorWindow
{
    private static readonly BindingFlags BindingFlag =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private AddressableManager _addressableManager;
    private string _label = string.Empty;
    private string _key = string.Empty;
    private string _status = string.Empty;
    private Vector2 _scroll;
    private readonly Dictionary<string, bool> _foldouts = new();
    private readonly HashSet<string> _downloadedLabels = new();

    [MenuItem("PhikozzLib/Addressable Editor Window")]
    protected static void OpenWindow()
    {
        Open<AddressableEditorWindow>("Addressable Editor");
    }

    protected override void DrawGUI()
    {
        TitleLabel("Addressable Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _addressableManager = ObjectField(
            "AddressableManager",
            _addressableManager != null ? _addressableManager : FindAnyObjectByType<AddressableManager>(),
            true);

        _label = TextField("Label", _label);
        _key = TextField("Key", _key);

        Space();

        BeginHorizontal();

        if (Button("Download") && TryGetService(out var downloadService) && RequireLabel())
        {
            Execute(async () =>
            {
                await downloadService.DownloadDependencies(_label);
                _downloadedLabels.Add(_label);
                _status = $"Downloaded: {_label}";
            });
        }

        if (Button("Preload Locations") && TryGetService(out var preloadLocationService) && RequireLabel())
        {
            Execute(async () =>
            {
                await preloadLocationService.PreloadLocations<GameObject>(_label);
                _status = $"Preloaded Locations: {_label}";
            });
        }

        if (Button("Preload Assets") && TryGetService(out var preloadAssetService) && RequireLabel())
        {
            Execute(async () =>
            {
                await preloadAssetService.PreloadAssets<GameObject>(_label);
                _status = $"Preloaded Assets: {_label}";
            });
        }

        EndHorizontal();

        BeginHorizontal();

        if (Button("Is Loaded Asset Key") && TryGetService(out var loadedKeyService) && RequireLabel() && RequireKey())
        {
            _status = loadedKeyService.IsLoadedAssetKey(_label, _key)
                ? $"Loaded: {_key}"
                : $"Not loaded: {_key}";
        }
        
        if (Button("Is Cached Label") && TryGetService(out var loadedLabelService) && RequireLabel())
        {
            _status = loadedLabelService.IsCachedLabel(_label)
                ? $"Cached: {_label}"
                : $"Not cached: {_label}";
        }

        if (Button("Release") && TryGetService(out var releaseService) && RequireLabel() && RequireKey())
        {
            releaseService.Release(_label, _key);
            _status = $"Released: {_key}";
        }

        if (Button("Release All") && TryGetService(out var releaseAllService) && RequireLabel())
        {
            releaseAllService.ReleaseAll(_label);
            _status = $"Released all: {_label}";
        }

        EndHorizontal();

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }

        Space(8f);
        DrawLoadedLabels();
    }

    private void DrawLoadedLabels()
    {
        BoldLabel("Addressable Labels / Keys");
        Space();

        IDictionary caches = null;
        TryGetLabelCaches(out caches);

        var allLabels = new HashSet<string>(_downloadedLabels);

        if (caches != null)
        {
            foreach (DictionaryEntry entry in caches)
            {
                if (entry.Key is string label)
                    allLabels.Add(label);
            }
        }

        if (allLabels.Count == 0)
        {
            Info("현재 표시할 label이 없습니다.");
            return;
        }

        _scroll = BeginScrollView(_scroll, GUILayout.Height(260f));

        foreach (var label in allLabels.OrderBy(x => x))
        {
            object cache = caches?[label];
            var locationKeys = cache != null ? GetKeys(cache, "LocationByKey") : new HashSet<string>();
            var loadedKeys = cache != null ? GetKeys(cache, "AssetByKey") : new HashSet<string>();
            bool isDownloaded = _downloadedLabels.Contains(label);

            if (!_foldouts.ContainsKey(label))
                _foldouts[label] = true;

            _foldouts[label] = Foldout(
                _foldouts[label],
                $"{label} [{GetLabelStateText(isDownloaded, locationKeys.Count, loadedKeys.Count)}]");

            if (!_foldouts[label])
                continue;

            BeginIndent();

            Label("Downloaded", isDownloaded ? "Yes" : "No");
            Label("Locations", locationKeys.Count.ToString());
            Label("Loaded Assets", loadedKeys.Count.ToString());

            if (locationKeys.Count == 0)
            {
                Label(isDownloaded ? "- Downloaded dependencies only -" : "- No Keys -");
            }
            else
            {
                foreach (var keyName in locationKeys.OrderBy(x => x))
                {
                    bool isLoaded = loadedKeys.Contains(keyName);
                    Label(isLoaded ? $"[Loaded] {keyName}" : $"[Location] {keyName}");
                }
            }

            EndIndent();
            Space();
        }

        EndScrollView();
    }
    
    private static string GetLabelStateText(bool isDownloaded, int locationCount, int loadedCount)
    {
        if (loadedCount > 0)
            return $"Downloaded:{(isDownloaded ? "Y" : "N")} / Loaded:{loadedCount}/{locationCount}";

        if (locationCount > 0)
            return $"Downloaded:{(isDownloaded ? "Y" : "N")} / Locations:{locationCount}";

        if (isDownloaded)
            return "Downloaded Only";

        return "Empty";
    }

    private bool TryGetLabelCaches(out IDictionary caches)
    {
        caches = null;

        var field = typeof(AddressableManager).GetField("_labelCaches", BindingFlag);
        if (field == null)
            return false;

        caches = field.GetValue(_addressableManager) as IDictionary;
        return caches != null;
    }

    private HashSet<string> GetKeys(object cache, string propertyName)
    {
        var result = new HashSet<string>();
        var property = cache.GetType().GetProperty(propertyName, BindingFlag);
        var dictionary = property?.GetValue(cache) as IDictionary;

        if (dictionary == null)
            return result;

        foreach (DictionaryEntry entry in dictionary)
        {
            if (entry.Key is string key)
                result.Add(key);
        }

        return result;
    }

    private bool TryGetService(out IAddressableService service)
    {
        service = _addressableManager;

        if (service != null)
            return true;

        _status = "AddressableManager를 찾을 수 없습니다.";
        Debug.LogError(_status);
        return false;
    }

    private bool RequireLabel()
    {
        if (!string.IsNullOrWhiteSpace(_label))
            return true;

        _status = "Label을 입력하세요.";
        Debug.LogError(_status);
        return false;
    }

    private bool RequireKey()
    {
        if (!string.IsNullOrWhiteSpace(_key))
            return true;

        _status = "Key를 입력하세요.";
        Debug.LogError(_status);
        return false;
    }

    private void Execute(Func<UniTask> action)
    {
        async void Run()
        {
            try
            {
                await action();
            }
            catch (Exception e)
            {
                _status = e.Message;
                Debug.LogException(e);
            }
        }

        Run();
    }
}