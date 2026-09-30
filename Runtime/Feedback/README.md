# Feedback

> 입력 이벤트에 Feedback(`MMF_Player`)을 붙이는 컴포넌트 모음입니다.  
> `PointerFeedback`은 마우스·터치, `NavigationFeedback`은 키보드·게임패드 UI 조작을 담당합니다.

<br>

## 관련 타입

| Type | Role |
| --- | --- |
| `PointerFeedback` | 포인터 Up/Down/Click(좌/우클릭 구분), Enter/Exit(hover), 비활성 상태 클릭에 Feedback을 붙이는 컴포넌트. UI뿐만 아니라 Sprite, 3D 오브젝트에서도 동작 |
| `NavigationFeedback` | 키보드·게임패드로 선택(Select)·선택 해제(Deselect)·확인(Submit)했을 때 Feedback을 붙이는 컴포넌트 |

두 컴포넌트 모두 드래그·스크롤 이벤트 인터페이스를 구현하지 않습니다. 그래서 `ScrollRect` 안의 버튼에 붙여도 부모의 드래그·휠 스크롤을 가로채지 않습니다. (`EventTrigger`는 모든 이벤트 인터페이스를 구현해서 스크롤을 가로채므로 대신 사용하지 않습니다.)

<br>

### `PointerFeedback`

```csharp
public class PointerFeedback : MonoBehaviour,
    IPointerUpHandler, IPointerDownHandler, IPointerClickHandler,
    IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private MMF_Player _leftPointerUpFeedback;
    [SerializeField] private MMF_Player _leftPointerDownFeedback;
    [SerializeField] private MMF_Player _leftPointerClickFeedback;

    [SerializeField] private MMF_Player _rightPointerUpFeedback;
    [SerializeField] private MMF_Player _rightPointerDownFeedback;
    [SerializeField] private MMF_Player _rightPointerClickFeedback;

    [SerializeField] private MMF_Player _pointerEnterFeedback;
    [SerializeField] private MMF_Player _pointerExitFeedback;

    [SerializeField] private Selectable _selectable;            // 비활성 여부를 판단할 Button 등 (비워두면 Awake에서 같은 오브젝트에서 찾음)
    [SerializeField] private MMF_Player _disabledClickFeedback;  // 비활성 상태에서 클릭했을 때의 거부 Feedback

    public void SetInteractable(bool isInteractable) { ... }     // Selectable이 없는 오브젝트의 비활성 상태 설정
    // OnDisable()에서 StopAllFeedbacks()
}
```

- Up/Down/Click은 `PointerEventData.button`으로 좌/우클릭을 구분해서 해당하는 `MMF_Player`만 재생합니다.
- 필요한 필드에만 `MMF_Player`를 할당합니다. 비워둔 필드는 재생되지 않습니다.
- Enter/Exit(hover)는 좌/우 구분 없이 공통 Feedback 하나로 처리합니다. hover 이벤트는 눌린 버튼이 없어서 `eventData.button`이 항상 `Left`로 들어오기 때문입니다.

**비활성 상태 처리**

`Button.interactable`을 꺼도, 같은 오브젝트에 붙은 `PointerFeedback`은 포인터 이벤트를 그대로 받습니다. 그래서 `Selectable`(Button 등)이 연결돼 있고 비활성이면 아래처럼 동작합니다.

| 이벤트 | 활성 상태 | 비활성 상태 |
|---|---|---|
| Up / Down / Enter / Exit | 해당 Feedback 재생 | 재생하지 않음 |
| Click | 클릭 Feedback 재생 | `Disabled Click Feedback`(거부 연출) 재생 |

- `Selectable` 필드를 비워두면 `Awake()`에서 같은 오브젝트의 `Selectable`(Button 등)을 찾아 연결합니다. 다른 오브젝트의 버튼 상태를 따라가야 할 때만 직접 할당합니다.
- `Selectable`이 없는 오브젝트(Sprite, 3D 등)는 `SetInteractable(bool)`로 직접 비활성 상태를 정합니다. `Selectable`이 연결돼 있으면 둘 중 하나라도 비활성이면 비활성으로 처리합니다.
- Feedback을 거부 연출까지 전부 끄려면 컴포넌트 자체를 끕니다(`enabled = false`). EventSystem은 꺼진 컴포넌트에 포인터 이벤트를 보내지 않습니다.

```csharp
// 잠긴 보물상자(Sprite) → 누르면 거부 연출
_chestFeedback.SetInteractable(false);

// 잠금 해제 → 일반 클릭 Feedback
_chestFeedback.SetInteractable(true);
```
- 예) 재화가 부족해 비활성화된 구매 버튼을 누르면, 짧게 흔들리는 거부 Feedback으로 "지금은 누를 수 없다"는 걸 알려줄 수 있습니다.

