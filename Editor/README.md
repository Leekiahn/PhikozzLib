# Editor Windows

<img width="296" height="264" alt="Image" src="https://github.com/user-attachments/assets/4b7e0302-6e66-48ec-beca-972eef150192" />

> PhikozzLib에서 제공하는 EditorWindow 도구 모음입니다.  
> 대부분의 창은 `BaseEditorWindow`를 기반으로 하며, 메뉴 `PhikozzLib/...` 경로에서 열 수 있습니다.

---

## EditorWindow 목록
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

## 공통 특징
- `BaseEditorWindow`의 UI 헬퍼를 사용해 일관된 레이아웃을 제공합니다.
- 런타임 서비스 테스트용 창은 대부분 **Play Mode 전용**입니다.
- 여러 창은 `Core` 진입점 또는 씬 내 매니저를 통해 서비스를 찾습니다.
- 일부 창은 테스트 후 상태를 자동으로 복구하거나 임시 데이터를 정리합니다.
