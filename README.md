# Flareborn : The Soul Devourer

![Animation_0](https://github.com/user-attachments/assets/865d1a21-768c-43d8-947f-dc07064a37cb)

<br>

## 📖 목차

- [프로젝트 정보](-프로젝트-정보)
- [팀원 소개](-팀원-소개)
- [게임 소개](-게임-소개)
- [주요 기능](-주요-기능)
- [스크립트 정보](-스크립트-정보)

---

## 프로젝트 정보

* **개발 기간**: 2025.06.20 ~ 2025.08.14
* **개발 환경**: Unity 6000.1.8f1
* **플랫폼**: PC
* **게임 플레이**: [게임 다운로드](https://drive.google.com/file/d/17VHNLu1sp28Al4S3X_OFdt4t-0lplQVy/view)

---

## 팀원 소개

* **팀장**: 이자연
* **부팀장**: 유도균
* **팀원**: 박민
* **팀원**: 김석호

---

## 게임 소개
- **장르**: 2.5D 턴제 RPG
- **설명**: 플레이어는 주인공 **루티엔**이 되어 모험 떠나게 됩니다. 매력적인 동료들과 함께 몬스터를 물리치고, 화려한 이펙트와 박진감 넘치는 턴제 전투의 재미를 경험해보세요!

---

## 주요 기능

- **캐릭터 시스템**
  - 직업, 능력치, 스킬, 스탯 관리가 가능한 캐릭터 인스턴스 기반 구조.
  - 파티 멤버 관리 및 체력, 경험치, 장비 상태 저장 가능.

- **턴제 전투 시스템**
  - 플레이어와 적이 번갈아 행동하는 전투 시스템.
  - 스킬, 아이템 사용, 속도 기반 턴 처리 등 전략적인 전투 구현.

- **인벤토리 및 아이템 시스템**
  - 장비, 소비, 퀘스트, 룬 아이템 등 분류별 관리.
  - 스택형/비스택형 아이템 처리 및 UI 연동.

---

## 스크립트 정보

### 배틀

| 스크립트 경로                                                                                                                               | 설명                     | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------ | ----------- |
| [BattleSystem](https://github.com/pm2979/Flareborn_Code/blob/main/Battle/BattleSystem.cs)                                                    | 전투 흐름과 턴 진행 관리 | 박민        |
| [EffectManger](https://github.com/pm2979/Flareborn_Code/blob/main/Battle/BattleEffect/EffectManger.cs)                                        | 전투 중 이펙트 관리      | 박민        |
| [BattleEntities](https://github.com/pm2979/Flareborn_Code/blob/main/Battle/Entities/BattleEntities.cs)                                        | 배틀 유닛 관리           | 박민        |
| [EncounterSystem](https://github.com/pm2979/Flareborn_Code/blob/main/Battle/EncounterSystem.cs)                                              | 전투 진입 관리           | 유도균      |

### 캐릭터

| 스크립트 경로                                                                                                                               | 설명                   | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------- | ----------- |
| [CharacterInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Character/CharacterInstance.cs)                                        | 캐릭터 정보 관리       | 박민        |
| [EquipmentController](https://github.com/pm2979/Flareborn_Code/blob/main/Character/Equip/EquipmentController.cs)                              | 장비 장착 관리         | 박민        |
| [SkillInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Character/Skill/SkillInstance.cs)                                          | 스킬 정보 관리         | 박민        |
| [TraitController](https://github.com/pm2979/Flareborn_Code/blob/main/Character/Traits/TraitController.cs)                                     | 특성 관리              | 박민        |

### 적

| 스크립트 경로                                                                                                                               | 설명                     | 주요 기여자   |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------ | ------------- |
| [EnemyInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Enemy/EnemyInstance.cs)                                                    | 적 정보 관리             | 박민, 유도균  |
| [BaseEnemyAI](https://github.com/pm2979/Flareborn_Code/blob/main/Enemy/Battle/EnemyAI/BaseEnemyAI.cs)                                         | 적의 배틀 상태 추상 클래스 | 박민          |
| [OverWorldEnemy](https://github.com/pm2979/Flareborn_Code/blob/main/Enemy/OverWorld/OverWorldEnemy.cs)                                        | 적의 필드 상태 관리      | 유도균        |
| [EnemySpawner](https://github.com/pm2979/Flareborn_Code/blob/main/Enemy/EnemySpawner.cs)                                                      | 적 스폰 관리             | 유도균        |

### 던전

| 스크립트 경로                                                                                                                               | 설명             | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ---------------- | ----------- |
| [DungeonInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Dungeon/DungeonInstance.cs)                                              | 던전 정보 관리   | 유도균      |
| [DungeonPortal](https://github.com/pm2979/Flareborn_Code/blob/main/Dungeon/DungeonPortal.cs)                                                  | 던전 포탈 관리   | 유도균      |

### 아이템

| 스크립트 경로                                                                                                                               | 설명             | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ---------------- | ----------- |
| [ItemInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Item/ItemInstance.cs)                                                      | 아이템 정보 관리 | 박민        |
| [Inventory](https://github.com/pm2979/Flareborn_Code/blob/main/Item/Inventory/Inventory.cs)                                                   | 인벤토리 관리    | 김석호      |
| [RuneCrafting](https://github.com/pm2979/Flareborn_Code/blob/main/Item/Rune/RuneCrafting.cs)                                                  | 룬 생성          | 박민        |
| [CurrencyWallet](https://github.com/pm2979/Flareborn_Code/blob/main/Item/Wallet/CurrencyWallet.cs)                                            | 화폐 관리        | 김석호      |

### NPC 및 퀘스트

| 스크립트 경로                                                                                                                               | 설명             | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ---------------- | ----------- |
| [NPC](https://github.com/pm2979/Flareborn_Code/blob/main/Overworld/NPC/NPC.cs)                                                               | NPC 관리         | 이자연      |
| [Quest](https://github.com/pm2979/Flareborn_Code/blob/main/Overworld/QuestSystem/Quest.cs)                                                   | 퀘스트 관리      | 이자연      |
| [TutorialManager](https://github.com/pm2979/Flareborn_Code/blob/main/Overworld/QuestSystem/Tutorial/TutorialManager.cs)                       | 튜토리얼 관리    | 이자연      |
| [DialogueActivator](https://github.com/pm2979/Flareborn_Code/blob/main/Overworld/DialogueSystem/DialogueActivator.cs)                         | 다이얼로그 관리  | 이자연      |

### UI

| 스크립트 경로                                                                                                                               | 설명             | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ---------------- | ----------- |
| [BattleUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/BattleUI.cs)                                                                | 배틀UI           | 박민        |
| [OverworldUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/OverworldUI.cs)                                                          | 필드UI 관리      | 박민        |
| [EquipmentUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/EquipmentUI/EquipmentUI.cs)                                              | 장비 장착UI      | 박민        |
| [InventoryUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/Inventory/InventoryUI.cs)                                                | 인벤토리UI       | 박민        |
| [StatusUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/StatUI/StatusUI.cs)                                                         | 스테이터스UI     | 박민        |
| [SoundSettings](https://github.com/pm2979/Flareborn_Code/blob/main/UI/SoundOptionUI/SoundSettings.cs)                                        | 사운드 관리UI    | 김석호      |
| [QuestLogUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/QuestUI/QuestLogUI.cs)                                                     | 퀘스트UI         | 이자연      |

