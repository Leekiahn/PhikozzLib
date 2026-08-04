# Core.Save

> 저장/로드 서비스를 제공합니다.  
> `ISaveService` 인터페이스를 상속받아 구현합니다.

---

## 주요 기능

- 제네릭 기반 데이터 저장/로드
- JSON 저장 지원
- Binary 저장 지원
- 단일 키 삭제
- 전체 저장 데이터 삭제
- `Application.persistentDataPath/Save` 경로 사용
- 비동기 저장(`SaveAsync`) 지원
- `SaveConfig`를 `Resources`폴더에서 자동으로 참조합니다. 이름을 바꾸지 마세요.

---

## Public API

| Method | Description |
|---|---|
| `Save<T>(string key, T data)` | 데이터를 동기적으로 저장합니다. |
| `SaveAsync<T>(string key, T data)` | 데이터를 비동기적으로 저장합니다. |
| `TryLoad<T>(string key, out T data)` | 키에 해당하는 데이터를 불러옵니다. |
| `Delete(string key)` | 특정 키의 저장 파일을 삭제합니다. |
| `DeleteAll()` | 저장된 모든 파일을 삭제합니다. |

---

## 저장 타입

- `Json`
- `Binary`
