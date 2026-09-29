# SaveManager

> 저장/로드 서비스를 제공합니다.  
> `ISaveService` 인터페이스를 상속받아 구현합니다.

<br>

## 주요 기능

- 제네릭 기반 데이터 저장/로드
- JSON 저장 지원 (`JsonUtility`)
- Binary 저장 지원 (Odin `SerializationUtility`)
- 비동기 저장(`SaveAsync`) 지원
- 단일 키 삭제 / 전체 저장 데이터 삭제

<br>

## Public API

| Method | Description |
|---|---|
| `Save<T>(string key, T data)` | 데이터를 동기적으로 저장합니다. 실패하면 예외를 던집니다. |
| `SaveAsync<T>(string key, T data)` | 데이터를 스레드풀에서 비동기적으로 저장합니다. |
| `TryLoad<T>(string key, out T data)` | 키에 해당하는 데이터를 불러옵니다. 파일이 없으면 로그 없이 `false`를, 파일이 있는데 읽기에 실패하면 경고 로그를 남기고 `false`를 반환합니다. |
| `Delete(string key)` | 특정 키의 저장 파일을 삭제합니다. |
| `DeleteAll()` | 저장된 모든 파일을 삭제합니다. |

<br>

## 저장 타입

`_saveType` 필드(Json/Binary)에 따라 파일 확장자와 직렬화 방식이 바뀝니다. 파일 경로는 `Application.persistentDataPath/{_saveDirectory}/{key}.{json|bin}`입니다.

<br>

## `TryLoad` 실패 시 원인을 알 수 있습니다

`TryLoad`는 실패하면 `false`를 반환합니다 — 호출부에서는 `if (TryLoad(...))`로 깔끔하게 분기할 수 있어야 하기 때문입니다. 실패는 두 가지로 나눠 처리합니다.

| 상황 | 성격 | 동작 |
|---|---|---|
| 세이브 파일이 없음 (첫 실행 등) | 정상 | 로그 없이 `false` |
| 파일이 있는데 손상됐거나 타입이 안 맞아 역직렬화 실패 | 문제 | `Debug.LogWarning`에 예외 내용을 남기고 `false` |

손상 경고는 `DevLog`가 아니라 `Debug.LogWarning`을 사용합니다. 실제 플레이어 환경에서 생기는 문제라서, 릴리스 빌드의 `Player.log`에도 원인이 남아야 하기 때문입니다.

<br>

## 사용 예시

```csharp
[System.Serializable]
public class PlayerSaveData
{
    public int Level;
    public float Hp;
}

private ISaveService _saveService;

private void Start()
{
    _saveService = ServiceLocator.Get<ISaveService>();
}

public void Save()
{
    _saveService.Save("player", new PlayerSaveData { Level = 5, Hp = 80f });
}

public void Load()
{
    if (_saveService.TryLoad<PlayerSaveData>("player", out var data))
    {
        Debug.Log($"Loaded level {data.Level}");
    }
}
```
