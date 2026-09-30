# BaseDraggable

> 드래그 앤 드롭의 공통 흐름(집기 → 따라가기 → 놓을 대상 찾기 → 실패 시 원위치 + Feedback)을 처리하는 추상 클래스입니다.  
> "무엇을 어디에 놓을 수 있고, 놓았을 때 무엇을 할지"만 하위 클래스에서 구현합니다. UI와 SpriteRenderer / 3D 오브젝트 모두 지원합니다.

---

## 주요 기능

- 포인터를 따라 이동 (잡은 지점 기준이라 중심으로 튀지 않음)
- 놓은 위치에서 대상(`TTarget`) 자동 탐색
- 실패 시 원래 위치·부모로 복귀
- 집기 / 성공 / 실패 / 거부 Feedback
- UI / World(SpriteRenderer, 3D) 전환 (`Is UI`)

---

## 구조

```csharp
// TTarget : 놓을 수 있는 대상 컴포넌트 타입 (예: InventorySlot, CardZone)
public abstract class BaseDraggable<TTarget> : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
    where TTarget : Component
{
    [SerializeField] private bool _isUI = true;

    [ShowIf("_isUI")] [SerializeField] private CanvasGroup _canvasGroup;   // 비워두면 Awake에서 찾거나 추가
    [ShowIf("_isUI")] [SerializeField] private Transform _dragParent;      // 드래그 중 옮겨둘 부모 (선택)
    [HideIf("_isUI")] [SerializeField] private Collider2D _collider2D;     // 비워두면 Awake에서 찾음
    [HideIf("_isUI")] [SerializeField] private Collider _collider;         // 비워두면 Awake에서 찾음

    [SerializeField] private MMF_Player _beginDragFeedback;    // 집었을 때
    [SerializeField] private MMF_Player _dropSuccessFeedback;  // 놓기 성공
    [SerializeField] private MMF_Player _dropFailFeedback;     // 놓기 실패 (원위치)
    [SerializeField] private MMF_Player _deniedDragFeedback;   // 집을 수 없는 상태에서 집으려 할 때

    protected abstract bool TryDrop(TTarget target);   // 놓았을 때 할 일. 성공하면 true
    protected virtual bool CanBeginDrag() => true;     // 잠금 등으로 못 집으면 false
    protected virtual void ReturnToOrigin() { ... }    // 실패 시 원위치 (기본: 즉시 복귀)
}
```

---

## 하위 클래스에서 구현하는 것

| Member | Description |
|---|---|
| `TryDrop(TTarget target)` | **(필수)** 대상 위에 놓았을 때 할 일. 성공하면 `true`, 놓을 수 없으면 `false`를 반환합니다. 성공 시 위치·부모는 여기서 정하고, 그대로 두면 놓은 위치에 남습니다. |
| `CanBeginDrag()` | 집을 수 있는지 여부. `false`면 거부 Feedback을 재생하고 드래그를 취소합니다. 기본값은 항상 `true`. |
| `ReturnToOrigin()` | 놓기 실패 시 원래 부모·순서·위치로 즉시 돌아갑니다. 애니메이션으로 돌아가게 하려면 override합니다. |

---

## 드래그 흐름

```
BeginDrag : CanBeginDrag()가 false → 거부 Feedback + 드래그 취소
            원래 부모·순서·위치 저장 → (UI) Drag Parent로 이동 → 레이캐스트 차단 해제 → 집기 Feedback
Drag      : 포인터를 따라 이동
EndDrag   : 레이캐스트 차단 복구 → 포인터 아래 오브젝트의 부모 방향으로 TTarget 탐색
            → 찾았고 TryDrop(target) == true  : 성공 Feedback
            → 그 외                          : ReturnToOrigin() + 실패 Feedback
```

---

## UI / World 설정

`Is UI`를 켜면 UI, 끄면 SpriteRenderer / 3D 오브젝트로 동작합니다. 인스펙터에는 해당 모드에 필요한 필드만 표시됩니다.

| | UI (`Is UI` 켬) | SpriteRenderer / 3D (`Is UI` 끔) |
|---|---|---|
| 필요한 설정 | Raycast Target이 켜진 Graphic | `Collider2D` 또는 `Collider` + 카메라에 `Physics2DRaycaster` 또는 `PhysicsRaycaster` |
| 포인터 따라가기 | 화면 좌표를 부모 RectTransform 평면의 좌표로 변환 | 카메라 기준으로 오브젝트의 현재 깊이에 맞춰 월드 좌표로 변환 |
| 드래그 중 레이캐스트 | `CanvasGroup.blocksRaycasts = false` | 자기 `Collider2D` / `Collider`를 끔 |
| 추가 옵션 | `Drag Parent` — 드래그 중 옮겨둘 부모 | - |

- **드래그 중 레이캐스트를 끄는 이유**: 끄지 않으면 놓을 때 포인터 아래가 항상 자기 자신으로 잡혀서 대상을 찾을 수 없습니다.
- **`Drag Parent`**: 최상위 Canvas 등을 지정하면 드래그하는 동안 그 아래로 옮겨서, 스크롤 영역의 Mask에 잘리거나 다른 UI 뒤에 가려지지 않게 합니다. 비워두면 부모를 바꾸지 않습니다.
- 대상 탐색은 포인터 아래 오브젝트에서 부모 방향으로 `TTarget`을 찾습니다(`GetComponentInParent`). 슬롯의 자식 이미지 위에 놓아도 슬롯이 찾아집니다.

---

## 사용 예시

**인벤토리 아이템 → 슬롯 (UI)**

```csharp
using PhikozzLib;

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
            return false; // 원위치 + 실패 Feedback
        }

        slot.PlaceItem(_itemView); // 슬롯 아래로 옮기는 등 성공 시 위치는 여기서 결정
        return true;               // 성공 Feedback
    }
}
```

**카드 → 필드 (SpriteRenderer)**

```csharp
public class CardDraggable : BaseDraggable<CardZone>
{
    protected override bool TryDrop(CardZone zone)
    {
        return zone.TryPlaceCard(this);
    }
}
```

---

## 주의사항

- **`BaseDraggable`은 추상 클래스라 직접 붙일 수 없습니다.** 하위 클래스를 만들어 붙입니다.
- **스크롤 목록 안의 오브젝트를 드래그 가능하게 만들면, 그 오브젝트 위에서는 스크롤이 되지 않습니다.** 드래그 이벤트를 드래그 오브젝트가 가져가기 때문입니다. 둘 다 필요하면 "스크롤 방향으로 끌면 부모 `ScrollRect`로 넘기고, 다른 방향이면 드래그"하는 처리를 하위 클래스에 추가해야 합니다.
- **드래그 도중 오브젝트가 꺼지면**(`OnDisable`) 레이캐스트 차단 상태만 되돌리고 원위치는 하지 않습니다.
- 같은 오브젝트에 `PointerFeedback`을 같이 붙여도 됩니다. 드래그로 판정되면 Unity가 클릭을 취소하므로, 드래그 후 클릭 Feedback이 잘못 재생되지 않습니다.
- `Awake()`를 override할 때는 `base.Awake()`를 호출해야 `CanvasGroup` / `Collider` 자동 연결이 동작합니다. `OnDisable()`도 마찬가지입니다.
