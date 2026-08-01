using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 테스트용 FloatingText 서비스를 실행하고 설정된 타입 목록을 확인하는 에디터 창입니다.
/// </summary>
public class FloatingTextEditorWindow : BaseEditorWindow
{
    private static readonly BindingFlags BindingFlag =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private FloatingTextManager _floatingTextManager;
    private eFloatingTextType _type = eFloatingTextType.Default;
    private string _value = "123";
    private Vector3 _position;
    private Vector3 _direction = Vector3.up;
    private string _status = string.Empty;

    [MenuItem("PhikozzLib/Floating Text Editor Window")]
    private static void OpenWindow()
    {
        Open<FloatingTextEditorWindow>("Floating Text Editor");
    }

    protected override void DrawGUI()
    {
        TitleLabel("Floating Text Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _floatingTextManager = ObjectField(
            "FloatingTextManager",
            _floatingTextManager != null ? _floatingTextManager : FindAnyObjectByType<FloatingTextManager>(),
            true);

        _type = EnumField("Type", _type);
        _value = TextField("Value", _value);
        _position = Vector3Field("Position", _position);
        _direction = Vector3Field("Direction", _direction);

        Space();

        if (Button("Spawn") && TryGetManager(out var manager) && RequireValue() && IsConfiguredType(manager, _type))
        {
            manager.Spawn(_type, _value, _position, _direction);
            _status = $"Spawned floating text: {_type} / {_value}";
            Debug.Log(_status);
        }

        Space();
        DrawConfiguredTypes();

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }
    }

    private void DrawConfiguredTypes()
    {
        BoldLabel("Configured Types");

        if (_floatingTextManager == null)
        {
            Info("FloatingTextManager를 지정하면 설정된 타입을 표시합니다.");
            return;
        }

        var configuredTypes = GetConfiguredTypes(_floatingTextManager);
        if (configuredTypes.Count == 0)
        {
            Info("등록된 FloatingText 타입이 없습니다.");
            return;
        }

        foreach (var configuredType in configuredTypes)
        {
            Label(configuredType.ToString());
        }
    }

    private bool TryGetManager(out FloatingTextManager manager)
    {
        manager = _floatingTextManager != null ? _floatingTextManager : FindAnyObjectByType<FloatingTextManager>();

        if (manager != null)
            return true;

        try
        {
            manager = Core.FloatingText;
            return manager != null;
        }
        catch (Exception e)
        {
            _status = $"FloatingText 서비스가 준비되지 않았습니다. {e.Message}";
            Debug.LogError(_status);
            return false;
        }
    }

    private bool RequireValue()
    {
        if (!string.IsNullOrWhiteSpace(_value))
            return true;

        _status = "Value를 입력하세요.";
        Debug.LogError(_status);
        return false;
    }

    private bool IsConfiguredType(FloatingTextManager manager, eFloatingTextType type)
    {
        if (GetConfiguredTypes(manager).Contains(type))
            return true;

        _status = $"설정되지 않은 FloatingText 타입입니다: {type}";
        Debug.LogError(_status);
        return false;
    }

    private static HashSet<eFloatingTextType> GetConfiguredTypes(FloatingTextManager manager)
    {
        var result = new HashSet<eFloatingTextType>();

        if (manager == null)
            return result;

        var field = typeof(FloatingTextManager).GetField("_floatingTextSpawners", BindingFlag);
        var entries = field?.GetValue(manager) as IEnumerable;
        if (entries == null)
            return result;

        foreach (var entry in entries)
        {
            if (entry == null)
                continue;

            var typeProperty = entry.GetType().GetProperty("Type", BindingFlag);
            var spawnerProperty = entry.GetType().GetProperty("Spawner", BindingFlag);

            if (typeProperty?.GetValue(entry) is eFloatingTextType configuredType &&
                spawnerProperty?.GetValue(entry) != null)
            {
                result.Add(configuredType);
            }
        }

        return result;
    }
}
