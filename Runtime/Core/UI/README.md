# UI

> 팝업을 타입 기준으로 열고·닫고·재사용하는 UI 서비스(`IUIService`)와 UI 베이스 클래스입니다.

## Public API

### `IUIService`

| Method | Description |
|---|---|
| `RegisterPopup(UIPopup prefab)` | 팝업 프리팹을 타입 기준으로 등록합니다. |
| `UnregisterPopup(UIPopup prefab)` | 등록을 해제하고, 열린 인스턴스가 있으면 제거합니다. |
| `OpenPopup<T>()` | 팝업을 열고 `T`로 반환합니다. 처음에만 생성하고 이후엔 재사용합니다. 미등록 타입이면 `null`. |
| `ClosePopup<T>()` / `ClosePopup(UIPopup popup)` | 팝업을 닫습니다. |
| `CloseTopPopup()` | 가장 나중에 연 팝업을 닫습니다. 닫을 게 없으면 `false`. |
| `CloseAllPopup()` | 열린 팝업을 모두 닫습니다. |

### 베이스 클래스

| Type | Member | Description |
|---|---|---|
| `UIBase` | `Refresh()` | (필수 구현) 화면을 최신 데이터로 갱신합니다. |
| `UIPopup` | `Open()` / `Close()` | 열기(`Refresh` → `OnOpen` → 열기 Feedback) / 닫기(닫기 Feedback → `OnClose`). |
| | `OnOpen()` / `OnClose()` | 실제 활성화/비활성화. 커스텀 연출이 필요하면 override. |
| | `IsVisible` | 열려 있는지 여부. |
| `UISlot<TData>` | `SetData(TData data)` | 데이터를 넣고 `Refresh()`를 호출합니다. |
| | `Data` | 현재 데이터. |
| | `OnLeftClick()` / `OnRightClick()` | 좌/우클릭 시 호출. 필요한 쪽만 override. |
| `UIModalPanel` | - | 팝업 뒤를 덮는 배경. 클릭하면 소속 팝업을 닫습니다. |

`UIPopup` 인스펙터 필드(`Modal Panel`, `Open/Close Feedback`, `First Selected`)는 모두 선택 사항이며, 비워두면 해당 기능을 쓰지 않습니다.

## 사용 예시

```csharp
public class InventoryPopup : UIPopup
{
    [SerializeField] private TextMeshProUGUI _titleText;

    public override void Refresh()
    {
        _titleText.text = "Inventory";
    }
}

public class ItemSlot : UISlot<ItemData>
{
    [SerializeField] private TextMeshProUGUI _nameText;

    public override void Refresh()
    {
        _nameText.text = Data.Name;
    }

    protected override void OnLeftClick()
    {
        Debug.Log($"Clicked: {Data.Name}");
    }
}
```

```csharp
private IUIService _uiService;

private void Start()
{
    _uiService = ServiceLocator.Get<IUIService>();
}

public void OnClickInventoryButton()
{
    _uiService.OpenPopup<InventoryPopup>();
}

public void OnClickCloseButton()
{
    _uiService.ClosePopup<InventoryPopup>();
}
```

## 주의사항

- `Load By Addressable Service`를 켜면 `Popup Label Reference` 라벨의 팝업이 자동 등록됩니다(`BootstrapConfig`에 `AddressableManager` 필요, `Popup Parent` 지정). 끄면 `RegisterPopup`으로 직접 등록합니다(`ExamplePopupLoader.cs` 참고).
- **모달**: 화면 전체를 덮는 `Img_ModalPanel`에 `UIModalPanel`과 Raycast Target이 켜진 Image를 붙여 `Modal Panel`에 할당합니다.
- **First Selected**: 키보드·게임패드로 열었을 때 처음 선택할 버튼입니다. 닫으면 이전 선택으로 돌아갑니다. 팝업 밖으로 커서가 나가지 않게 하려면 팝업 안 버튼들의 Navigation을 Explicit으로 설정합니다.
- `UISlot` 클릭을 받으려면 오브젝트(또는 자식)에 Raycast Target이 켜진 Graphic이 있어야 합니다.
