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
| `IUIDragDataHandler` | 드래그 앤 드롭이 끝났을 때 두 UI 요소 간 데이터를 어떻게 주고받을지 정의하는 인터페이스 |
| `UIDragHandler` | 실제 드래그 입력(Begin/Drag/End/Drop)을 처리하고, 드롭 시 `IUIDragDataHandler`로 데이터 처리를 위임하는 컴포넌트 |
| `UIPointerFeedback` | `Button` 없이도 포인터 Up/Down/Click/Enter/Exit(좌/우클릭 구분)에 Feedback을 붙이는 컴포넌트 |

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
| `Init()` | `UIManager`가 팝업을 최초로 `Instantiate`한 직후 1번만 호출합니다. |
| `Open()` | `Refresh()`로 최신 데이터를 반영한 뒤 `OnOpen()`을 실행하고, 열기 Feedback이 있으면 재생합니다. |
| `Close()` | 닫기 Feedback이 있으면 그걸 재생하고(끝나면 자동으로 비활성화), 없으면 바로 `OnClose()`로 비활성화합니다. |
| `OnOpen()` / `OnClose()` | 실제 활성화/비활성화 동작. 커스텀 애니메이션이 필요하면 override. |

**모달 배경 클릭 닫기**

- 팝업 프리팹에서 `Use Modal`을 켜면 `Modal Panel` 필드가 표시됩니다.
- 화면 전체를 덮는 `Img_ModalPanel` 오브젝트에 `UIModalPanel`과 Raycast Target이 켜진 Image를 추가한 뒤, 해당 컴포넌트를 `Modal Panel`에 할당합니다.
- `UIModalPanel`은 부모 `UIPopup`을 직접 참조하며, 패널을 좌클릭하면 `Close()`를 호출합니다. 실제 콘텐츠는 패널보다 앞에 배치하므로 클릭해도 닫히지 않습니다.

**Feedback 관련 주의사항**

- `_useOpenFeedback`/`_useCloseFeedback`이 켜져 있고 `MMF_Player`가 실제로 할당돼 있을 때만 재생됩니다.
- 닫기 Feedback을 쓰면 실제 `SetActive(false)`는 `_closeFeedback.Events.OnComplete`가 끝난 뒤 실행됩니다(`Init()`에서 리스너 등록).
- 닫는 애니메이션 도중 같은 팝업을 다시 `Open()`하면 나중에 끝나는 `OnComplete`가 방금 다시 연 팝업을 도로 꺼버릴 수 있어서, `Open()` 맨 앞에서 `_closeFeedback.StopFeedbacks()`로 이전 닫기 애니메이션을 끊고 시작합니다.

<br>

### `UISlot<TData>`

```csharp
public abstract class UISlot<TData> : UIBase, IUIDragDataHandler, IPointerClickHandler
{
    protected TData Data { get; private set; }

    public void SetData(TData data) { Data = data; Refresh(); }

    public virtual void HandleDragDataWith(IUIDragDataHandler other) { }

    public void OnPointerClick(PointerEventData eventData) { ... } // 좌/우클릭 구분해서 OnLeftClick()/OnRightClick() 호출

    protected virtual void OnLeftClick() { }
    protected virtual void OnRightClick() { }
}
```

| Member | Description |
|---|---|
| `Data` | 마지막으로 바인딩된 데이터. `Refresh()`/`OnLeftClick()`/`OnRightClick()`/`HandleDragDataWith()` 구현에서 읽어서 씁니다. |
| `SetData(TData data)` | 새 데이터를 반영하고 `Refresh()`를 호출합니다. |
| `HandleDragDataWith(IUIDragDataHandler other)` | 드래그 앤 드롭으로 다른 UI 요소가 이 슬롯에 드롭됐을 때 실행할 로직(`IUIDragDataHandler` 구현). 기본은 빈 구현이며, 드래그를 지원할 슬롯만 `override`합니다. |
| `OnLeftClick()` / `OnRightClick()` | 좌클릭/우클릭됐을 때 실행할 로직. 둘 다 `virtual`이라 필요한 쪽만 `override`합니다. |

- 클릭 감지는 `Button` 없이 `IPointerClickHandler`를 직접 구현합니다. 클릭을 받으려면 이 오브젝트(또는 자식)에 **Raycast Target이 켜진 Graphic**(Image 등)이 있어야 합니다.
- 드래그 앤 드롭 입력 자체는 `UISlot<TData>`가 아니라 `UIDragHandler`가 처리합니다. `UISlot<TData>`는 `IUIDragDataHandler`만 구현해서, 드롭됐을 때의 **데이터 처리 로직**만 제공합니다.

<br>

### `IUIDragDataHandler` / `UIDragHandler`

```csharp
public interface IUIDragDataHandler
{
    void HandleDragDataWith(IUIDragDataHandler other);
}
```

- 드래그 앤 드롭이 끝났을 때, 드래그를 시작한 쪽과 드롭된 쪽이 서로 데이터를 어떻게 반영할지 정의하는 인터페이스입니다.
- `UISlot<TData>`가 기본 구현(빈 메서드)을 제공하므로, 드래그 데이터를 실제로 처리해야 하는 슬롯에서만 `override`하면 됩니다.

```csharp
public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public event Action<UIDragHandler, UIDragHandler> OnSlotDropped;
}
```

