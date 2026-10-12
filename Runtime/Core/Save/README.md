# SaveManager

> 데이터를 key 단위 파일로 저장·로드하는 서비스입니다. JSON(`JsonUtility`)과 Binary(Odin)를 지원합니다. (`ISaveService`)

## Public API

| Method | Description |
|---|---|
| `Save<T>(string key, T data)` | 동기로 저장합니다. 실패하면 예외를 던집니다. |
| `SaveAsync<T>(string key, T data)` | 호출 시점의 데이터를 저장합니다. 파일 쓰기만 스레드풀에서 합니다. |
| `Load<T>(string key, out T data)` | 불러오고 결과를 `Success` / `NotFound` / `Corrupted`로 반환합니다. |
| `Exists(string key)` | 저장 파일이 있는지 확인합니다. |
| `Delete(string key)` | 저장 파일 하나를 삭제합니다. |
| `DeleteFolder(string folder)` | 폴더(슬롯)를 통째로 삭제합니다. |
| `DeleteAll()` | 모든 저장 파일을 삭제합니다. |

## 사용 예시

```csharp
[System.Serializable]
public class PlayerSaveData
{
    public int Level;
    public float Hp;
}

public void Save()
{
    _saveService.Save("Slot1/Player", new PlayerSaveData { Level = 5, Hp = 80f });
}

public void Load()
{
    switch (_saveService.Load<PlayerSaveData>("Slot1/Player", out var data))
    {
        case eSaveLoadResult.Success:
            ApplySaveData(data);
            break;
        case eSaveLoadResult.NotFound:
            StartNewGame();
            break;
        case eSaveLoadResult.Corrupted:
            _uiService.OpenPopup<SaveCorruptedPopup>(); // 손상 안내는 게임 쪽에서 처리
            break;
    }
}
```

## 주의사항

- 파일 경로는 `persistentDataPath/{Save Directory}/{key}.json|bin`입니다. key에 `/`를 넣으면 폴더가 생기므로 `"Slot1/Player"`처럼 슬롯을 구분하고 `DeleteFolder("Slot1")`로 슬롯을 지웁니다.
- **JSON은 `[Serializable]` 클래스·구조체만 저장됩니다.** `int`, `string`, `Dictionary`, 최상위 `List`를 저장하려면 Binary를 씁니다.
- **Binary를 IL2CPP 빌드에서 쓰면** `Tools > Odin > Preferences > Serialization > AOT Generation`에서 Scan Project → Generate DLL을 해둡니다. 안 하면 빌드에서만 저장/로드가 실패할 수 있습니다.
- 같은 key로 `SaveAsync`를 연달아 호출하면 같은 파일을 동시에 써서 충돌할 수 있습니다. 앞의 저장을 `await`한 뒤 호출합니다.
- 빈 파일(저장 중 앱 종료 등)도 `Corrupted`로 처리하며, 손상 시 `Debug.LogError`를 남깁니다.
