# 도서 관리 시스템 (WPF + MVVM)

WPF와 MVVM 패턴을 활용하여 구현한 데스크탑 도서 관리 애플리케이션.  
SQLite 데이터베이스와 Entity Framework Core를 연동하여 로컬에서 CRUD 및 다양한 기능을 지원.

---

## 주요 기능

| 기능 | 설명 |
|------|------|
| 도서 추가/수정/삭제 | 제목, 저자, 출판사, 출판일, 카테고리 등 입력 가능 |
| 검색 & 필터링 | 제목 검색, 카테고리 필터링 |
| CSV 내보내기 | 현재 목록을 CSV 파일로 저장 |
| 전체 삭제 | 모든 도서를 한 번에 삭제 (확인창 포함) |
| 카테고리 통계 | 카테고리별 도서 수 실시간 표시 |
| 최근 도서 Top 5 | 가장 최근 출판된 도서 5권 표시 |
|  프린트 / PDF 출력 | 필터링된 목록을 프린트 또는 PDF로 출력 가능 |

---

## 기술 스택

- **UI**: WPF (.NET 8), MVVM 패턴
- **DB**: SQLite + Entity Framework Core
- **언어**: C#
- **기타**: XAML, LINQ, PrintDialog, SaveFileDialog

---
