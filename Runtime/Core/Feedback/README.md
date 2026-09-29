# Feedback

> `PointerFeedback`은 `Button` 없이도 포인터 Up/Down/Click/Enter/Exit에 Feedback(`MMF_Player`)을 붙이는 컴포넌트입니다.  
> EventSystem의 `IPointer*Handler`만 사용하므로 UI뿐만 아니라 Sprite, 3D 오브젝트에서도 동작합니다.

<br>

## 관련 타입

| Type | Role |
| --- | --- |
| `PointerFeedback` | 포인터 Up/Down/Click(좌/우클릭 구분), Enter/Exit(hover)에 Feedback을 붙이는 컴포넌트 |

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
    // OnDisable()에서 StopAllFeedbacks()
}
```

- Up/Down/Click은 `PointerEventData.button`으로 좌/우클릭을 구분해서 해당하는 `MMF_Player`만 재생합니다.
- `_useLeftPointerFeedback`/`_useRightPointerFeedback`으로 좌/우클릭 Feedback 사용 여부를 나눠서 켤 수 있습니다. 꺼져 있으면 해당 쪽 `StopFeedbacks()`도 호출하지 않습니다.
- Enter/Exit(hover)는 좌/우 구분 없이 공통 Feedback 하나로 처리합니다. hover 이벤트는 눌린 버튼이 없어서 `eventData.button`이 항상 `Left`로 들어오기 때문입니다.

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

Feedback을 주고 싶은 오브젝트에 `PointerFeedback`을 붙이고, 좌/우클릭 중 필요한 쪽의 `Use Left Pointer Feedback`/`Use Right Pointer Feedback`을 켠 뒤 원하는 `MMF_Player` 필드(Up/Down/Click)를 할당합니다. hover Feedback이 필요하면 `Hover Feedbacks Settings`의 Enter/Exit 필드를 할당하면 끝입니다.
