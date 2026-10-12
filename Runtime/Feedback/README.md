# Feedback

> 입력 이벤트에 Feel Feedback(`MMF_Player`)을 연결하는 컴포넌트입니다. `PointerFeedback`은 마우스·터치, `NavigationFeedback`은 키보드·게임패드를 담당합니다.

## Public API

### `PointerFeedback`

| Member | Description |
|---|---|
| `Left/Right Pointer Up/Down/Click Feedback` | 좌/우 버튼별 Up·Down·Click 시 재생합니다. |
| `Pointer Enter/Exit Feedback` | hover 진입·이탈 시 재생합니다. |
| `Selectable` | 비활성 여부를 판단할 Button 등. 비워두면 같은 오브젝트에서 찾습니다. |
| `Disabled Click Feedback` | 비활성 상태에서 클릭했을 때 재생합니다(거부 연출). |
| `SetInteractable(bool isInteractable)` | `Selectable`이 없는 오브젝트(Sprite, 3D)의 비활성 상태를 정합니다. |

### `NavigationFeedback`

| Member | Description |
|---|---|
| `Select / Deselect / Submit Feedback` | 커서가 오거나 떠날 때, 확인(Enter / 게임패드 A)할 때 재생합니다. |
| `Selectable` | 비활성 여부를 판단할 Button 등. 비워두면 같은 오브젝트에서 찾습니다. |
| `Disabled Submit Feedback` | 비활성 상태에서 확인했을 때 재생합니다. |

## 사용 예시

```
Btn_Buy
├─ Button
├─ PointerFeedback      ← 마우스·터치
└─ NavigationFeedback   ← 키보드·게임패드
```

필요한 Feedback 필드에만 `MMF_Player`를 할당합니다. 비워둔 필드는 재생되지 않습니다.

```csharp
// 잠긴 보물상자(Sprite) → 누르면 거부 연출
_chestFeedback.SetInteractable(false);
```

## 주의사항

- 씬에 `EventSystem`이 필요합니다. Sprite는 `Collider2D` + 카메라에 `Physics2DRaycaster`, 3D는 `Collider` + `PhysicsRaycaster`, UI는 Raycast Target이 켜진 Graphic이 있어야 합니다.
- 비활성 상태에서는 Up/Down/Enter/Exit Feedback을 재생하지 않고, 클릭 시 `Disabled Click Feedback`만 재생합니다. 거부 연출까지 끄려면 컴포넌트를 끕니다.
- `NavigationFeedback`은 마우스 클릭으로 선택된 경우를 무시합니다(hover 연출과 겹치지 않게).
- 두 컴포넌트 모두 드래그·스크롤 이벤트를 받지 않아 `ScrollRect` 안에서도 스크롤을 막지 않습니다. 대신 `EventTrigger`를 쓰면 스크롤이 막힙니다.
