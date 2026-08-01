using System;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 테스트용 Save 서비스를 실행하고 임시 저장 데이터를 정리하는 에디터 창입니다.
/// </summary>
public class SaveEditorWindow : BaseEditorWindow
{
    private const string TemporaryKeyPrefix = "phikozzlib_editor_test_save";

    private SaveManager _saveManager;
    private string _saveKey = TemporaryKeyPrefix;
    private string _message = "Editor Save";
    private int _count = 1;
    private string _status = string.Empty;
    private string _loadedSummary = "-";
    private string _lastTemporaryKey = string.Empty;

    [MenuItem("PhikozzLib/Save Editor Window")]
    private static void OpenWindow()
    {
        Open<SaveEditorWindow>("Save Editor");
    }

    private void OnDisable()
    {
        CleanupTemporarySave();
    }

    protected override void DrawGUI()
    {
        TitleLabel("Save Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _saveManager = ObjectField(
            "SaveManager",
            _saveManager != null ? _saveManager : FindAnyObjectByType<SaveManager>(),
            true);

        _saveKey = TextField("Save Key", _saveKey);
        _message = TextField("Message", _message);
        _count = IntField("Count", _count);

        Space();

        BeginHorizontal();

        if (Button("Save Sample") && TryGetService(out var saveService) && RequireKey())
        {
            var data = new SaveEditorSampleData
            {
                Message = _message,
                Count = _count
            };

            saveService.Save(_saveKey, data);

            if (_saveKey.StartsWith(TemporaryKeyPrefix, StringComparison.Ordinal))
                _lastTemporaryKey = _saveKey;

            _status = $"Saved sample data with key: {_saveKey}";
            Debug.Log(_status);
        }

        if (Button("Load Sample") && TryGetService(out var loadService) && RequireKey())
        {
            if (loadService.TryLoad(_saveKey, out SaveEditorSampleData data))
            {
                _loadedSummary = $"{data.Message} / {data.Count}";
                _status = $"Loaded sample data with key: {_saveKey}";
            }
            else
            {
                _loadedSummary = "-";
                _status = $"저장 데이터를 찾지 못했습니다: {_saveKey}";
                Debug.LogWarning(_status);
            }
        }

        if (Button("Delete Sample") && TryGetService(out var deleteService) && RequireKey())
        {
            deleteService.Delete(_saveKey);

            if (string.Equals(_lastTemporaryKey, _saveKey, StringComparison.Ordinal))
                _lastTemporaryKey = string.Empty;

            _loadedSummary = "-";
            _status = $"Deleted save data with key: {_saveKey}";
            Debug.Log(_status);
        }

        EndHorizontal();

        Space();

        BeginBox();
        BoldLabel("Sample Data");
        Label("Loaded", _loadedSummary);
        Label("Temporary Save Prefix", TemporaryKeyPrefix);
        EndBox();

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }
    }

    private bool TryGetService(out ISaveService service, bool logError = true)
    {
        service = _saveManager;

        if (service != null)
            return true;

        try
        {
            service = Core.Save;
            return service != null;
        }
        catch (Exception e)
        {
            if (logError)
            {
                _status = $"Save 서비스가 준비되지 않았습니다. {e.Message}";
                Debug.LogError(_status);
            }

            return false;
        }
    }

    private bool RequireKey()
    {
        if (!string.IsNullOrWhiteSpace(_saveKey))
            return true;

        _status = "Save Key를 입력하세요.";
        Debug.LogError(_status);
        return false;
    }

    private void CleanupTemporarySave()
    {
        if (string.IsNullOrEmpty(_lastTemporaryKey))
            return;

        if (!_lastTemporaryKey.StartsWith(TemporaryKeyPrefix, StringComparison.Ordinal))
            return;

        if (!TryGetService(out var service, false))
            return;

        service.Delete(_lastTemporaryKey);
        _lastTemporaryKey = string.Empty;
    }

    [Serializable]
    private class SaveEditorSampleData
    {
        public string Message;
        public int Count;
    }
}
