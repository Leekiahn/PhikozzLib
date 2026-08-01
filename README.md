# PhikozzLib Documentation

PhikozzLib는 Unity 프로젝트에서 반복적으로 필요한 공통 기능과 시스템을 모듈화한 커스텀 라이브러리입니다.  
서비스 초기화, 리소스 관리, UI, 오디오, 데이터, 씬 전환 등 프로젝트 전반에서 자주 사용되는 기능을 일관된 방식으로 제공하여 개발 생산성과 유지보수성을 높이는 것을 목표로 합니다.

각 기능은 독립적인 모듈로 구성되어 있어 프로젝트 요구 사항에 맞게 필요한 시스템만 선택적으로 사용할 수 있습니다. 또한 서비스 로케이터와 부트스트랩 구조를 기반으로 초기화 흐름과 의존성 관리를 단순화하며, 런타임 기능을 지원하는 다양한 EditorWindow 도구를 함께 제공합니다.

## Services
- [Service Locater](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/SeviceLocator)
- [Bootstap](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Bootstrap)
- [Core](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core)
- [Addressable](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Addressable)
- [Audio](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Audio)
- [UI](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/UI)
- [Data](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Data)
- [Event](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Event)
- [Scene](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Scene)
- [Effect](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Effect)
- [Save](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Save)
- [Time](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Time)
- [Input](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Input)
- [Localization](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/Localization)
- [FloatingText](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Core/FloatingText)

## Canera
- [CameraManager](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Camera)

## Pooling
- [TrackedPool](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Pooling)

## Generic Singleton
- [Generic Singleton](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/Singleton)

## StateMachine
- [StateMachine](https://github.com/Leekiahn/PhikozzLib/tree/main/Runtime/StateMachine)

## Editor
- [Editor Windows](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor)
- [AddressableEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/AddressableEditorWindow)
- [AudioEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/AudioEditorWindow)
- [BootstrapConfigEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/BootstrapConfigEditorWindow)
- [CameraEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/CameraEditorWindow)
- [EffectEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/EffectEditorWindow)
- [FloatingTextEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/FloatingTextEditorWindow)
- [LocalizationEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/LocalizationEditorWindow)
- [SceneEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/SceneEditorWindow)
- [TimeEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/TimeEditorWindow)



---


   
# 의존성/권장 설치 패키지
- Feel (필수)
- BG Database (필수)
- Odin Inspector (필수)
- Localization (자동)
- Addressables (자동)
- UniTask (자동)
- Cimemachine (자동)
- Input System (자동)

---

# 패키지 설치

<img width="504" height="137" alt="Image" src="https://github.com/user-attachments/assets/1774c18c-b9ea-42ff-8d29-7262bd725619" />

- Pachage Manager -> Install Package from git URL에 https://github.com/Leekiahn/PhikozzLib.git 해당 링크를 붙여넣고 Install합니다.
- 필요로 하는 필수 패키지를 모두 설치해야 합니다.




