# LocalizationManager

<img width="611" height="183" alt="Image" src="https://github.com/user-attachments/assets/937b3920-c153-4b3b-966e-f5e3dfe77a43" />

> 로컬라이제이션 서비스와 테이블 기반 문자열/에셋 조회를 제공합니다.  
> `ILocalizationService` 인터페이스를 상속받아 구현합니다.

---

## 주요 기능

- 로케일 변경
- 문자열 로컬라이즈 조회
- Smart String 파라미터 지원
- 로컬라이즈된 에셋 비동기 로드

---

## Public API

| Method | Description |
|---|---|
| `SetLocale(string localeCode)` | 현재 로케일을 변경합니다. Unity Localization의 로케일 코드(예: `"ko"`, `"en"`)를 그대로 씁니다. |
| `GetString(string localeTableRef, string localeEntryRef)` | 테이블과 엔트리로 문자열을 가져옵니다. |
| `GetString(string localeTableRef, string localeEntryRef, LocalizedString.ChangeHandler onChanged, params object[] arguments)` | 변경 콜백 및 Smart String 파라미터를 포함한 문자열을 가져옵니다. |
| `GetAssetAsync<T>(string localeTableRef, string localeEntryRef)` | 로컬라이즈된 에셋을 비동기로 가져옵니다. |

<br>

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

public string GetGreeting()
{
    return _localizationService.GetString("UITable", "Greeting");
}
```
