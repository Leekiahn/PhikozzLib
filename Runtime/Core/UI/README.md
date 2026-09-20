# UI

> `UIManager`는 Addressables 기반으로 팝업 프리팹을 프리로드하고, 타입 기준으로 열기/닫기/재사용을 관리하는 UI 서비스입니다.  
> `IUIService` 인터페이스를 상속받아 구현합니다.

<br>

## 관련 타입

| Type | Role |
| --- | --- |
| `UIBase` | 모든 UI 요소의 공통 베이스. `Refresh()`만 약속한다 |
| `UIPopup` | 열기/닫기가 있는 UI(창, 팝업, HUD 등)의 베이스 |
| `UISlot<TData>` | 리스트/그리드 아이템처럼 데이터 바인딩 + 클릭만 있는 UI의 베이스 |
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

**Feedback 관련 주의사항**

- `_useOpenFeedback`/`_useCloseFeedback`이 켜져 있고 `MMF_Player`가 실제로 할당돼 있을 때만 재생됩니다.
- 닫기 Feedback을 쓰면 실제 `SetActive(false)`는 `_closeFeedback.Events.OnComplete`가 끝난 뒤 실행됩니다(`Init()`에서 리스너 등록).
- 닫는 애니메이션 도중 같은 팝업을 다시 `Open()`하면 나중에 끝나는 `OnComplete`가 방금 다시 연 팝업을 도로 꺼버릴 수 있어서, `Open()` 맨 앞에서 `_closeFeedback.StopFeedbacks()`로 이전 닫기 애니메이션을 끊고 시작합니다.

<br>

### `UISlot<TData>`

```csharp
public abstract class UISlot<TData> : UIBase, IPointerClickHandler
{
    protected TData Data { get; private set; }

    public void SetData(TData data) { Data = data; Refresh(); }

    public void OnPointerClick(PointerEventData eventData) { ... } // 클릭 Feedback 재생 후 OnClick() 호출
    protected abstract void OnClick();
}
```

| Member | Description |
|---|---|
| `Data` | 마지막으로 바인딩된 데이터. `Refresh()`/`OnClick()` 구현에서 읽어서 씁니다. |
| `SetData(TData data)` | 새 데이터를 반영하고 `Refresh()`를 호출합니다. |
| `OnClick()` | 클릭됐을 때 실행할 로직(`abstract`). |

- 클릭 감지는 `Button` 없이 `IPointerClickHandler`를 직접 구현합니다. 클릭을 받으려면 이 오브젝트(또는 자식)에 **Raycast Target이 켜진 Graphic**(Image 등)이 있어야 합니다.
- 클릭 Feedback 정지는 `SetData()`가 아니라 **`OnDisable()`**에서 처리합니다 — 리스트 재사용(pooling)으로 슬롯이 비활성화됐다 재활성화되는 흐름에 맞춘 것입니다.

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
| `RegisterPopup<T>(T prefab)` | 타입 `T`의 팝업 프리팹을 직접 등록합니다(Addressables 라벨을 안 거치는 수동 경로). |
| `UnregisterPopup<T>(T prefab)` | 등록을 해제합니다. 열려 있는 인스턴스가 있으면 `Destroy`하고, 등록 목록에서도 제거합니다. |
| `OpenPopup<T>()` | 타입 `T`의 팝업을 엽니다. 이미 생성된 인스턴스가 있으면 재사용, 없으면 새로 `Instantiate` + `Init()` + `Open()`. 등록되지 않은 타입이면 `null`을 반환하고 경고 로그를 남깁니다. 인스턴스 자체는 `T`로 캐스팅되어 반환되므로, 호출부에서 변수에 담아두면 `UIManager`를 다시 거치지 않고 그 팝업 고유의 메서드를 직접 호출할 수 있습니다. |
| `ClosePopup<T>()` | 타입 `T`의 열린 팝업을 닫습니다. |
| `ClosePopup(UIPopup popup)` | 인스턴스를 직접 넘겨서 닫습니다. |
| `CloseAllPopup()` | 현재 열려 있는 모든 팝업을 닫습니다. |

<br>

## 사용 예시

**1. Addressables 라벨로 자동 등록 (기본 경로)**

`UIManager` 프리팹에 `Popup Label Reference`와 `Popup Parent`를 지정해두면, `Bootstrapper`가 `UIManager.Init()`을 호출할 때 그 라벨에 속한 모든 프리팹을 자동으로 로드해 등록합니다.

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

    protected override void OnClick()
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
