# BootstrapConfigValidator

> `BootstrapConfig`의 설정 실수를 플레이 전에 에디터에서 찾아 콘솔 경고(`[PhikozzLib]`)로 알려주는 검증 툴입니다.

## 검사 항목

| 항목 | 런타임에 생기는 문제 |
|---|---|
| `Resources/BootstrapConfig.asset`이 없음 | 서비스가 하나도 등록되지 않음 |
| `Managers`에 빈(null) 슬롯 | `Instantiate(null)`로 NRE |
| 같은 프리팹이 중복 등록 | 같은 서비스가 두 번 생성·등록됨 |
| 프리팹에 `IServiceRegister` 구현 컴포넌트가 없음 | `RegisterService()` 호출 시 NRE |

## 사용 방법

- 에디터 시작·스크립트 재컴파일 시 자동으로 검사합니다(`[InitializeOnLoad]`).
- `BootstrapConfig.asset`만 수정했을 때는 `PhikozzLib > Validate Bootstrap Config` 메뉴로 다시 검사합니다.

## 주의사항

- 서비스 간 의존 관계(A가 `InitAsync`에서 조회하는 B가 등록 목록에 없음)는 검사하지 않습니다.
- 씬에 직접 배치한 서비스는 검사 대상이 아닙니다.
