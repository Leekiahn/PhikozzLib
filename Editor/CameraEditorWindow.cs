using System;
using PhikozzLib;
using PhikozzLib.Editor;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class CameraEditorWindow : BaseEditorWindow
{
    private CameraManager _cameraManager;
    private eCameraType _cameraType = eCameraType.Main;
    private string _status = string.Empty;

    [MenuItem("PhikozzLib/Camera Editor Window")]
    private static void OpenWindow()
    {
        Open<CameraEditorWindow>("Camera Editor");
    }

    protected override void DrawGUI()
    {
        TitleLabel("Camera Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _cameraManager = ObjectField(
            "CameraManager",
            _cameraManager != null ? _cameraManager : FindAnyObjectByType<CameraManager>(),
            true);

        if (!TryGetManager(out var manager, false))
        {
            Info("CameraManager를 지정하면 현재 카메라 상태를 표시합니다.");
            return;
        }

        DrawCurrentStatus(manager);
        Space();
        DrawRegisteredCameras(manager);
        Space();

        _cameraType = EnumField("Camera Type", _cameraType);

        Space();

        if (Button("Set Camera"))
        {
            SetCamera(manager);
        }

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }
    }

    private void DrawCurrentStatus(CameraManager manager)
    {
        var activeCamera = manager.GetActiveCamera();

        BeginBox();
        BoldLabel("Current Camera");
        Label("Active Type", GetCameraTypeLabel(manager, activeCamera));
        Label("Camera", activeCamera != null ? activeCamera.name : "-");
        Label("Priority", activeCamera != null ? activeCamera.Priority.ToString() : "-");
        EndBox();
    }

    private void DrawRegisteredCameras(CameraManager manager)
    {
        BeginBox();
        BoldLabel("Registered Cameras");

        foreach (eCameraType type in Enum.GetValues(typeof(eCameraType)))
        {
            var camera = manager.GetCamera(type);
            string marker = manager.IsCurrent(type) ? " [Active]" : string.Empty;
            Label($"{type} : {GetCameraLabel(camera)}{marker}");
        }

        EndBox();
    }

    private void SetCamera(CameraManager manager)
    {
        var camera = manager.GetCamera(_cameraType);
        if (camera == null)
        {
            _status = $"등록되지 않은 Camera 타입입니다: {_cameraType}";
            Debug.LogError(_status);
            return;
        }

        manager.SetCamera(_cameraType);
        _status = $"Set camera: {_cameraType} / {camera.name}";
        Debug.Log(_status);
    }

    private bool TryGetManager(out CameraManager manager, bool logError = true)
    {
        manager = _cameraManager != null ? _cameraManager : CameraManager.Instance;

        if (manager == null)
        {
            manager = FindAnyObjectByType<CameraManager>();
        }

        if (manager != null)
            return true;

        if (logError)
        {
            _status = "CameraManager를 찾을 수 없습니다.";
            Debug.LogError(_status);
        }

        return false;
    }

    private static string GetCameraTypeLabel(CameraManager manager, CinemachineCamera activeCamera)
    {
        if (activeCamera == null)
            return "-";

        foreach (eCameraType type in Enum.GetValues(typeof(eCameraType)))
        {
            if (manager.IsCurrent(type))
                return type.ToString();
        }

        return "Unknown";
    }

    private static string GetCameraLabel(CinemachineCamera camera)
    {
        return camera != null
            ? $"{camera.name} (Priority: {camera.Priority})"
            : "Not Registered";
    }
}