| Member | Description |
|---|---|
| `OnBeginDrag` | 드래그 시작 시 원래 부모/형제 인덱스/위치를 저장하고, 최상위 `Canvas`로 옮겨서 다른 UI 위로 그려지게 합니다. 자신을 포함한 모든 `Graphic`의 `raycastTarget`을 꺼서 자기 자신이 드롭 판정을 가로채지 않게 합니다. |
| `OnDrag` | 포인터 위치를 그대로 따라가게 이동시킵니다. |
| `OnEndDrag` | 원래 부모/형제 인덱스/위치로 복원합니다. 드롭에 성공하든 실패하든 항상 호출됩니다. |
| `OnDrop` | 드롭된 오브젝트(자기 자신)가 드래그 중이던 오브젝트와 다를 때만 동작합니다. 양쪽의 `IUIDragDataHandler`를 `GetComponent`로 가져와서, **드래그를 시작한 쪽**의 `HandleDragDataWith(드롭 대상)`만 호출하고 `OnSlotDropped`를 발생시킵니다. |
| `OnSlotDropped` | 드롭이 성공했을 때 발생하는 이벤트. `(from, to)` 순서로 전달됩니다. 같은 드롭 신호를 여러 곳에서 구독해야 할 때 사용합니다. |

- 드래그 가능한 슬롯을 만들려면, `IUIDragDataHandler`를 구현한 컴포넌트(예: `UISlot<TData>` 상속 클래스)와 같은 GameObject에 `UIDragHandler`를 추가해야 합니다.
- 자신이 드래그 중일 때 `OnDisable()`이 호출되면 위치를 복원합니다 — 드래그 도중 오브젝트가 비활성화되는 경우(팝업이 닫히는 등)에 대비한 처리입니다.
- 실제 데이터 처리는 `IUIDragDataHandler`에게 위임하고, `UIDragHandler`는 순수하게 드래그 입력(이동/복원/드롭 판정)만 담당합니다.

<br>

### `UIPointerFeedback`

```csharp
public class UIPointerFeedback : MonoBehaviour,
    IPointerUpHandler, IPointerDownHandler, IPointerClickHandler,
    IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private bool _useLeftPointerFeedback;
    [SerializeField] private MMF_Player _leftPointerUpFeedback;
    [SerializeField] private MMF_Player _leftPointerDownFeedback;
    [SerializeField] private MMF_Player _leftPointerClickFeedback;
    [SerializeField] private MMF_Player _leftPointerExitFeedback;
    [SerializeField] private MMF_Player _leftPointerEnterFeedback;

    [SerializeField] private bool _useRightPointerFeedback;
    [SerializeField] private MMF_Player _rightPointerUpFeedback;
    [SerializeField] private MMF_Player _rightPointerDownFeedback;
    [SerializeField] private MMF_Player _rightPointerClickFeedback;
    [SerializeField] private MMF_Player _rightPointerExitFeedback;
    [SerializeField] private MMF_Player _rightPointerEnterFeedback;
    // OnDisable()에서 StopAllFeedbacks()
}
```

- `Button` 컴포넌트 없이도 동작하며, 좌클릭/우클릭을 구분해서 각각 Up/Down/Click/Exit/Enter Feedback을 지정할 수 있습니다.
- `_useLeftPointerFeedback`/`_useRightPointerFeedback`으로 좌/우클릭 Feedback 사용 여부를 나눠서 켤 수 있습니다. 꺼져 있으면 해당 쪽 `StopFeedbacks()`도 호출하지 않습니다.
- 재생은 `PointerEventData.button`으로 좌/우클릭을 구분해서 해당하는 `MMF_Player`만 실행합니다.

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

`UIManager` Inspector의 `Load By Addressable Service`를 켜면 Addressables 라벨에서 팝업을 자동 등록합니다. 이 경우 `BootstrapConfig`에 `AddressableManager`와 `UIManager`를 등록하고, `Popup Label Reference`와 `Popup Parent`를 지정합니다. `Bootstrapper`가 `UIManager.Init()`을 호출하면 해당 라벨의 모든 프리팹을 로드합니다.

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

**5. 클릭/포인터 Feedback 추가**

Feedback을 주고 싶은 오브젝트에 `UIPointerFeedback`을 붙이고, 좌/우클릭 중 필요한 쪽의 `_useLeftPointerFeedback`/`_useRightPointerFeedback`을 켠 뒤 원하는 `MMF_Player` 필드(Up/Down/Click/Exit/Enter)를 할당하면 끝입니다.

**6. 드래그 앤 드롭 슬롯 정의**

같은 값을 가진 슬롯끼리 드래그해서 합치는 예시입니다. 슬롯 프리팹에 `UIDragHandler` 컴포넌트를 함께 추가하고, `UISlot<TData>` 상속 클래스에서 `HandleDragDataWith()`를 `override`해서 드롭됐을 때의 로직을 정의합니다.

```csharp
using PhikozzLib;
using TMPro;
using UnityEngine;

public class MergeSlot : UISlot<int>
{
    [SerializeField] private TextMeshProUGUI _valueText;

    public override void Refresh()
    {
        _valueText.text = Data.ToString();
    }

    public override void HandleDragDataWith(IUIDragDataHandler other)
    {
        if (other is MergeSlot otherSlot && otherSlot.Data == Data)
        {
            SetData(Data + otherSlot.Data);
            otherSlot.SetData(0);
            return;
        }

        base.HandleDragDataWith(other);
    }
}
```

- 드래그를 시작한 슬롯의 `HandleDragDataWith()`만 호출되므로, "내 데이터를 드롭 대상과 어떻게 합칠지"는 항상 드래그를 시작한 쪽 기준으로 작성합니다.
- 조건에 맞지 않는 드롭이면 `base.HandleDragDataWith(other)`를 호출해 기본(빈) 동작으로 넘깁니다.
