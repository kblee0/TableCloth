﻿#language: ko-KR
기능: ResourceResolver

원격지 서버에 있는 문서나 이미지, 파일을 다운로드하거나 정보를 가져오는 것과 관련된 동작을 담당하는 기능을 모아둔 컴포넌트입니다.

시나리오: a. 카탈로그 문서를 불러온다.
만일 a.a. 카탈로그 문서를 불러오는 함수를 호출하면
그러면 a.b. 카탈로그 문서에 1개 이상의 사이트 정보가 들어있다.
그리고 a.c. 마지막으로 카탈로그를 불러온 날짜와 시간 정보를 확인할 수 있다.

시나리오: b. 프로그램의 최신 정보를 가져온다.
먼저 b.a. 다음의 리포지터리에서 정보를 가져오려 한다.
  | 소유자 이름    | 리포지터리 이름   |
  | yourtablecloth | TableCloth        |
만일 b.b. 버전 정보를 가져오는 함수를 호출하면
그러면 b.c. GitHub에 출시한 최신 버전 정보를 반환한다.

시나리오: c. 프로그램의 다운로드 URL을 가져온다.
먼저 c.a. 다음의 리포지터리에서 정보를 가져오려 한다.
  | 소유자 이름    | 리포지터리 이름   |
  | yourtablecloth | TableCloth        |
만일 c.b. 다운로드 URL을 가져오는 함수를 호출하면
그러면 c.c. GitHub에서 최신 버전의 리소스를 다운로드할 수 있는 URL을 반환한다.