# PhikozzLib Documentation

> Unity 프로젝트에서 반복적으로 필요한 공통 기능(서비스 초기화, 리소스, UI, 이펙트, 데이터, 씬 전환 등)을 모듈화한 라이브러리입니다.

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
- [Pooling](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Pooling/README.md)

## Feedback
- [PointerFeedback / NavigationFeedback](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Feedback/README.md)

## Camera
- [CameraManager](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Camera/README.md)

## Generic Singleton
- [Generic Singleton](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Singleton/README.md)

## StateMachine
- [StateMachine](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/StateMachine/README.md)

## Observable
- [ObservableValue](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Observable/README.md)

## Command
- [CommandHistory](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Command/README.md)

## Stat Modifier
- [Stat](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/StatModifier/README.md)

## Drag
- [BaseDraggable](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Drag/README.md)

## DevLog
- [DevLog](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Log/README.md)

## Editor Tools
- [Editor](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/README.md) — `BootstrapConfig` 설정 실수를 에디터 단계에서 미리 잡아주는 검증 툴

---

# 의존성/권장 설치 패키지
- [Feel](https://assetstore.unity.com/packages/tools/particles-effects/feel-183370) (필수) — Save/UI/Feedback/Time/Scene/FloatingText 모듈이 `MMF_Player`, `MMTimeManager`, `MMAdditiveSceneLoadingManager`, `MMFloatingTextSpawner` 등을 직접 사용합니다.
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
