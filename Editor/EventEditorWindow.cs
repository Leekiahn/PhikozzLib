using System;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 테스트용 Event 서비스를 실행하고 구독 상태를 확인하는 에디터 창입니다.
/// </summary>
public class EventEditorWindow : BaseEditorWindow
{
    private EventManager _eventManager;
    private string _message = "Editor Event";
    private int _count = 1;
    private string _status = string.Empty;
    private bool _isSubscribed;
    private int _receivedCount;
    private string _lastEventSummary = "-";

    private readonly Action<EditorTestEvent> _handler;

    public EventEditorWindow()
    {
        _handler = OnEditorTestEventReceived;
    }

    [MenuItem("PhikozzLib/Event Editor Window")]
    private static void OpenWindow()
    {
        Open<EventEditorWindow>("Event Editor");
    }

    private void OnDisable()
    {
        if (_isSubscribed && TryGetService(out var service, false))
        {
            service.Unsubscribe(_handler);
        }

        _isSubscribed = false;
    }

    protected override void DrawGUI()
    {
        TitleLabel("Event Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _eventManager = ObjectField(
            "EventManager",
            _eventManager != null ? _eventManager : FindAnyObjectByType<EventManager>(),
            true);

        _message = TextField("Message", _message);
        _count = IntField("Count", _count);

        Space();

        BeginHorizontal();

        if (Button(_isSubscribed ? "Subscribed" : "Subscribe"))
        {
            Subscribe();
        }

        if (Button("Unsubscribe"))
        {
            Unsubscribe();
        }

        if (Button("Publish") && TryGetService(out var publishService))
        {
            publishService.Publish(new EditorTestEvent(_message, _count, DateTime.Now.ToString("HH:mm:ss")));
            _status = $"Published event: message=\"{_message}\", count={_count}";
            Debug.Log(_status);
        }

        EndHorizontal();

        Space();

        BeginBox();
        BoldLabel("Subscription Status");
        Label("Subscribed", _isSubscribed ? "Yes" : "No");
        Label("Received Count", _receivedCount.ToString());
        Label("Last Event", _lastEventSummary);
        EndBox();

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }
    }

    private void Subscribe()
    {
        if (_isSubscribed)
        {
            _status = "이미 테스트 이벤트를 구독 중입니다.";
            Debug.LogWarning(_status);
            return;
        }

        if (!TryGetService(out var service))
            return;

        service.Subscribe(_handler);
        _isSubscribed = true;
        _status = "테스트 이벤트 구독을 등록했습니다.";
        Debug.Log(_status);
    }

    private void Unsubscribe()
    {
        if (!_isSubscribed)
        {
            _status = "현재 등록된 테스트 이벤트 구독이 없습니다.";
            Debug.LogWarning(_status);
            return;
        }

        if (!TryGetService(out var service))
            return;

        service.Unsubscribe(_handler);
        _isSubscribed = false;
        _status = "테스트 이벤트 구독을 해제했습니다.";
        Debug.Log(_status);
    }

    private void OnEditorTestEventReceived(EditorTestEvent evt)
    {
        _receivedCount++;
        _lastEventSummary = $"{evt.Message} / {evt.Count} / {evt.SentAt}";
        _status = $"Received event #{_receivedCount}: {_lastEventSummary}";
        RepaintWindow();
    }

    private bool TryGetService(out IEventService service, bool logError = true)
    {
        service = _eventManager;

        if (service != null)
            return true;

        try
        {
            service = Core.Event;
            return service != null;
        }
        catch (Exception e)
        {
            if (logError)
            {
                _status = $"Event 서비스가 준비되지 않았습니다. {e.Message}";
                Debug.LogError(_status);
            }

            return false;
        }
    }

    private readonly struct EditorTestEvent
    {
        public readonly string Message;
        public readonly int Count;
        public readonly string SentAt;

        public EditorTestEvent(string message, int count, string sentAt)
        {
            Message = message;
            Count = count;
            SentAt = sentAt;
        }
    }
}
