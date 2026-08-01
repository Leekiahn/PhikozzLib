# BootstrapConfigEditorWindow

<img width="458" height="537" alt="Image" src="https://github.com/user-attachments/assets/ccb1ec0e-e295-4cd6-94e2-208f80178ce0" />

> `BootstrapConfig` 에셋을 편집하고 `Resources/BootstrapConfig.asset`으로 복사 배치하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Bootstrap Config Editor`

## 사용 조건
- Edit Mode에서 사용 가능
- 대상 `BootstrapConfig` 에셋을 지정해야 합니다.

## 주요 기능
- `_managers` 목록 직접 편집
- `Assets/Resources` 폴더 자동 생성
- `BootstrapConfig.asset` 생성 또는 갱신
- 저장된 에셋 선택 및 Ping 처리

## 주요 입력값
- `Config`: 편집할 `BootstrapConfig` 에셋

## 주요 버튼
- `Place In Resources`

## 참고
- 저장 대상 경로는 항상 `Assets/Resources/BootstrapConfig.asset`입니다.
- 대상 파일이 없으면 새로 만들고, 있으면 직렬화 데이터를 복사해 덮어씁니다.
