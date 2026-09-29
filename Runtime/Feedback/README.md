# Feedback

> `PointerFeedback`은 `Button` 없이도 포인터 Up/Down/Click/Enter/Exit에 Feedback(`MMF_Player`)을 붙이는 컴포넌트입니다.  
> EventSystem의 `IPointer*Handler`만 사용하므로 UI뿐만 아니라 Sprite, 3D 오브젝트에서도 동작합니다.

<br>

## 관련 타입

| Type | Role |
| --- | --- |
| `PointerFeedback` | 포인터 Up/Down/Click(좌/우클릭 구분), Enter/Exit(hover), 비활성 상태 클릭에 Feedback을 붙이는 컴포넌트 |

<br>

### `PointerFeedback`

```csharp
public class PointerFeedback : MonoBehaviour,
    IPointerUpHandler, IPointerDownHandler, IPointerClickHandler,
    IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private bool _useLeftPointerFeedback;
    [SerializeField] private MMF_Player _leftPointerUpFeedback;
    [SerializeField] private MMF_Player _leftPointerDownFeedback;
    [SerializeField] private MMF_Player _leftPointerClickFeedback;

    [SerializeField] private bool _useRightPointerFeedback;
    [SerializeField] private MMF_Player _rightPointerUpFeedback;
    [SerializeField] private MMF_Player _rightPointerDownFeedback;
    [SerializeField] private MMF_Player _rightPointerClickFeedback;

    [SerializeField] private MMF_Player _pointerEnterFeedback;
    [SerializeField] private MMF_Player _pointerExitFeedback;

    [SerializeField] private Selectable _selectable;            // 비활성 여부를 판단할 Button 등
    [SerializeField] private MMF_Player _disabledClickFeedback;  // 비활성 상태에서 클릭했을 때의 거부 Feedback
    // OnDisable()에서 StopAllFeedbacks()
}
```

- Up/Down/Click은 `PointerEventData.button`으로 좌/우클릭을 구분해서 해당하는 `MMF_Player`만 재생합니다.
- `_useLeftPointerFeedback`/`_useRightPointerFeedback`으로 좌/우클릭 Feedback 사용 여부를 나눠서 켤 수 있습니다. 꺼져 있으면 해당 쪽 `StopFeedbacks()`도 호출하지 않습니다.
- Enter/Exit(hover)는 좌/우 구분 없이 공통 Feedback 하나로 처리합니다. hover 이벤트는 눌린 버튼이 없어서 `eventData.button`이 항상 `Left`로 들어오기 때문입니다.

**비활성 상태 처리**

`Button.interactable`을 꺼도, 같은 오브젝트에 붙은 `PointerFeedback`은 포인터 이벤트를 그대로 받습니다. 그래서 `Selectable`(Button 등)이 연결돼 있고 비활성이면 아래처럼 동작합니다.

| 이벤트 | 활성 상태 | 비활성 상태 |
|---|---|---|
| Up / Down / Enter / Exit | 해당 Feedback 재생 | 재생하지 않음 |
| Click | 클릭 Feedback 재생 | `Disabled Click Feedback`(거부 연출) 재생 |

- 비활성 처리를 쓰려면 인스펙터에서 `Selectable` 필드에 같은 오브젝트의 Button 등을 직접 할당합니다.
- `Selectable`이 없는 오브젝트(Sprite 등)는 항상 활성 상태로 동작합니다.
- 예) 재화가 부족해 비활성화된 구매 버튼을 누르면, 짧게 흔들리는 거부 Feedback으로 "지금은 누를 수 없다"는 걸 알려줄 수 있습니다.

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

Feedback을 주고 싶은 오브젝트에 `PointerFeedback`을 붙이고, 좌/우클릭 중 필요한 쪽의 `Use Left Pointer Feedback`/`Use Right Pointer Feedback`을 켠 뒤 원하는 `MMF_Player` 필드(Up/Down/Click)를 할당합니다. hover Feedback이 필요하면 `Hover Feedbacks Settings`의 Enter/Exit 필드를, 비활성 버튼의 거부 연출이 필요하면 `Disabled Feedbacks Settings`의 `Disabled Click Feedback`을 할당합니다.

<br>

## Feel에 이미 있는 기능

아래 기능은 Feel에 있으므로 따로 만들지 않았습니다.

| 필요 | Feel 기능 |
|---|---|
| 충돌·트리거 시 Feedback | `MMTriggerAndCollision` — 2D/3D 충돌·트리거 Enter/Exit/Stay를 UnityEvent로 제공. 여기에 `MMF_Player.PlayFeedbacks`를 연결 |
| 활성화·시작 시 Feedback | `MMF_Player` 인스펙터의 `Auto Play On Start` / `Auto Play On Enable` 옵션 |
