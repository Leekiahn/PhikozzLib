# DataManager
> `BaseData.cs`를 상속받는 데이터 클래스를 `DataContainer<T>`에 담아 초기화/로드하고 ID/이름 조회합니다.  
> `IDataService` 인터페이스를 상속받아 구현합니다.  
> 현재 `BG Database` 에셋에 의존합니다.  

<br>

## 주요 기능

- 게임 시작 시 데이터 로드
- 원본 데이터 엔티티를 런타임 데이터 객체로 변환
- 타입별 데이터 컨테이너 초기화
- Id, Name 기준 빠른 조회 제공
- 전체 데이터 컬렉션 순회 지원

<br>

## 관련 타입

| Type | Role |
| --- | --- |
| `BaseData` |  모든 데이터 모델의 공통 베이스 클래스  |
| `LocalizedBaseData ` | 로컬라이징 참조 정보를 포함하는 데이터 베이스 클래스 |
| `DataContainer<T>` | 타입별 데이터 조회/보관 컨테이너 |

<br>

## Public API

### `DataContainter<T>`

| Method | Description |
|-----|-----|
| ` Get(int id) ` |  Id로 데이터를 조회합니다.  |
| ` Get(string name) `|  Name으로 데이터를 조회합니다.  |
| ` GetAll() ` |  전체 데이터를 반환합니다.  |

### `DataContainter<T>`

| Method | Description |
|-----|-----|
| ` Register<T>(DataContainer<T> container) ` |  DataContainer를 등록합니다.  |
| ` DataContainer<T> GetContainer<T>() `|  Containter를 타입으로 조회합니다.  |


<br>

## Setup

<img width="759" height="403" alt="Image" src="https://github.com/user-attachments/assets/b39f403f-97eb-4abf-9cd7-9b4ccca9b594" />

- 구글 스프레드시트를 작성합니다.
- name(A1)열과 id(B1)열은 필수로 존재해야 합니다.
- `LocalizedBaseData`를 상속한다면 C1열과 D1열에 각각 테이블 참조(string)과 엔트리 참조(string)가 존재해야 합니다.

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

```csharp
public class TestData : LocalizedBaseData
{
    public int Power { get; private set; }
    
    public TestData(BG_TestData data) : base(data.id, data.name, data.locale_table_ref, data.locale_entry_ref)
    {
        Power = data.power;
    }
}
```
- `BaseData` 혹은 `LocalizedBaseData`를 상속받아 Data클래스를 작성합니다.
- `ExampleDataLoader`코드를 참고해 `DataLoader` 스크립트를 작성하세요.

<br>

```csharp
private IDataService _dataService;

private void Awake()
{
    _dataService = ServiceLocator.Get<IDataService>();
}

public void DataTest()
{
    var data = _dataService.GetContainer<TestData>().Get(1);
 }
```
- 위처럼 전역적으로 데이터 조회가 가능합니다.

<br>

<img width="890" height="654" alt="Image" src="https://github.com/user-attachments/assets/a8178e6a-d760-4899-a0e1-b09d6740c739" />

- `LocalizedBaseData`를 상속받았다면 Unity Localization의 Table Name과 Entry Key로 언어 대응이 가능합니다.  
[Local](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Localization) 


