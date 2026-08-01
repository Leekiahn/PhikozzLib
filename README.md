# PhikozzLib Documentation

PhikozzLib는 Unity 프로젝트에서 반복적으로 필요한 공통 기능과 시스템을 모듈화한 커스텀 라이브러리입니다.  
서비스 초기화, 리소스 관리, UI, 오디오, 데이터, 씬 전환 등 프로젝트 전반에서 자주 사용되는 기능을 일관된 방식으로 제공하여 개발 생산성과 유지보수성을 높이는 것을 목표로 합니다.

각 기능은 독립적인 모듈로 구성되어 있어 프로젝트 요구 사항에 맞게 필요한 시스템만 선택적으로 사용할 수 있습니다. 또한 서비스 로케이터와 부트스트랩 구조를 기반으로 초기화 흐름과 의존성 관리를 단순화하며, 런타임 기능을 지원하는 다양한 EditorWindow 도구를 함께 제공합니다.

## Services
- [Service Locater](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/SeviceLocator/README.md)
- [Bootstap](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Bootstrap/README.md)
- [Core](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/README.md)
- [Addressable](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Addressable/README.md)
- [Audio](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Audio/README.md)
- [UI](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/UI/README.md)
- [Data](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Data/README.md)
- [Event](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Event/README.md)
- [Scene](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Scene/README.md)
- [Effect](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Effect/README.md)
- [Save](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Save/README.md)
- [Time](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Time/README.md)
- [Input](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Input/README.md)
- [Localization](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/Localization/README.md)
- [FloatingText](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Core/FloatingText/README.md)

## Canera
- [CameraManager](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Camera/README.md)

## Pooling
- [TrackedPool](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Pooling/README.md)

## Generic Singleton
- [Generic Singleton](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/Singleton/README.md)

## StateMachine
- [StateMachine](https://github.com/Leekiahn/PhikozzLib/blob/main/Runtime/StateMachine/README.md)

## Editor
- [Editor Windows](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/README.md)
- [BootstrapConfigEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/BootstrapConfigEditorWindow/README.md)
- [AddressableEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/AddressableEditorWindow/README.md)
- [AudioEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/AudioEditorWindow/README.md)
- [CameraEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/CameraEditorWindow/README.md)
- [EffectEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/EffectEditorWindow/README.md)
- [FloatingTextEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/FloatingTextEditorWindow/README.md)
- [LocalizationEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/LocalizationEditorWindow/README.md)
- [SceneEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/SceneEditorWindow/README.md)
- [TimeEditorWindow](https://github.com/Leekiahn/PhikozzLib/blob/main/Editor/TimeEditorWindow/README.md)



---


   
# 의존성/권장 설치 패키지
- [Feel](https://assetstore.unity.com/packages/tools/particles-effects/feel-183370) (필수)
- [BG Database](https://assetstore.unity.com/packages/tools/integration/bg-database-data-editor-with-google-sheets-and-excel-syncing-112262) (필수)
- [Odin Inspector](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041) (필수)
- [UniTask](https://github.com/cysharp/unitask) (필수)
- Localization (자동)
- Addressables (자동)
- Cimemachine (자동)
- Input System (자동)

---

# 패키지 설치

<img width="504" height="137" alt="Image" src="https://github.com/user-attachments/assets/1774c18c-b9ea-42ff-8d29-7262bd725619" />

- Pachage Manager -> Install Package from git URL에 https://github.com/Leekiahn/PhikozzLib.git 해당 링크를 붙여넣고 Install합니다.
- 먼저 필요로 하는 필수 패키지를 모두 설치해야 합니다.
- `Library\PackageCache\com.phikozz.phikozzlib`을 `Packages` 폴더로 옮기면 코드 수정이 가능합니다.




