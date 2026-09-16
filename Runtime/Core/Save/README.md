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
| `TryLoad<T>(string key, out T data)` | 키에 해당하는 데이터를 불러옵니다. 실패하면 `false`를 반환하고 경고 로그를 남깁니다. |
| `Delete(string key)` | 특정 키의 저장 파일을 삭제합니다. |
| `DeleteAll()` | 저장된 모든 파일을 삭제합니다. |

<br>

## 저장 타입

`_saveType` 필드(Json/Binary)에 따라 파일 확장자와 직렬화 방식이 바뀝니다. 파일 경로는 `Application.persistentDataPath/{_saveDirectory}/{key}.{json|bin}`입니다.

<br>

## `TryLoad` 실패 시 원인을 알 수 있습니다

`TryLoad`는 파일이 없거나, 손상됐거나, 타입이 안 맞아 역직렬화에 실패하면 예외를 삼키고 `false`를 반환합니다 — 호출부에서는 `if (TryLoad(...))`로 깔끔하게 분기할 수 있어야 하기 때문입니다. 다만 예외를 완전히 버리면 "왜 로드가 안 됐는지"를 디버깅할 방법이 없어지므로, 내부적으로 `Debug.LogWarning`에 예외 내용을 남깁니다. 정상적인 "파일이 처음이라 없는" 케이스에서도 로그가 남는다는 점은 감안하세요.

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
