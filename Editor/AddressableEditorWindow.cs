using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;

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

    [MenuItem("PhikozzLib/Addressable Editor Window")]
    public static void ShowWindow()
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

        if (Button("Is Loaded") && TryGetService(out var loadedService) && RequireLabel() && RequireKey())
        {
            _status = loadedService.IsLoaded(_label, _key)
                ? $"Loaded: {_key}"
                : $"Not loaded: {_key}";
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
        BoldLabel("Loaded Labels / Keys");
        Space();

        if (_addressableManager == null)
        {
            Info("AddressableManager를 찾으면 로드된 목록이 표시됩니다.");
            return;
        }

        if (!TryGetLabelCaches(out var caches) || caches.Count == 0)
        {
            Info("현재 로드된 label이 없습니다.");
            return;
        }

        _scroll = BeginScrollView(_scroll, GUILayout.Height(260f));

        foreach (DictionaryEntry cacheEntry in caches)
        {
            string label = cacheEntry.Key as string;
            object cache = cacheEntry.Value;

            if (string.IsNullOrEmpty(label) || cache == null)
                continue;

            var locationKeys = GetKeys(cache, "LocationByKey");
            var loadedKeys = GetKeys(cache, "AssetByKey");

            if (!_foldouts.ContainsKey(label))
                _foldouts[label] = true;

            _foldouts[label] = Foldout(
                _foldouts[label],
                $"{label} ({loadedKeys.Count}/{locationKeys.Count})");

            if (!_foldouts[label])
                continue;

            BeginIndent();

            if (locationKeys.Count == 0)
            {
                Label("- No Keys -");
            }
            else
            {
                foreach (var keyName in locationKeys)
                {
                    bool isLoaded = loadedKeys.Contains(keyName);
                    Label(isLoaded ? $"[Loaded] {keyName}" : $"[Pending] {keyName}");
                }
            }

            EndIndent();
            Space();
        }

        EndScrollView();
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