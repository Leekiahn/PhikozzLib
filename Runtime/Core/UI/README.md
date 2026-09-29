# UI

> `UIManager`는 Addressables 프리로드 여부를 선택하고, 타입 기준으로 팝업 열기/닫기/재사용을 관리하는 UI 서비스입니다.  
> `IUIService` 인터페이스를 상속받아 구현합니다.

<br>

## 관련 타입

| Type | Role |
| --- | --- |
| `UIBase` | 모든 UI 요소의 공통 베이스. `Refresh()`만 약속한다 |
| `UIPopup` | 열기/닫기가 있는 UI(창, 팝업, HUD 등)의 베이스 |
| `UISlot<TData>` | 리스트/그리드 아이템처럼 데이터 바인딩 + 클릭만 있는 UI의 베이스 |
| `UIModalPanel` | 팝업 뒤를 덮는 모달 배경. 클릭하면 소속 팝업을 닫는다 |

> 포인터 Up/Down/Click/Enter/Exit에 Feedback을 붙이는 `PointerFeedback`은 UI 전용이 아니라서 [Feedback](../Feedback/README.md) 모듈로 옮겼습니다.

<br>

### `UIBase`

```csharp
public abstract class UIBase : MonoBehaviour
{
    public abstract void Refresh();
}
```

- `Refresh()`만 강제합니다. 언제 호출할지는 하위 클래스가 결정합니다(`UIPopup`은 `Open()` 시, `UISlot<T>`는 `SetData()` 시).

<br>

### `UIPopup`

```csharp
public abstract class UIPopup : UIBase
{
    public bool IsVisible { get; protected set; }

    [SerializeField] private bool _useModal;
    [ShowIf("_useModal")]
    [SerializeField] private UIModalPanel _modalPanel;

    [SerializeField] private bool _useFeedback;
    [ShowIf("_useFeedback")]
    [SerializeField] private MMF_Player _openFeedback;
    [ShowIf("_useFeedback")]
    [SerializeField] private MMF_Player _closeFeedback;

    public virtual void Init() { ... }   // UIManager가 최초 생성 시 1회 호출
    public void Open() { ... }           // Refresh() → OnOpen() → (있으면) 열기 Feedback 재생
    public void Close() { ... }          // (있으면) 닫기 Feedback 재생, 없으면 바로 OnClose()

    protected virtual void OnOpen() { gameObject.SetActive(true); }
    protected virtual void OnClose() { gameObject.SetActive(false); }
}
```

| Member | Description |
|---|---|
| `IsVisible` | 현재 열려 있는지 여부. `UIManager`가 팝업 재사용 여부를 판단할 때 씁니다. |
| `Init()` | `UIManager`가 팝업을 최초로 `Instantiate`한 직후 1번만 호출합니다. 모달 패널에 자신을 연결하고, 닫기 Feedback 완료 리스너를 등록합니다. |
| `Open()` | `Refresh()`로 최신 데이터를 반영한 뒤 `OnOpen()`을 실행하고, 열기 Feedback이 있으면 재생합니다. |
| `Close()` | 닫기 Feedback이 있으면 그걸 재생하고(끝나면 자동으로 비활성화), 없으면 바로 `OnClose()`로 비활성화합니다. |
| `OnOpen()` / `OnClose()` | 실제 활성화/비활성화 동작. 커스텀 애니메이션이 필요하면 override. |

**모달 배경 클릭 닫기**

- 팝업 프리팹에서 `Use Modal`을 켜면 `Modal Panel` 필드가 표시됩니다.
- 화면 전체를 덮는 `Img_ModalPanel` 오브젝트에 `UIModalPanel`과 Raycast Target이 켜진 Image를 추가한 뒤, 해당 컴포넌트를 `Modal Panel`에 할당합니다.
- `UIPopup.Init()`에서 `UIModalPanel.SetPopup(this)`로 소속 팝업이 연결됩니다. 패널을 클릭하면 `IUIService.ClosePopup(popup)`을 호출해 닫습니다. 실제 콘텐츠는 패널보다 앞에 배치하므로 클릭해도 닫히지 않습니다.

**Feedback 관련 주의사항**

- `Use Feedback`이 켜져 있고 해당 `MMF_Player`가 실제로 할당돼 있을 때만 재생됩니다.
- 닫기 Feedback을 쓰면 실제 `SetActive(false)`는 `_closeFeedback.Events.OnComplete`가 끝난 뒤 실행됩니다(`Init()`에서 리스너 등록).
- 닫는 애니메이션 도중 같은 팝업을 다시 `Open()`하면 나중에 끝나는 `OnComplete`가 방금 다시 연 팝업을 도로 꺼버릴 수 있어서, `Open()` 맨 앞에서 `_closeFeedback.StopFeedbacks()`로 이전 닫기 애니메이션을 끊고 시작합니다.

<br>

### `UISlot<TData>`

```csharp
public abstract class UISlot<TData> : UIBase, IPointerClickHandler
{
    protected TData Data { get; private set; }

    public void SetData(TData data) { Data = data; Refresh(); }

    public void OnPointerClick(PointerEventData eventData) { ... } // 좌/우클릭 구분해서 OnLeftClick()/OnRightClick() 호출

    protected virtual void OnLeftClick() { }
    protected virtual void OnRightClick() { }
}
```

