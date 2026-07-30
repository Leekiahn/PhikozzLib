# Core
```csharp
Core.Audio.PlayBgm(0, 0);
Core.Addressable.Get<GameObject>("Dungeon", "Monster");
```
- ServiceLocater에 등록된 서비스들의 전역 접근 진입점을 단일화한 정적 클래스입니다.

<br>
<br>

```csharp
public static IAddressableService Addressable => ServiceLocator.Get<IAddressableService>();
public static IAudioService Audio => ServiceLocator.Get<IAudioService>();
public static IUIService UI => ServiceLocator.Get<IUIService>();
```
- 위처럼 정적 필드에 서비스를 캐싱합니다.
