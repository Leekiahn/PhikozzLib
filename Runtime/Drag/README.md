# BaseDraggable

> 드래그 앤 드롭의 공통 흐름(집기 → 따라가기 → 대상 찾기 → 실패 시 원위치 + Feedback)을 처리하는 추상 클래스입니다. UI와 Sprite/3D 모두 지원합니다.

## Public API

하위 클래스(`BaseDraggable<TTarget>`, `TTarget` = 놓을 대상 컴포넌트)에서 구현하는 멤버입니다.

| Member | Description |
|---|---|
| `TryDrop(TTarget target)` | **(필수)** 대상에 놓았을 때 할 일. 성공하면 `true`, 실패하면 `false`(원위치 + 실패 Feedback). |
| `CanBeginDrag()` | 집을 수 있는지 여부. `false`면 거부 Feedback을 재생합니다. 기본값 `true`. |
| `ReturnToOrigin()` | 실패 시 원위치 방법. 기본은 즉시 복귀입니다. |
| `OnEnterTarget(TTarget target)` / `OnExitTarget(TTarget target)` | 드래그 중 대상 위로 들어옴 / 벗어남. 슬롯 강조 등에 씁니다. |
| `OnStayTarget(TTarget target, PointerEventData eventData)` | 같은 대상 위에서 드래그가 움직일 때마다 호출됩니다. |

인스펙터: `Is UI`(UI / World 전환), `Drag Parent`(UI 드래그 중 옮겨둘 부모, 선택), 집기·성공·실패·거부 Feedback(선택).

## 사용 예시

```csharp
public class ItemDraggable : BaseDraggable<InventorySlot>
{
    [SerializeField] private ItemView _itemView;

    protected override bool CanBeginDrag()
    {
        return !_itemView.Item.IsLocked;
    }

    protected override bool TryDrop(InventorySlot slot)
    {
        if (!slot.CanAccept(_itemView.Item))
        {
            return false;
        }

        slot.PlaceItem(_itemView);
        return true;
    }

    protected override void OnEnterTarget(InventorySlot slot)
    {
        slot.SetHighlight(slot.CanAccept(_itemView.Item));
    }

    protected override void OnExitTarget(InventorySlot slot)
    {
        slot.ClearHighlight();
    }
}
```

## 주의사항

- **UI**는 Raycast Target이 켜진 Graphic, **Sprite/3D**(`Is UI` 끔)는 `Collider2D`/`Collider` + 카메라에 `Physics2DRaycaster`/`PhysicsRaycaster`가 필요합니다.
- `Awake()` / `OnDisable()`을 override하면 `base`를 꼭 호출합니다.
- `OnExitTarget`은 놓았을 때와 드래그 중 꺼졌을 때도 호출되므로 강조 해제는 여기서 합니다.
- `OnStayTarget`은 매 프레임이 아니라 포인터가 움직일 때만 호출됩니다.
- 스크롤 목록 안의 드래그 오브젝트 위에서는 스크롤이 되지 않습니다. 둘 다 필요하면 하위 클래스에서 방향에 따라 `ScrollRect`로 이벤트를 넘겨야 합니다.
