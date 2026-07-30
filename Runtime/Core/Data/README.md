# Core.Data
- `BaseData.cs`를 상속받는 데이터 클래스를 `DataContainer<T>`에 담아 초기화/로드하고 ID/이름 조회합니다.
- `IDataService` 인터페이스를 상속받아 구현합니다.
- 현재 `BG Database` 에셋에 의존합니다.

<br>

## Overview

- 게임 시작 시 데이터 로드
- 원본 데이터 엔티티를 런타임 데이터 객체로 변환
- 타입별 데이터 컨테이너 초기화
- Id, Name 기준 빠른 조회 제공
- 전체 데이터 컬렉션 순회 지원

<br>

## Features

- 타입별 데이터 관리: `DataContainer<T>`를 사용해 데이터 종류별로 분리 관리합니다.
- 빠른 조회: Id, Name 기준 딕셔너리 조회를 지원합니다.
- 전체 순회 지원: 리스트 기반 전체 데이터 열람이 가능합니다.
- 확장 쉬운 구조: 새 데이터 타입을 추가하고 Load()에 초기화 메서드를 연결하면 확장할 수 있습니다.
- 로컬라이징 대응 기반: `LocalizedBaseData`를 상속받은 데이터는 Unity Localization에 대응이 가능합니다.
- 전역 접근: Core.Data를 통해 어디서든 접근 가능합니다.

<br>

## Abstract Data Class

| Type | Role |
| --- | --- |
| `BaseData` |  모든 데이터 모델의 공통 베이스 클래스  |
| `LocalizedBaseData ` | 로컬라이징 참조 정보를 포함하는 데이터 베이스 클래스 |
| `DataContainer<T>` | 타입별 데이터 조회/보관 컨테이너 |

<br>

## Public API

### `DataContainter<T>`

| Method | Description |
|-----|-----|
| ` Get(int id) ` |  Id로 데이터를 조회합니다.  |
| ` Get(string name) `|  Name으로 데이터를 조회합니다.  |
| ` GetAll() ` |  전체 데이터를 반환합니다.  |


<br>

## Setup

<img width="759" height="403" alt="Image" src="https://github.com/user-attachments/assets/b39f403f-97eb-4abf-9cd7-9b4ccca9b594" />

- 구글 스프레드시트를 작성합니다.
- name(A1)열과 id(B1)열은 필수로 존재해야 합니다.
- `LocalizedBaseData`를 상속한다면 C1열과 D1열에 각각 테이블 참조(string)과 엔트리 참조(string)가 존재해야 합니다.

<br>

<img width="1143" height="532" alt="Image" src="https://github.com/user-attachments/assets/c7e3da59-946f-4d7f-a4a9-6f3ad44782b0" />

- 

```csharp
public class TestData : LocalizedBaseData
{
    public int Power { get; private set; }
    
    public TestData(BG_TestData data) : base(data.id, data.name, data.locale_table_ref, data.locale_entry_ref)
    {
        Power = data.power;
    }
}
```
- `BaseData` 혹은 `LocalizedBaseData`를 상속받아 Data클래스를 작성합니다.


