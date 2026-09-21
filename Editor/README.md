# BootstrapConfigValidator

> `BootstrapConfig`에 등록된 매니저 목록을 검사해서, `Bootstrapper`가 런타임에 예외를 던지기 전에  
> 에디터 단계에서 미리 문제를 잡아주는 검증 툴입니다.  
> 순수 에디터 전용 코드이며 `EditorAssembly.asmdef`에 속해 플레이어 빌드에는 포함되지 않습니다.

<br>

## 왜 필요한가

`Bootstrapper.Init()`은 `BootstrapConfig.Managers` 리스트를 순회하며 각 프리팹을 `Instantiate`하고  
`GetComponent<IServiceRegister>().RegisterService()`를 호출합니다.

다음과 같은 실수는 지금까지 **런타임에 NullReferenceException이 터져야만** 발견됐습니다.

- `Managers` 리스트에 빈(null) 슬롯이 있음 (프리팹을 지웠는데 리스트에서 안 뺐을 때)
- 같은 프리팹이 리스트에 중복 등록됨
- 등록한 프리팹에 `IServiceRegister`를 구현한 컴포넌트가 없음

`BootstrapConfigValidator`는 이 세 가지를 에디터가 켜지거나 스크립트가 재컴파일될 때마다 자동으로 검사해서,  
플레이 버튼을 누르기 전에 콘솔 경고로 미리 알려줍니다.

<br>

## 언제 실행되는가

| 트리거 | 설명 |
| --- | --- |
| `[InitializeOnLoad]` | 에디터 시작 시, 스크립트 재컴파일(도메인 리로드) 시마다 자동 실행 |
| `PhikozzLib > Validate Bootstrap Config` 메뉴 | 코드 변경 없이 `BootstrapConfig.asset`만 수정했을 때 수동으로 재검사 |

<br>

## 검사 항목

| 항목 | 조건 | 의미 |
| --- | --- | --- |
| Config 존재 여부 | `Resources/BootstrapConfig.asset`을 찾지 못함 | `Bootstrapper`가 아무 서비스도 등록하지 못함 |
| 빈 슬롯 | `Managers[i] == null` | `Bootstrapper`가 `Instantiate(null)`로 NRE |
| 중복 등록 | 같은 프리팹이 리스트에 두 번 이상 존재 | 같은 서비스가 중복 생성/등록됨 |
| `IServiceRegister` 미구현 | 프리팹에 `GetComponent<IServiceRegister>()`가 `null` | `Bootstrapper`가 `RegisterService()` 호출 시 NRE |

문제를 찾으면 `[PhikozzLib]` 접두사를 붙인 `Debug.LogWarning`을 콘솔에 남깁니다.

<br>

## 한계

- `IServiceInit.InitAsync()` 내부에서 어떤 서비스를 조회하는지는 코드 분석 없이는 알 수 없어서, 서비스 간 의존 관계(예: A가 B를 조회하는데 B가 아예 등록 목록에 없음)까지는 검사하지 않습니다.
- 검사 대상은 오직 `BootstrapConfig.asset` 하나이며, 씬에 직접 배치된 서비스는 대상이 아닙니다.
