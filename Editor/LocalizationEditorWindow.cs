using System;
using System.Linq;
using System.Reflection;
using PhikozzLib;
using PhikozzLib.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// 테스트용 Localization 서비스를 조회하고 locale 전환을 확인하는 에디터 창입니다.
/// </summary>
public class LocalizationEditorWindow : BaseEditorWindow
{
    private static readonly BindingFlags BindingFlag =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private LocalizationManager _localizationManager;
    private string _localeCode = string.Empty;
    private string _tableReference = string.Empty;
    private string _entryReference = string.Empty;
    private string _localizedValue = "-";
    private string _status = string.Empty;
    private Vector2 _scroll;
    private string _initialLocaleCode = string.Empty;

    [MenuItem("PhikozzLib/Localization Editor Window")]
    private static void OpenWindow()
    {
        Open<LocalizationEditorWindow>("Localization Editor");
    }

    private void OnDisable()
    {
        if (!Application.isPlaying || string.IsNullOrWhiteSpace(_initialLocaleCode))
            return;

        if (TryGetService(out var service, false))
        {
            service.SetLocale(_initialLocaleCode);
        }
    }

    protected override void DrawGUI()
    {
        TitleLabel("Localization Editor");
        Space();

        if (!Application.isPlaying)
        {
            Warning("Play Mode에서만 사용할 수 있습니다.");
            return;
        }

        _localizationManager = ObjectField(
            "LocalizationManager",
            _localizationManager != null ? _localizationManager : FindAnyObjectByType<LocalizationManager>(),
            true);

        DrawLocaleStatus();
        Space();

        _localeCode = TextField("Locale Code", _localeCode);
        _tableReference = TextField("Table", _tableReference);
        _entryReference = TextField("Entry", _entryReference);

        Space();

        BeginHorizontal();

        if (Button("Set Locale") && TryGetService(out var localeService) && RequireLocaleCode())
        {
            localeService.SetLocale(_localeCode);
            _status = $"Set locale: {_localeCode}";
            Debug.Log(_status);
        }

        if (Button("Get String") && TryGetService(out var stringService) && RequireTableAndEntry())
        {
            _localizedValue = stringService.GetString(_tableReference, _entryReference);
            _status = $"Loaded localized string: {_tableReference}/{_entryReference}";
            Debug.Log(_status);
        }

        EndHorizontal();

        Space();

        BeginBox();
        BoldLabel("Localized Result");
        Label(_localizedValue);
        EndBox();

        if (!string.IsNullOrEmpty(_status))
        {
            Space();
            Info(_status);
        }
    }

    private void DrawLocaleStatus()
    {
        BeginBox();
        BoldLabel("Locale Status");

        var settings = GetLocalizationSettings();
        if (settings == null)
        {
            Warning("LocalizationSettings 참조를 찾을 수 없습니다.");
            EndBox();
            return;
        }

        string selectedLocaleCode = settings.SelectedLocale != null
            ? settings.SelectedLocale.Identifier.Code
            : "-";

        if (string.IsNullOrWhiteSpace(_initialLocaleCode) && selectedLocaleCode != "-")
            _initialLocaleCode = selectedLocaleCode;

        Label("Selected Locale", selectedLocaleCode);
        Label("Available Locales", settings.GetAvailableLocales() != null
            ? settings.GetAvailableLocales().Locales.Count.ToString()
            : "0");
        EndBox();

        string[] localeCodes = GetLocaleCodes(settings);
        if (localeCodes.Length == 0)
        {
            Info("등록된 locale code가 없습니다.");
            return;
        }

        BoldLabel("Available Locale Codes");
        _scroll = BeginScrollView(_scroll, GUILayout.Height(120f));

        foreach (var localeCode in localeCodes)
        {
            Label(localeCode);
        }

        EndScrollView();
    }

    private LocalizationSettings GetLocalizationSettings()
    {
        if (_localizationManager == null)
            return null;

        var field = typeof(LocalizationManager).GetField("_localizationSettings", BindingFlag);
        return field?.GetValue(_localizationManager) as LocalizationSettings;
    }

    private static string[] GetLocaleCodes(LocalizationSettings settings)
    {
        if (settings?.GetAvailableLocales() == null)
            return Array.Empty<string>();

        return settings.GetAvailableLocales()
            .Locales
            .Where(locale => locale != null)
            .Select(locale => locale.Identifier.Code)
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Distinct()
            .OrderBy(code => code)
            .ToArray();
    }

    private bool TryGetService(out ILocalizationService service, bool logError = true)
    {
        service = _localizationManager;

        if (service != null)
            return true;

        try
        {
            service = Core.Local;
            return service != null;
        }
        catch (Exception e)
        {
            if (logError)
            {
                _status = $"Localization 서비스가 준비되지 않았습니다. {e.Message}";
                Debug.LogError(_status);
            }

            return false;
        }
    }

    private bool RequireLocaleCode()
    {
        if (!string.IsNullOrWhiteSpace(_localeCode))
            return true;

        _status = "Locale Code를 입력하세요.";
        Debug.LogError(_status);
        return false;
    }

    private bool RequireTableAndEntry()
    {
        if (string.IsNullOrWhiteSpace(_tableReference))
        {
            _status = "Table을 입력하세요.";
            Debug.LogError(_status);
            return false;
        }

        if (string.IsNullOrWhiteSpace(_entryReference))
        {
            _status = "Entry를 입력하세요.";
            Debug.LogError(_status);
            return false;
        }

        return true;
    }
}