| Member | Description |
|---|---|
| `Data` | 마지막으로 바인딩된 데이터. `Refresh()`/`OnLeftClick()`/`OnRightClick()` 구현에서 읽어서 씁니다. |
| `SetData(TData data)` | 새 데이터를 반영하고 `Refresh()`를 호출합니다. |
| `OnLeftClick()` / `OnRightClick()` | 좌클릭/우클릭됐을 때 실행할 로직. 둘 다 `virtual`이라 필요한 쪽만 `override`합니다. |

- 클릭 감지는 `Button` 없이 `IPointerClickHandler`를 직접 구현합니다. 클릭을 받으려면 이 오브젝트(또는 자식)에 **Raycast Target이 켜진 Graphic**(Image 등)이 있어야 합니다.

<br>

## `IUIService` / `UIManager` Public API

| Method | Description |
|---|---|
| `RegisterPopup(UIPopup prefab)` | 프리팹의 구체 타입을 key로 사용해 팝업을 등록합니다. `Load By Addressable Service`를 끈 경우 외부 데이터 로더에서 사용합니다. |
| `UnregisterPopup(UIPopup prefab)` | 프리팹 타입에 해당하는 팝업 등록을 해제합니다. 열린 인스턴스가 있으면 함께 제거합니다. |
| `OpenPopup<T>()` | 타입 `T`의 팝업을 엽니다. 이미 생성된 인스턴스가 있으면 재사용, 없으면 새로 `Instantiate` + `Init()` + `Open()`. 등록되지 않은 타입이면 `null`을 반환하고 경고 로그를 남깁니다. 인스턴스 자체는 `T`로 캐스팅되어 반환되므로, 호출부에서 변수에 담아두면 `UIManager`를 다시 거치지 않고 그 팝업 고유의 메서드를 직접 호출할 수 있습니다. |
| `ClosePopup<T>()` | 타입 `T`의 열린 팝업을 닫습니다. |
| `ClosePopup(UIPopup popup)` | 인스턴스를 직접 넘겨서 닫습니다. |
| `CloseAllPopup()` | 현재 열려 있는 모든 팝업을 닫습니다. |

<br>

## 사용 예시

**1. 로드 방식 설정**

`UIManager` Inspector의 `Load By Addressable Service`를 켜면 Addressables 라벨에서 팝업을 자동 등록합니다. 이 경우 `BootstrapConfig`에 `AddressableManager`와 `UIManager`를 등록하고, `Popup Label Reference`와 `Popup Parent`를 지정합니다. `Bootstrapper`가 `UIManager.InitAsync()`를 호출하면 해당 라벨의 모든 프리팹을 로드합니다.

`Load By Addressable Service`를 끄면 `UIManager`는 팝업을 자동으로 프리로드하지 않습니다. 이 경우 프로젝트의 데이터 로더 등 외부 코드에서 `IUIService.RegisterPopup(UIPopup prefab)`을 호출해 팝업을 등록합니다. 등록을 해제할 때는 `UnregisterPopup(UIPopup prefab)`을 사용합니다.

`ExamplePopupLoader.cs`는 BGDatabase 등 프로젝트별 데이터 타입을 직접 참조하지 않도록 전체를 주석 처리한 예시입니다. 패키지 사용자는 이 내용을 프로젝트 쪽 스크립트로 복사하고, `BG_Popup`과 `prefab` 필드를 사용하는 데이터 구조에 맞게 바꾼 뒤 사용합니다.

**2. 팝업 정의**

```csharp
using PhikozzLib;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : UIPopup
{
    [SerializeField] private Text _titleText;

    public override void Refresh()
    {
        _titleText.text = "Inventory";
    }
}
```

**3. 팝업 열고 닫기**

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

- `UIManager`가 `_openedPopups`에 타입별 인스턴스를 캐싱하므로, `OpenPopup<T>()`를 여러 번 호출해도 `Instantiate`는 최초 1회만 일어나고 이후에는 같은 인스턴스를 재사용합니다.
- `OpenPopup<T>()`는 `T` 타입 그대로 반환하므로, 열자마자 그 팝업 고유의 메서드를 호출해야 한다면 아래처럼 반환값을 직접 받아서 캐싱해두는 것도 가능합니다.

```csharp
private InventoryPopup _inventoryPopup;

public void OnClickInventoryButton()
{
    _inventoryPopup = _uiService.OpenPopup<InventoryPopup>();
    _inventoryPopup.Refresh();
}
```

**4. 슬롯(리스트 아이템) 정의**

```csharp
using PhikozzLib;
using UnityEngine.UI;

public class ItemSlot : UISlot<ItemData>
{
    [SerializeField] private Text _nameText;

    public override void Refresh()
    {
        _nameText.text = Data.Name;
    }

    protected override void OnLeftClick()
    {
        Debug.Log($"Clicked: {Data.Name}");
    }
}

// 리스트를 채울 때
foreach (var slotInstance in slots)
{
    slotInstance.SetData(itemDataList[i]);
}
```
