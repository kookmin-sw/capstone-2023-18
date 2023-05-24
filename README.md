# ‘Driple - Drift College’
![teamLogo](/PageAssets/img/dripple-logo-color.png)

----

## [팀 페이지 방문하기](https://kookmin-sw.github.io/capstone-2023-18)

### 1. 프로젝트 소개

**[Unity3D를 사용한 실시간 멀티플레이 레이싱 게임 제작]**

‘드리프트 콜리지’는 캐주얼 아케이드 레이싱 게임으로서, 플레이어는 다른 플레이어와 팀을 결성하여 다른 팀보다 먼저 자신의 팀원을 결승선에 도착시키는 것을 목적으로 진행하게 되는 게임이다. 플레이어들은 ‘국민대학교’를 배경으로 한 교내 형태의 맵을 질주하며 맵 중간 중간에 위치한 아이템을 사용하여 팀의 플레이를 유리하게 이끌어 갈 수 있다.

프로젝트는 Unity를 기반으로 진행된다.
Unity와 C#을 사용하여 카트바디의 주행, 플레이 요소등을 구현하게 된다. 해당 프로젝트의 게임방식은 기본적으로 싱글플레이와 멀티플레이로 구분된다. 싱글플레이는 지정된 맵을 학습된 AI와 유저가 대결하여 실력을 키울 수 있게 장려하는 방식이다. 이를 위해 PyTorch-TensorBoard를 활용하는 Unity ML-Agent를 활용하여 맵을 주행하는 AI 카트바디 모델을 학습시킨다. 멀티플레이의 경우에는 실시간 네트워크 대전을 구현한다. 이를 위해 Unity Gaming Services에서 제공되는 Relay와 Lobby를 사용한다. 이 두 가지를 사용하여 MatchMaking 및 P2P 연결을 가능하게 한다. 마지막으로 실제 존재하는 대학가 배경을 구현하기 위해 3D 오브젝트 모델링 툴인 Blender를 사용하여 그래픽 리소스를 제작한다. 

현재 넥슨에서 서비스 되고 있는 '카트라이더'와 같은 캐쥬얼 레이싱 게임을 제작한다.

**목표**

1. 자동차의 물리법칙에 대해 연구하며 직접 구현함으로서 실제 서비스 되고 있는 게임수준의 조작감과 플레이 경험을 구현. - 완료

2. Unity ML-Agent를 활용하여 레이싱게임 Bot을 제작. - 완료

3. 기존 레이싱게임과의 차별요소를 제작합니다 - 역할군 시스템 완료

4. 실제 대학교정의 느낌을 받을 수 있도록 맵 디자인 - 완료

크게 위의 4가지의 목표를 달성함을 목적으로 진행되는 프로젝트이다.

클라이언트 제작에서는 Unity/C#을 사용하며, 필요한 에셋(카트 디자인, 맵 등)은 Blender를 사용하여 제작한다.

멀티플레이를 위한 서버의 경우에는 유저의 데이터를 관리하는것보다는 익명 로그인으로 멀티플레이를 진행하는것을 우선을 목표로 하기때문에 Unity Relay+Robby 서비스를 활용하여 매치메이킹 시스템을 제작한다.

**사용 기술**
![preview](/PageAssets/img/UseSkill.png)

### 2. 시연 영상

**[![Video Label](http://img.youtube.com/vi/C-_7uvCk1UU/0.jpg)](https://youtu.be/C-_7uvCk1UU)**
-

### 3. 팀 소개

| 이름 | 학번 | 이메일 | 담당파트 |
| --- | --- | --- | --- |
| 원상연 | 20171654 | cdsywon@kookmin.ac.kr | 카트 물리엔진 |
| 김성현 | 20181586 | archun39@kookmin.ac.kr | 인공지능 플레이어 |
| 안현웅 | 20171645 | asd212164@kookmin.ac.kr | 3D오브젝트 및 UI |

### 4. 사용법

- 프로젝트 파일 다운로드.

[](https://github.com/kookmin-sw/capstone-2023-18/archive/refs/heads/master.zip)

- 압축파일 해제 후, Unity Hub를 실행.
- 압축 해제 한 폴더 내에 UnityProject 폴더를 Unity 21.3.14f버전으로 실행.

### 포스터
![poster](/PageAssets/img/Poster.jpg)
