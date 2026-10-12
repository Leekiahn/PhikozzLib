# LocalizationManager

<img width="611" height="183" alt="Image" src="https://github.com/user-attachments/assets/937b3920-c153-4b3b-966e-f5e3dfe77a43" />

> Unity Localization 기반으로 로케일 변경과 테이블 문자열·에셋 조회를 제공하는 서비스입니다. (`ILocalizationService`)

## Public API

| Method | Description |
|---|---|
| `SetLocale(string localeCode)` | 로케일을 변경합니다. (예: `"ko"`, `"en"`) |
| `GetString(string localeTableRef, string localeEntryRef, params object[] arguments)` | 테이블 문자열을 가져옵니다. 인자는 `{0}`, `{1}`… 순서로 들어갑니다. |
| `GetAssetAsync<T>(string localeTableRef, string localeEntryRef)` | 로컬라이즈된 에셋을 비동기로 가져옵니다. |

## 사용 예시

```csharp
private ILocalizationService _localizationService;

private void Start()
{
    _localizationService = ServiceLocator.Get<ILocalizationService>();
}

public void OnClickEnglishButton()
{
    _localizationService.SetLocale("en");
}

public string GetGoldText(int gold)
{
    // 엔트리 예: "{0} 골드"
    return _localizationService.GetString("UITable", "GoldFormat", gold);
}
```

## 주의사항

- **`GetString`은 호출 시점의 문자열만 반환합니다.** 로케일이 바뀔 때 자동 갱신돼야 하는 UI는 `LocalizeStringEvent` 컴포넌트를 씁니다.
- 복수형(`{0:plural:...}`) 같은 Smart String 문법은 엔트리의 **Smart** 옵션을 켜야 동작합니다.
- Smart가 꺼진 엔트리에 인자를 넘길 때 `{`, `}`를 그대로 표시하려면 `{{`, `}}`로 씁니다.
