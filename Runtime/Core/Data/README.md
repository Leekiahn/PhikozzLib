# DataManager

> `BaseData`를 상속받는 데이터 클래스를 `DataContainer<T>`에 담아 타입별로 등록/조회합니다.  
> `IDataService` 인터페이스를 상속받아 구현합니다.

<br>

## 주요 기능

- 타입별 `DataContainer<T>` 등록 및 조회
- 이름(`string`) 기준 개별 데이터 조회, 전체 조회

<br>

## 관련 타입

| Type | Role |
| --- | --- |
| `BaseData` | 모든 데이터 모델의 공통 베이스 클래스. `Name`만 가집니다 |
| `DataContainer<T>` | 타입별 데이터 조회/보관 컨테이너 |

<br>

## Public API

### `DataContainer<T>`

| Method | Description |
|-----|-----|
| `Get(string name)` | Name으로 데이터를 조회합니다. |
| `GetAll()` | 전체 데이터를 반환합니다. |

### `IDataService`

| Method | Description |
|-----|-----|
| `AddDataContainer<T>(DataContainer<T> container)` | `DataContainer`를 타입 기준으로 등록합니다. |
| `GetDataContainer<T>()` | 타입으로 `DataContainer`를 조회합니다. 등록된 적 없으면 `null`을 반환합니다. |

<br>

## Effect 모듈과 같은 이유로, 데이터 소스는 패키지 밖에서 채운다

`DataManager`는 `AddDataContainer`/`GetDataContainer`만 제공하고, 데이터를 실제로 어디서 가져오는지 전혀 모릅니다. 이 패키지는 git URL로 설치되므로, 특정 프로젝트에서만 존재하는 BGDatabase 생성 코드(`BG_TestData` 등)를 패키지가 직접 참조하면 다른 프로젝트에서는 컴파일이 깨집니다.

그래서 `ExampleDataLoader.cs`는 **전체가 주석 처리된 예시 코드**로만 존재합니다. 실제로 쓰려면 이 내용을 프로젝트 쪽 스크립트로 복사해서 주석을 풀고 쓰세요.

```csharp
// ExampleDataLoader.cs 내용
public class TestData : BaseData
{
    public int Power { get; private set; }

    public TestData(BG_TestData data) : base(data.name)
    {
        Power = data.power;
    }
}

[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
private static void LoadData()
{
    var dataService = ServiceLocator.Get<IDataService>();
    var list = new List<TestData>();

    BG_TestData.ForEachEntity(data => list.Add(new TestData(data)));

    dataService.AddDataContainer(new DataContainer<TestData>(list));
}
```

BGDatabase가 아니어도 상관없습니다 — `BaseData`를 상속한 클래스의 리스트만 만들 수 있으면, `AddDataContainer`로 등록하면 됩니다.

<br>

## BGDatabase로 데이터를 만드는 방법 (Google Sheets 연동 예시)

<img width="759" height="403" alt="Image" src="https://github.com/user-attachments/assets/b39f403f-97eb-4abf-9cd7-9b4ccca9b594" />

- 구글 스프레드시트를 작성합니다. `name`(A1)열은 필수로 존재해야 합니다(BGDatabase 엔티티의 내장 이름 필드).

<br>

<img width="1143" height="532" alt="Image" src="https://github.com/user-attachments/assets/c7e3da59-946f-4d7f-a4a9-6f3ad44782b0" />

- BGDatabase -> Export/Import -> Data Sources에서 구글 스프레드 시트를 추가합니다.

<br>

<img width="1140" height="524" alt="Image" src="https://github.com/user-attachments/assets/6f1f1e6f-de2c-408a-97ea-60ae7704f581" />

- 원하는 데이터소스 타입으로 `SpreadSheet ID`를 얻습니다.

<br>

<img width="1141" height="526" alt="Image" src="https://github.com/user-attachments/assets/9b1e95eb-97b8-427e-9799-d0c6825b226b" />

<img width="1143" height="528" alt="Image" src="https://github.com/user-attachments/assets/0914800a-794e-4ff3-89a2-cb4aae30c016" />

- 똑같이 Jobs를 추가한 후, DataSource를 선택합니다.
- `Update Ids on Import`는 체크 해제합니다.
- Merge Mode를 Transfer로 설정합니다.
- Import -> Save -> CodeGen을 클릭합니다.

<br>

<img width="1140" height="521" alt="Image" src="https://github.com/user-attachments/assets/668aac65-f815-46df-9670-d7974c947ab5" />

<img width="1143" height="525" alt="Image" src="https://github.com/user-attachments/assets/fcfbb1f7-4759-454d-b049-5cce832edb82" />

- Configuration Metas에 시트를 추가하고 모든 열을 등록해줍니다.
- Import -> Save -> CodeGen을 클릭하면 Database에 시트가 추가되었습니다.

<br>

## 사용 예시

```csharp
private IDataService _dataService;

private void Start()
{
    _dataService = ServiceLocator.Get<IDataService>();
}

public void DataTest()
{
    var data = _dataService.GetDataContainer<TestData>().Get("Sword");
}
```