<br>

### `NavigationFeedback`

```csharp
public class NavigationFeedback : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] private MMF_Player _selectFeedback;          // 방향키·스틱으로 커서가 왔을 때
    [SerializeField] private MMF_Player _deselectFeedback;        // 커서가 떠났을 때
    [SerializeField] private MMF_Player _submitFeedback;          // Enter / Space / 게임패드 A

    [SerializeField] private Selectable _selectable;              // 비활성 여부를 판단할 Button 등 (비워두면 Awake에서 같은 오브젝트에서 찾음)
    [SerializeField] private MMF_Player _disabledSubmitFeedback;  // 비활성 상태에서 Submit했을 때의 거부 Feedback
    // OnDisable()에서 StopAllFeedbacks()
}
```

| 이벤트 | 활성 상태 | 비활성 상태 |
|---|---|---|
| Select / Deselect | 해당 Feedback 재생 | 재생하지 않음 |
| Submit | `Submit Feedback` 재생 | `Disabled Submit Feedback`(거부 연출) 재생 |

- **마우스 클릭으로 선택된 경우는 무시합니다.** `Button`을 마우스로 클릭하면 Unity가 그 버튼을 선택 상태로도 만드는데, 이때 Select Feedback까지 재생되면 hover Feedback과 겹치기 때문입니다. 마우스로 선택될 때는 이벤트 정보가 `PointerEventData`로 들어오므로 이것으로 구분합니다. 마우스 쪽 연출은 `PointerFeedback`이 담당합니다.
- 입력 방식과 상관없이 같은 연출을 원하면, `PointerFeedback`의 hover·클릭 Feedback과 같은 `MMF_Player`를 할당합니다.
- 키보드·게임패드로 UI를 조작하지 않는 게임(모바일 전용 등)이나, 선택될 일이 없는 오브젝트(Sprite 등)에는 붙이지 않아도 됩니다.
- **방향키로 어느 버튼으로 이동할지는 `NavigationFeedback`이 아니라 각 `Button`의 Navigation 설정이 정합니다.** `NavigationFeedback`은 선택·해제·확인이 일어났을 때 연출만 담당합니다.
- 처음에 선택된 오브젝트가 없으면 방향키가 동작하지 않습니다. EventSystem의 `First Selected`나 `UIPopup`의 `First Selected`로 처음 선택할 버튼을 지정합니다.

<br>

## 필요한 설정

`PointerFeedback`이 포인터 이벤트를 받으려면 씬에 `EventSystem`이 있어야 하고, 대상 종류에 따라 아래 설정이 추가로 필요합니다.

| 대상 | 필요한 설정 |
|---|---|
| UI | 이 오브젝트(또는 자식)에 **Raycast Target이 켜진 Graphic**(Image 등) |
| Sprite (2D) | 이 오브젝트에 `Collider2D` + 카메라에 `Physics2DRaycaster` |
| 3D 오브젝트 | 이 오브젝트에 `Collider` + 카메라에 `PhysicsRaycaster` |

<br>

## 사용 예시

Feedback을 주고 싶은 오브젝트에 `PointerFeedback`을 붙이고, `Left Click Feedbacks Settings` / `Right Click Feedbacks Settings`에서 필요한 `MMF_Player` 필드(Up/Down/Click)를 할당합니다. hover Feedback이 필요하면 `Hover Feedbacks Settings`의 Enter/Exit 필드를, 비활성 버튼의 거부 연출이 필요하면 `Disabled Feedbacks Settings`의 `Disabled Click Feedback`을 할당합니다.

키보드·게임패드 조작도 지원한다면 같은 오브젝트에 `NavigationFeedback`을 추가로 붙이고, Select/Deselect/Submit Feedback을 할당합니다.

```
Btn_Buy
├─ Button
├─ PointerFeedback      ← 마우스·터치
└─ NavigationFeedback   ← 키보드·게임패드
```

<br>

## Feel에 이미 있는 기능

아래 기능은 Feel에 있으므로 따로 만들지 않았습니다.

| 필요 | Feel 기능 |
|---|---|
| 충돌·트리거 시 Feedback | `MMTriggerAndCollision` — 2D/3D 충돌·트리거 Enter/Exit/Stay를 UnityEvent로 제공. 여기에 `MMF_Player.PlayFeedbacks`를 연결 |
| 활성화·시작 시 Feedback | `MMF_Player` 인스펙터의 `Auto Play On Start` / `Auto Play On Enable` 옵션 |
