using System;
using System.Collections.Generic;
using System.Linq;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 테스트용 Scene 서비스를 실행하고 창이 닫힐 때 임시 hold 상태를 정리하는 에디터 창입니다.
/// </summary>
public class SceneEditorWindow : BaseEditorWindow
{
    private SceneLoadManager _sceneLoadManager;
    private string _sceneName = string.Empty;
    private string _loadingSceneName = string.Empty;
    private eSceneLoadingHoldMode _holdMode = eSceneLoadingHoldMode.BeforeSceneActivation;
    private bool _holdStatus;
    private string _status = string.Empty;
    private Vector2 _scroll;
    private bool _holdWasChanged;
    private readonly HashSet<string> _loadedByWindow = new();

    [MenuItem("PhikozzLib/Scene Editor Window")]
    private static void OpenWindow()
    {
        Open<SceneEditorWindow>("Scene Editor");
    }

    private void OnDisable()
    {
        if (!Application.isPlaying)
            return;

        if (_holdWasChanged && TryGetService(out var holdService, false))
        {
            holdService.SetHold(_holdMode, false);
        }

        if (!TryGetService(out var unloadService, false))
            return;

        foreach (var sceneName in _loadedByWindow.ToArray())
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded && scene.name != SceneManager.GetActiveScene().name)
            {
                unloadService.UnloadSceneAsync(sceneName);
            }
        }

        _loadedByWindow.Clear();
    }

    protected override void DrawGUI()
    {
        TitleLabel("Scene Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _sceneLoadManager = ObjectField(
            "SceneLoadManager",
            _sceneLoadManager != null ? _sceneLoadManager : FindAnyObjectByType<SceneLoadManager>(),
            true);

        Warning("Scene 테스트는 현재 Play Mode의 로드 상태를 변경할 수 있습니다. Single Load는 확인 후에만 실행하세요.");
        Space();

        DrawSceneStatus();
        Space();

        _sceneName = TextField("Scene Name", _sceneName);
        _loadingSceneName = TextField("Loading Scene", _loadingSceneName);
        _holdMode = EnumField("Hold Mode", _holdMode);
        _holdStatus = Toggle("Hold Status", _holdStatus);

        Space();

        BeginHorizontal();

        if (Button("Load Scene") && TryGetService(out var loadService) && RequireSceneName() &&
            Confirm("Load Scene", $"'{_sceneName}' 씬을 Single 모드로 로드합니다. 계속할까요?"))
        {
            loadService.LoadScene(_sceneName);
            _status = $"Load scene: {_sceneName}";
            Debug.Log(_status);
        }

        if (Button("Load With Loading") && TryGetService(out var loadingService) &&
            RequireSceneName() && RequireLoadingSceneName() &&
            Confirm("Load Scene With Loading", $"'{_sceneName}' 씬을 Loading Scene과 함께 로드합니다. 계속할까요?"))
        {
            loadingService.LoadSceneWithLoading(_sceneName, _loadingSceneName);
            _status = $"Load scene with loading: {_sceneName} / {_loadingSceneName}";
            Debug.Log(_status);
        }

        EndHorizontal();

        BeginHorizontal();

        if (Button("Load Additive") && TryGetService(out var additiveService) && RequireSceneName())
        {
            additiveService.LoadAdditiveScene(_sceneName);
            _loadedByWindow.Add(_sceneName);
            _status = $"Load additive scene: {_sceneName}";
            Debug.Log(_status);
        }

        if (Button("Load Additive With Loading") && TryGetService(out var additiveLoadingService) &&
            RequireSceneName() && RequireLoadingSceneName())
        {
            additiveLoadingService.LoadAdditiveSceneWithLoading(_sceneName, _loadingSceneName);
            _loadedByWindow.Add(_sceneName);
            _status = $"Load additive with loading: {_sceneName} / {_loadingSceneName}";
            Debug.Log(_status);
        }

        if (Button("Unload Scene") && TryGetService(out var unloadService) && RequireSceneName())
        {
            if (IsLoadedScene(_sceneName))
            {
                unloadService.UnloadSceneAsync(_sceneName);
                _loadedByWindow.Remove(_sceneName);
                _status = $"Unload scene: {_sceneName}";
                Debug.Log(_status);
            }
            else
            {
                _status = $"로드되지 않은 씬입니다: {_sceneName}";
                Debug.LogWarning(_status);
            }
        }

        EndHorizontal();

        BeginHorizontal();

        if (Button("Preload Additive") && TryGetService(out var preloadService) && RequireSceneName())
        {
            preloadService.PreloadSceneAsync(_sceneName);
            _loadedByWindow.Add(_sceneName);
            _status = $"Preload scene: {_sceneName}";
            Debug.Log(_status);
        }

        if (Button("Activate Preloaded") && TryGetService(out var activateService))
        {
            if (activateService.GetPreloadedSceneHandle() == null)
            {
                _status = "활성화할 preloaded scene이 없습니다.";
                Debug.LogWarning(_status);
            }
            else
            {
                activateService.ActivatePreloadedScene();
                _status = "Activated preloaded scene.";
                Debug.Log(_status);
            }
        }

        if (Button("Apply Hold") && TryGetService(out var holdService))
        {
            holdService.SetHold(_holdMode, _holdStatus);
            _holdWasChanged = true;
            _status = $"Set hold: {_holdMode} / {_holdStatus}";
            Debug.Log(_status);
        }

        EndHorizontal();

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }
    }

    private void DrawSceneStatus()
    {
        BoldLabel("Loaded Scenes");
        _scroll = BeginScrollView(_scroll, GUILayout.Height(140f));

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            string active = scene == SceneManager.GetActiveScene() ? " [Active]" : string.Empty;
            Label($"{scene.name}{active}");
        }

        EndScrollView();
    }

    private bool IsLoadedScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
            return false;

        return SceneManager.GetSceneByName(sceneName).isLoaded;
    }

    private bool TryGetService(out ISceneService service, bool logError = true)
    {
        service = _sceneLoadManager;

        if (service != null)
            return true;

        try
        {
            service = Core.Scene;
            return service != null;
        }
        catch (Exception e)
        {
            if (logError)
            {
                _status = $"Scene 서비스가 준비되지 않았습니다. {e.Message}";
                Debug.LogError(_status);
            }

            return false;
        }
    }

    private bool RequireSceneName()
    {
        if (!string.IsNullOrWhiteSpace(_sceneName))
            return true;

        _status = "Scene Name을 입력하세요.";
        Debug.LogError(_status);
        return false;
    }

    private bool RequireLoadingSceneName()
    {
        if (!string.IsNullOrWhiteSpace(_loadingSceneName))
            return true;

        _status = "Loading Scene을 입력하세요.";
        Debug.LogError(_status);
        return false;
    }
}
