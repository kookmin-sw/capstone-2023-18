[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-2e0aaae1b6195c2367325f4f02e2d04e9abb55f0b24a779b69b11b9e10269abc.svg)](https://classroom.github.com/online_ide?assignment_repo_id=10030695&assignment_repo_type=AssignmentRepo)
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

### 실행 방법

#### 실행 파일
**[실행파일 공유링크](https://1drv.ms/f/s!Aoe7hGnKbLVrhZ0ncqbkB6PUSjDS2g?e=RG6z4o)**

해당 링크에 접속하여 다운로드 하신 후, UnityProject.exe 를 실행하시면 데모버전 플레이가 가능합니다.
윈도우 환경에서만 가능하며 기본 해상도가 1600X1200으로 설정되어있어 이보다 낮은 해상도를 가지신 분들의 경우에는 현재 직접 해상도를 줄여주셔야 합니다.

이 부분은 추후 수정이 될 예정입니다.

추가적으로 게임 스타트 이후 CREATE를 눌러 방을 생성하신 후 게임을 시작해주시면 됩니다.
원래대로라면 레드, 블루팀의 인원이 맞아야 실행이 가능해야 하지만 테스트목적상 이러한 제한이 없는 상황입니다.

(23.05.23 이슈)
현재 이슈로 인하여 게임 플레이 이후 로비로 돌아오는것에 문제가 간헐적으로 발생하는 중입니다.
이러한 부분에 있어 빠르게 문제를 확인중이며 정상적으로 플레이 이후 로비로 돌아오게 되면 재업로드하겠습니다.

#### 에디터 파일

[전체 프로젝트 파일 다운로드](https://github.com/kookmin-sw/capstone-2023-18/archive/refs/heads/master.zip)

- 압축파일 해제 후, Unity Hub를 실행.
- 압축 해제 한 폴더 내에 UnityProject 폴더를 Unity 21.3.14f버전으로 실행.

### 포스터
![poster](/PageAssets/img/Poster.jpg)
