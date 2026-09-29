# SaveManager

> 저장/로드 서비스를 제공합니다.  
> `ISaveService` 인터페이스를 상속받아 구현합니다.

<br>

## 주요 기능

- 제네릭 기반 데이터 저장/로드
- JSON 저장 지원 (`JsonUtility`)
- Binary 저장 지원 (Odin `SerializationUtility`)
- 비동기 저장(`SaveAsync`) 지원
- 폴더 키(`"Slot1/Player"`)로 슬롯 구분
- 존재 확인 / 단일 키 삭제 / 폴더(슬롯) 삭제 / 전체 삭제

<br>

## Public API

| Method | Description |
|---|---|
| `Save<T>(string key, T data)` | 데이터를 동기적으로 저장합니다. 실패하면 예외를 던집니다. |
| `SaveAsync<T>(string key, T data)` | 호출 시점에 직렬화를 끝내고, 파일 쓰기만 스레드풀에서 비동기로 합니다. |
| `Load<T>(string key, out T data)` | 키에 해당하는 데이터를 불러오고 결과를 `eSaveLoadResult`(`Success` / `NotFound` / `Corrupted`)로 반환합니다. |
| `Exists(string key)` | 키에 해당하는 저장 파일이 있는지 확인합니다. 슬롯 선택 화면의 빈 슬롯 표시 등에 사용합니다. |
| `Delete(string key)` | 특정 키의 저장 파일을 삭제합니다. |
| `DeleteFolder(string folder)` | 폴더 안의 저장 파일을 모두 삭제합니다. 슬롯 삭제에 사용합니다. |
| `DeleteAll()` | 저장 폴더 안의 모든 파일과 하위 폴더(슬롯 포함)를 삭제합니다. |

<br>

## 저장 타입

`_saveType` 필드(Json/Binary)에 따라 파일 확장자와 직렬화 방식이 바뀝니다. 파일 경로는 `Application.persistentDataPath/{_saveDirectory}/{key}.{json|bin}`입니다.

| 타입 | 저장 가능한 데이터 |
|---|---|
| `Json` (`JsonUtility`) | `[Serializable]` 클래스·구조체만 가능합니다. `int`, `string`을 바로 넣거나 `Dictionary`, 최상위 `List`는 저장되지 않습니다. |
| `Binary` (Odin) | 위 타입 외에 `Dictionary`, 기본 타입, 다형성 등도 저장할 수 있습니다. |

<br>

## 슬롯 (폴더 키)

키에 `/`로 폴더 경로를 넣으면 하위 폴더에 저장됩니다. 슬롯은 이 폴더로 구분합니다.

```
Save/
├─ Settings.json          ← _saveService.Save("Settings", ...)       공용 데이터
├─ Slot1/
│  ├─ Player.json         ← _saveService.Save("Slot1/Player", ...)
│  └─ Info.json           ← _saveService.Save("Slot1/Info", ...)      슬롯 선택 화면용 정보
└─ Slot2/
   └─ Player.json
```

```csharp
bool hasSave = _saveService.Exists("Slot1/Player"); // 빈 슬롯 표시
_saveService.DeleteFolder("Slot1");                 // 슬롯 1만 삭제, 공용 데이터는 유지
```

- 슬롯 개념을 라이브러리에 직접 넣지 않고 "폴더"로 풀었기 때문에, 슬롯 번호 방식이든 이름 방식이든 게임이 자유롭게 정할 수 있습니다.
- 슬롯 선택 화면에 보여줄 정보(저장 시각, 플레이 시간, 레벨 등)는 게임마다 다르므로, 게임 쪽에서 `Slot1/Info` 같은 키로 따로 저장합니다.
- 폴더는 저장할 때만 만들어집니다. `Load` / `Exists`만 호출해서는 빈 슬롯 폴더가 생기지 않습니다.

<br>

## `SaveAsync`의 저장 시점

`SaveAsync`는 **호출한 순간의 데이터**를 저장합니다. 직렬화는 메인 스레드에서 즉시 끝내고, 파일 쓰기만 스레드풀에서 합니다.

```csharp
_saveService.SaveAsync("Slot1/Player", playerData).Forget();
playerData.Level = 99; // 이미 직렬화가 끝났으므로 저장 결과에 영향 없음
```

직렬화까지 스레드풀에서 하면, 그 사이 메인 스레드에서 데이터가 바뀌었을 때 반쯤 바뀐 상태가 저장될 수 있기 때문입니다.

<br>

## `Load` 결과

`Load`는 성공/실패뿐 아니라 **실패 이유**를 `eSaveLoadResult`로 돌려줍니다. "파일이 없음"과 "파일이 손상됨"은 게임에서 처리해야 할 방식이 다르기 때문입니다.

| 결과 | 상황 | 로그 | 게임 쪽 처리 예 |
|---|---|---|---|
| `Success` | 정상적으로 불러옴 | 없음 | 데이터 적용 |
| `NotFound` | 세이브 파일이 없음 (첫 실행 등) | 없음 | 새 게임 시작 |
| `Corrupted` | 파일이 있는데 손상됨, 타입 불일치, 빈 파일 | `Debug.LogError` | 플레이어에게 손상 안내 팝업 |

- **빈 파일도 `Corrupted`로 처리합니다.** 저장 도중 앱이 종료되면 0바이트 파일이 남을 수 있는데, JSON 파서는 이 경우 예외 없이 `null`을 돌려주므로 따로 검사합니다.
- **손상 로그는 `DevLog`가 아니라 `Debug.LogError`를 사용합니다.** 실제 플레이어 환경에서 생기는 데이터 손실이라, 릴리스 빌드의 `Player.log`와 크래시 리포트 도구에도 남아야 하기 때문입니다.
- **플레이어에게 보여주는 팝업은 라이브러리가 띄우지 않습니다.** 문구·디자인·선택지가 게임마다 다르므로, `Corrupted` 결과를 받은 게임 쪽에서 처리합니다.

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
    switch (_saveService.Load<PlayerSaveData>("player", out var data))
    {
        case eSaveLoadResult.Success:
            ApplySaveData(data);
            break;
        case eSaveLoadResult.NotFound:
            StartNewGame();
            break;
        case eSaveLoadResult.Corrupted:
            _uiService.OpenPopup<SaveCorruptedPopup>(); // 플레이어에게 손상 안내
            break;
    }
}
```
