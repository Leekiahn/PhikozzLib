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
| `GetString(string localeTableRef, string localeEntryRef, params object[] arguments)` | 테이블과 엔트리로 문자열을 가져옵니다. Smart String 파라미터가 필요하면 `arguments`로 넘깁니다. |
| `GetAssetAsync<T>(string localeTableRef, string localeEntryRef)` | 로컬라이즈된 에셋을 비동기로 가져옵니다. |

> `GetString`은 호출한 시점의 문자열만 반환합니다. 로케일이 바뀔 때 자동으로 갱신되어야 하는 UI는 Unity의 `LocalizeStringEvent` 컴포넌트를 쓰거나, `LocalizedString` 필드를 직접 들고 `OnEnable`/`OnDisable`에서 `StringChanged`를 구독/해제하세요.

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

public string GetGoldText(int gold)
{
    // Smart String 엔트리 예: "{0} 골드"
    return _localizationService.GetString("UITable", "GoldFormat", gold);
}

public string GetRewardText(string playerName, int gold, int exp)
{
    // Smart String 엔트리 예: "{0}님이 {1} 골드와 {2} 경험치를 획득했습니다."
    // 인자는 넘긴 순서대로 {0}, {1}, {2}에 들어갑니다.
    return _localizationService.GetString("UITable", "RewardFormat", playerName, gold, exp);
}
```

### 인자 포맷 규칙

- `{0}`, `{1}` 같은 **인덱스 자리표시자**는 String Table 엔트리의 **Smart** 옵션과 관계없이 동작합니다. (Smart가 꺼져 있으면 `string.Format`으로 처리)
- 복수형(`{0:plural:...}`), 조건 분기(`{0:cond:...}`) 같은 **Smart String 전용 문법**은 엔트리의 **Smart** 옵션을 켜야 동작합니다. 꺼져 있으면 `string.Format`이 해석하지 못해 포맷 에러가 납니다.
- Smart가 꺼진 엔트리에 인자를 넘기면서 `{`, `}` 문자를 그대로 표시하려면 `{{`, `}}`로 이스케이프해야 합니다. (인자 없이 호출하면 원문 그대로 반환)
