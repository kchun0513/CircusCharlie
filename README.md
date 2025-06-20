# CircusCharlie VR

**Team 삼김** - 2020006108 김영현, 2020037329 김충훈, 2019089952 김현태

## 프로젝트 소개
서커스 찰리를 VR로 재해석하여, 플레이어가 계속 도전하고 하이스코어를 갱신하는 재미를 주는 게임을 목표로 개발하였다. 높은 난이도와 몰입감 있는 VR 환경을 통해 성취감과 도전 욕구를 자극하는 플레이 경험을 제공한다.

### 개발 역할 분담
-	**김영현**: 스테이지, 메뉴 페이지 개발 및 오브젝트 구현
-	**김충훈**: VR연결, VR움직임 구현 및 스테이지 개발 도움
-	**김현태**: 맵, 오브젝트 디자인 및 사운드 적용

## 주요 기술 스택
- Unity (2022.3.60f1)
- Oculus XR Plugin (Meta XR)
- Blender (모델링, 리깅)
- GitHub (버전 관리)
- Jira (프로젝트 관리 보조)
- Visual Studio Code (코드 작성)
- ChatGPT (개발 보조)
- Oculus Quest 2 (테스트 기기)

## 스테이지

![Honeycam 2025-06-20 11-08-12](https://github.com/user-attachments/assets/c27ef96e-1b7c-4a93-9b6c-84b87cff872d)

1. **스테이지 1:** 채찍 및 고삐 동작으로 호랑이 조종하며 다가오는 불고리, 불단지를 뛰어넘어 골까지 이동하는 것이 목표이다.

    - **전진:** 오른쪽 컨트롤러를 빠르게 아래로 휘두르기  
    - **대쉬:** 오른쪽 트리거 버튼  
    - **정지:** 오른쪽 컨트롤러를 위로 올리기  
    - **점프:** 왼쪽 X 버튼
    - **이동 방향:** 기기 회전값 (고개 방향)

![Honeycam 2025-06-20 11-08-26](https://github.com/user-attachments/assets/9cc086ec-eca0-4c3c-9d6e-5ec16e31e8de)

2. **스테이지 2:** 양 옆의 균형을 유지하며 달려오는 원숭이 회피하며 골까지 이동하는 것이 목표이다. 빨간 표식이 있는 원숭이는 앞의 원숭이를 건너 뛰며 보다 빨리 달려온다.
    
    - **전진:** 오른쪽 트리거 버튼  
    - **점프:** 왼쪽 X 버튼
    - **이동 방향:** 기기 회전값 (고개 방향)
    - **주의:** 컨트롤러 포인터가 BalanceZone을 벗어나지 않도록 유지한다.

![Honeycam 2025-06-20 11-09-33](https://github.com/user-attachments/assets/6c67c5e0-b6f0-4ff6-bc6c-32ccaac062b5)

3. **스테이지 3:** 말 조종 및 다가오는 트램펄린을 밟고 점프를 하거나 회피하며 골까지 이동하는 것이 목표
   
    - **대쉬:** 오른쪽 컨트롤러를 빠르게 아래로 휘두르기  
    - **감속:** 오른쪽 컨트롤러를 위로 올리기  
    - **점프:** 왼쪽 X 버튼
    - **이동 방향:** 기기 회전값 (고개 방향)

## 메뉴 기능

- **세팅 페이지:** 마스터, BGM, SFX 볼륨 조절
- **점수 페이지:** 최고 점수 기록 및 확인 (상위 5명)

## 점수 시스템

- 기본 5000점에서 점감
- 장애물 회피 성공 시 점수 추가
- 매 10000점 달성시 목숨 추가

## 설치 및 실행 가이드

1. 본 레포지토리 클론 후 Unity 프로젝트 열기
2. Oculus Quest 2 와 PC를 연결한 후 개발자 모드 설정
3. Build Settings에서 Platform을 Android로 설정
4. Run Device 항목에서 Oculus Quest 2 선택
5. Scenes In Build를 다음과 같이 선택
   
![스크린샷 2025-06-20 111838](https://github.com/user-attachments/assets/75e4f115-1dc0-4624-831b-8f9e84c6ffe1)

6. Build to Device 옆의 Patch And Run 실행

## 게임 플로우차트
![Activity Diagram](https://github.com/user-attachments/assets/f2e4e429-a13f-4682-9445-ad96f8e426f4)

## 주요 스크립트
[GameManager](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/GameManager.cs) : 게임 실행, 정지 및 화면 전환

[SoundManager](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/SoundManager.cs) : 게임 음악 관리 스크립트

[SettingManager](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/SettingManager.cs) : 게임 설정 관리 스크립트

[ThirdPersonController](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/ThirdPersonController.cs) : 플레이어 조종 스크립트, StarterAssets 에셋 기반

- [DetectShakeGesture()](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/ThirdPersonController.cs#L555) : 컨트롤러 내리기 감지 메서드
- [DetectPullGesture()](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/ThirdPersonController.cs#L581) : 컨트롤러 당기기 감지 메서드
- [GetMoveRotation()](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/ThirdPersonController.cs#L376) : 기기 회전값
    
[PlayerManager](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/PlayerManager.cs) : 플레이어-오브젝트 접촉 관리 및 스테이지별 점수 관리 목적 스크립트

[ObjGenController](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/ObjGenController.cs) : 오브젝트 생성 관리 스크립트

[ObjectController](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/ObjectController.cs) : 오브젝트 움직임 관리 스크립트

[BalanceManager](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/BalanceManager.cs) : 스테이지2 컨트롤러 밸런스 관리 스크립트

[WhipEffect](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/WhipEffect.cs) : 채찍 효과 재생 스크립트

[StandSpawner](https://github.com/kchun0513/CircusCharlie/blob/main/Assets/Scripts/StandSpawner.cs) : 원형 다층 관중석 자동 배치 스크립트




