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
| `UIButtonFeedback` | 기존 `Button`에 붙여서 클릭 Feedback만 추가하는 컴포넌트 |

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

### `UIButtonFeedback`

```csharp
public class UIButtonFeedback : MonoBehaviour
{
    [SerializeField] private MMF_Player _clickFeedback;
    // Awake()에서 Button.onClick에 리스너 등록, 클릭 시 _clickFeedback.PlayFeedbacks()
    // OnDisable()에서 StopFeedbacks()
}
```

- 클릭 감지는 기존 `Button.onClick`에 위임하고, Feedback 재생만 얹습니다.
- 이 컴포넌트를 붙였다면 `_clickFeedback`은 반드시 할당돼 있어야 합니다 — 안 쓸 버튼에는 컴포넌트 자체를 붙이지 않습니다.

<br>

## `IUIService` / `UIManager` Public API

| Method | Description |
|---|---|
| `RegisterPopup<T>(T prefab)` | 타입 `T`의 팝업 프리팹을 직접 등록합니다(Addressables 라벨을 안 거치는 수동 경로). |
| `UnregisterPopup<T>(T prefab)` | 등록을 해제합니다. 열려 있는 인스턴스가 있으면 `Destroy`하고, 등록 목록에서도 제거합니다. |
| `OpenPopup<T>()` | 타입 `T`의 팝업을 엽니다. 이미 생성된 인스턴스가 있으면 재사용, 없으면 새로 `Instantiate` + `Init()` + `Open()`. 등록되지 않은 타입이면 `null`을 반환하고 경고 로그를 남깁니다. |
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

**5. 버튼에 클릭 Feedback 추가**

`Button` 컴포넌트가 있는 오브젝트에 `UIButtonFeedback`을 추가로 붙이고, `Click Feedback` 필드에 `MMF_Player`를 할당하면 끝입니다.
