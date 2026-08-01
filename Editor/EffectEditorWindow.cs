using System;
using System.Linq;
using System.Reflection;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 테스트용 Effect 서비스를 실행하고 마지막 재생 이펙트를 정리하는 에디터 창입니다.
/// </summary>
public class EffectEditorWindow : BaseEditorWindow
{
    private static readonly BindingFlags BindingFlag =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private EffectManager _effectManager;
    private string _effectKey = string.Empty;
    private Vector3 _position;
    private Vector3 _rotationEuler;
    private Transform _attachTarget;
    private ParticleSystem _lastEffect;
    private string _status = string.Empty;
    private Vector2 _scroll;

    [MenuItem("PhikozzLib/Effect Editor Window")]
    private static void OpenWindow()
    {
        Open<EffectEditorWindow>("Effect Editor");
    }

    private void OnDisable()
    {
        StopLastEffect();
    }

    protected override void DrawGUI()
    {
        TitleLabel("Effect Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _effectManager = ObjectField(
            "EffectManager",
            _effectManager != null ? _effectManager : FindAnyObjectByType<EffectManager>(),
            true);

        _effectKey = TextField("Effect Key", _effectKey);
        _position = Vector3Field("Position", _position);
        _rotationEuler = Vector3Field("Rotation", _rotationEuler);
        _attachTarget = ObjectField("Attach Target", _attachTarget, true);

        Space();

        BeginHorizontal();

        if (Button("Play At Position") && TryGetService(out var positionService) && RequireConfiguredEffectKey())
        {
            _lastEffect = positionService.Play(
                _effectKey,
                _position,
                Quaternion.Euler(_rotationEuler));

            SetPlayStatus();
        }

        if (Button("Play Attached") && TryGetService(out var attachService) && RequireConfiguredEffectKey() && RequireAttachTarget())
        {
            _lastEffect = attachService.Play(
                _effectKey,
                _attachTarget.position,
                _attachTarget.rotation,
                _attachTarget);

            SetPlayStatus();
        }

        if (Button("Stop Last Effect"))
        {
            StopLastEffect();
        }

        EndHorizontal();

        Space();
        DrawEffectKeys();

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }
    }

    private void DrawEffectKeys()
    {
        string[] effectKeys = GetEffectKeys();

        BoldLabel("Configured Effect Keys");

        if (effectKeys.Length == 0)
        {
            Info("등록된 effect key가 없습니다.");
            return;
        }

        _scroll = BeginScrollView(_scroll, GUILayout.Height(160f));

        foreach (var key in effectKeys)
        {
            Label(key);
        }

        EndScrollView();
    }

    private string[] GetEffectKeys()
    {
        var database = GetPrivateField<EffectDatabase>(_effectManager, "_effectDatabase");

        if (database == null || database.ParticleSystemDic == null)
            return Array.Empty<string>();

        return database.ParticleSystemDic.Keys
            .Where(key => !string.IsNullOrWhiteSpace(key))
            .OrderBy(key => key)
            .ToArray();
    }

    private void StopLastEffect()
    {
        if (_lastEffect == null)
            return;

        _lastEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _status = "Stopped last effect.";
        _lastEffect = null;
    }

    private void SetPlayStatus()
    {
        if (_lastEffect == null)
        {
            _status = $"Effect key를 찾지 못했습니다: {_effectKey}";
            Debug.LogError(_status);
            return;
        }

        _status = $"Played effect: {_effectKey}";
        Debug.Log(_status);
    }

    private bool TryGetService(out IEffectService service)
    {
        service = _effectManager;

        if (service != null)
            return true;

        try
        {
            service = Core.Effect;
            return service != null;
        }
        catch (Exception e)
        {
            _status = $"Effect 서비스가 준비되지 않았습니다. {e.Message}";
            Debug.LogError(_status);
            return false;
        }
    }

    private bool RequireEffectKey()
    {
        if (!string.IsNullOrWhiteSpace(_effectKey))
            return true;

        _status = "Effect Key를 입력하세요.";
        Debug.LogError(_status);
        return false;
    }

    private bool RequireConfiguredEffectKey()
    {
        if (!RequireEffectKey())
            return false;

        if (GetEffectKeys().Contains(_effectKey))
            return true;

        _status = $"등록되지 않은 Effect Key입니다: {_effectKey}";
        Debug.LogError(_status);
        return false;
    }

    private bool RequireAttachTarget()
    {
        if (_attachTarget != null)
            return true;

        _status = "Attach Target을 지정하세요.";
        Debug.LogError(_status);
        return false;
    }

    private static T GetPrivateField<T>(object target, string fieldName) where T : class
    {
        if (target == null)
            return null;

        var field = target.GetType().GetField(fieldName, BindingFlag);
        return field?.GetValue(target) as T;
    }
}
