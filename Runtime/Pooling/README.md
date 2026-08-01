# Tracked Pool

> 오브젝트 풀링을 위한 유틸리티를 제공합니다.  
> 활성 오브젝트를 추적할 수 있는 `TrackedPool<T>`를 구현합니다.

---

## 주요 기능

- `UnityEngine.Pool.ObjectPool<T>` 기반 풀링
- 활성 오브젝트 추적
- 마지막에 생성된 활성 오브젝트부터 반환
- 특정 오브젝트 직접 반환 가능
- 전체 활성 오브젝트 반환
- 풀 전체 초기화 지원

---

## Public API

| Method | Description |
|---|---|
| `Get()` | 풀에서 오브젝트를 꺼내고 활성 목록에 추가합니다. |
| `Release()` | 가장 최근 활성 오브젝트를 풀로 반환합니다. |
| `Release(T obj)` | 지정한 오브젝트를 풀로 반환합니다. |
| `ReleaseAll()` | 활성 상태인 모든 오브젝트를 풀로 반환합니다. |
| `Clear()` | 활성 오브젝트를 반환한 뒤 풀 자체를 비웁니다. |

---

## 생성자

| Parameter | Description |
|---|---|
| `onCreate` | 새 오브젝트를 생성하는 함수입니다. |
| `onGet` | 오브젝트를 꺼낼 때 호출됩니다. |
| `onRelease` | 오브젝트를 반환할 때 호출됩니다. |
| `onDestroy` | 오브젝트가 파괴될 때 호출됩니다. |
| `defaultCapacity` | 초기 용량입니다. |
| `maxSize` | 최대 풀 크기입니다. |


## 사용

```csharp
    [SerializeField] private Test _prefab;
    [SerializeField] private Transform _parent;
    
    private TrackedPool<Test> _pool;

    private void Awake()
    {
        _pool = new TrackedPool<Test>
        (
            onCreate: () =>
            {
                var instance = Instantiate(_prefab, _parent);
                instance.Init();
                return instance;
            },
            onGet: instance =>
            {
                instance.gameObject.SetActive(true);
            },
            onRelease: instance =>
            {
                instance.gameObject.SetActive(false);
            },
            onDestroy: instance =>
            {
                Destroy(instance.gameObject);
            }
        );
    }
```
