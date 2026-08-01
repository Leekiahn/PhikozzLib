using System;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 테스트용 Time 서비스를 실행하고 종료 시 시간을 원복하는 에디터 창입니다.
/// </summary>
public class TimeEditorWindow : BaseEditorWindow
{
    private TimeManager _timeManager;
    private eTimeScaleMethods _timeScaleMethod = eTimeScaleMethods.For;
    private float _timeScale = 0f;
    private float _duration = 0.1f;
    private bool _lerp;
    private float _lerpSpeed = 1f;
    private bool _infinite;
    private float _freezeDuration = 0.05f;
    private string _status = string.Empty;

    [MenuItem("PhikozzLib/Time Editor Window")]
    private static void OpenWindow()
    {
        Open<TimeEditorWindow>("Time Editor");
    }

    private void OnDisable()
    {
        if (Application.isPlaying && TryGetService(out var service, false))
        {
            service.ResetTimeScale();
        }
    }

    protected override void DrawGUI()
    {
        TitleLabel("Time Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _timeManager = ObjectField(
            "TimeManager",
            _timeManager != null ? _timeManager : FindAnyObjectByType<TimeManager>(),
            true);

        BeginBox();
        BoldLabel("Current Time");
        Label("timeScale", Time.timeScale.ToString("0.00"));
        EndBox();

        Space();

        _timeScaleMethod = EnumField("TimeScale Method", _timeScaleMethod);
        _timeScale = Slider("Time Scale", _timeScale, 0f, 2f);
        _duration = FloatField("Duration", _duration);
        _lerp = Toggle("Lerp", _lerp);
        _lerpSpeed = FloatField("Lerp Speed", _lerpSpeed);
        _infinite = Toggle("Infinite", _infinite);
        _freezeDuration = FloatField("Freeze Duration", _freezeDuration);

        Space();

        BeginHorizontal();

        if (Button("Apply Time Scale") && TryGetService(out var timeScaleService))
        {
            timeScaleService.SetTimeScale(
                _timeScaleMethod,
                Mathf.Max(0f, _timeScale),
                Mathf.Max(0f, _duration),
                _lerp,
                Mathf.Max(0f, _lerpSpeed),
                _infinite);

            _status = $"Applied time scale: {_timeScaleMethod} / scale {Mathf.Max(0f, _timeScale):0.00}";
            Debug.Log(_status);
        }

        if (Button("Freeze Frame") && TryGetService(out var freezeService))
        {
            freezeService.FreezeFrame(Mathf.Max(0f, _freezeDuration));
            _status = $"Freeze frame: {Mathf.Max(0f, _freezeDuration):0.00}s";
            Debug.Log(_status);
        }

        EndHorizontal();

        BeginHorizontal();

        if (Button("Unfreeze") && TryGetService(out var unfreezeService))
        {
            unfreezeService.UnfreezeFrame();
            _status = "Unfreeze frame";
            Debug.Log(_status);
        }

        if (Button("Reset Time") && TryGetService(out var resetService))
        {
            resetService.ResetTimeScale();
            _status = "Reset time scale";
            Debug.Log(_status);
        }

        EndHorizontal();

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }
    }

    private bool TryGetService(out ITimeService service, bool logError = true)
    {
        service = _timeManager;

        if (service != null)
            return true;

        try
        {
            service = Core.Time;
            return service != null;
        }
        catch (Exception e)
        {
            if (logError)
            {
                _status = $"Time 서비스가 준비되지 않았습니다. {e.Message}";
                Debug.LogError(_status);
            }

            return false;
        }
    }
}
