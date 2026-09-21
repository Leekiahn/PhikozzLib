# PhikozzLib Documentation

PhikozzLib는 Unity 프로젝트에서 반복적으로 필요한 공통 기능과 시스템을 모듈화한 커스텀 라이브러리입니다.  
서비스 초기화, 리소스 관리, UI, 이펙트, 데이터, 씬 전환 등 프로젝트 전반에서 자주 사용되는 기능을 일관된 방식으로 제공하여 개발 생산성과 유지보수성을 높이는 것을 목표로 합니다.

각 기능은 독립적인 모듈로 구성되어 있어 프로젝트 요구 사항에 맞게 필요한 시스템만 선택적으로 사용할 수 있습니다. 서비스 로케이터와 부트스트랩 구조를 기반으로 초기화 흐름과 의존성 관리를 단순화했습니다.

## Services
- [Service Locater](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/SeviceLocator/README.md)
- [Bootstrap](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Bootstrap/README.md)
- [Addressable](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Addressable/README.md)
- [Audio](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Audio/README.md)
- [UI](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/UI/README.md)
- [Data](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Data/README.md)
- [Event](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Event/README.md)
- [Scene](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Scene/README.md)
- [Effect](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Effect/README.md)
- [Save](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Save/README.md)
- [Time](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Time/README.md)
- [Localization](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Localization/README.md)
- [FloatingText](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/FloatingText/README.md)

## Camera
- [CameraManager](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Camera/README.md)

## Pooling
- [TrackedPool](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Pooling/README.md)

## Generic Singleton
- [Generic Singleton](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Singleton/README.md)

## StateMachine
- [StateMachine](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/StateMachine/README.md)

## Editor Tools
- [Editor](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/README.md) — `BootstrapConfig` 설정 실수를 에디터 단계에서 미리 잡아주는 검증 툴

---

## 설계 원칙

- **서비스는 `ServiceLocator` + `Bootstrapper`로 등록/조회합니다.** 각 매니저는 `IServiceRegister`(`RegisterService`/`UnregisterService`)를 구현하고, 다른 서비스를 참조해야 하면 `Awake()` 대신 `IServiceInit.InitAsync()`을 씁니다. 비동기 초기화가 끝난 뒤 호출해야 하는 게임 로직은 `Bootstrapper.WaitUntilReadyAsync()`를 대기합니다. 자세한 내용은 [ServiceLocater](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/SeviceLocator/README.md) 참고.
- **패키지는 git URL로 설치되는 걸 전제로 설계했습니다.** `Effect`/`Data` 모듈처럼 특정 프로젝트에만 있는 코드(BGDatabase 생성 클래스 등)에 의존해야 할 수 있는 부분은, 패키지가 직접 참조하지 않고 `RegisterEffect`/`AddDataContainer` 같은 주입 API만 제공합니다. 실제 연결 코드는 전체가 주석 처리된 `ExampleXLoader.cs` 형태로만 남겨두고, 각 프로젝트가 그 내용을 복사해서 씁니다.
- **Feedback(MoreMountains Feel)은 켜고 끄는 옵션이 아니라, 붙이면 쓰는 것을 전제로 합니다.** `UIPopup`/`UISlot`/`UIButtonFeedback`의 Feedback 관련 필드는 방어적으로 null 체크하지 않는 경우가 있습니다 — 안 쓸 거면 해당 필드/컴포넌트 자체를 비워두거나 안 붙이면 됩니다.

---

# 의존성/권장 설치 패키지
- [Feel](https://assetstore.unity.com/packages/tools/particles-effects/feel-183370) (필수) — Save/UI/Time/Scene/FloatingText 모듈이 `MMF_Player`, `MMTimeManager`, `MMAdditiveSceneLoadingManager`, `MMFloatingTextSpawner` 등을 직접 사용합니다.
- [Odin Inspector](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041) (필수)
- [UniTask](https://github.com/cysharp/unitask) (필수)
- Localization (자동)
- Addressables (자동)
- Cinemachine (자동)
- [BG Database](https://assetstore.unity.com/packages/tools/integration/bg-database-data-editor-with-google-sheets-and-excel-syncing-112262) (선택) — 패키지 자체는 BGDatabase에 의존하지 않습니다. `Effect`/`Data` 모듈의 `ExampleXLoader.cs` 예시가 BGDatabase 사용을 전제로 작성돼 있을 뿐이며, 다른 데이터 소스를 써도 됩니다.

---

# 패키지 설치

<img width="504" height="137" alt="Image" src="https://github.com/user-attachments/assets/1774c18c-b9ea-42ff-8d29-7262bd725619" />

- Package Manager -> Install Package from git URL에 https://github.com/Leekiahn/PhikozzLib.git 링크를 붙여넣고 Install합니다.
- 먼저 필요로 하는 필수 패키지를 모두 설치해야 합니다.
