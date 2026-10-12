# DataManager

> `BaseData`를 상속한 데이터를 `DataContainer<T>`에 담아 타입별로 등록·조회하는 서비스입니다. (`IDataService`)

## Public API

| Member | Description |
|---|---|
| `IDataService.AddDataContainer<T>(DataContainer<T> container)` | 컨테이너를 타입 기준으로 등록합니다. |
| `IDataService.GetDataContainer<T>()` | 타입으로 컨테이너를 조회합니다. 미등록이면 `null`. |
| `DataContainer<T>.Get(string name)` | `Name`으로 데이터를 조회합니다. |
| `DataContainer<T>.GetAll()` | 전체 데이터를 반환합니다. |

## 사용 예시

**등록** (`ExampleDataLoader.cs`를 프로젝트로 복사해서 수정)

```csharp
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
    var list = new List<TestData>();
    BG_TestData.ForEachEntity(data => list.Add(new TestData(data)));

    ServiceLocator.Get<IDataService>().AddDataContainer(new DataContainer<TestData>(list));
}
```

**조회**

```csharp
var sword = _dataService.GetDataContainer<TestData>().Get("Sword");
```

## 주의사항

- `ExampleDataLoader.cs`는 전체가 주석 처리된 예시입니다. 패키지가 프로젝트별 코드(BGDatabase 생성 클래스 등)를 직접 참조하지 않게 하기 위해서입니다.
- 데이터 소스는 BGDatabase가 아니어도 됩니다. `BaseData`를 상속한 리스트만 만들면 등록할 수 있습니다.
