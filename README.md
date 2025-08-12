## 배틀

| 스크립트 경로                                                                                                                                     | 설명                 | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------ | ------ |
| [BattleSystem](https://github.com/pm2979/Flareborn_Code/blob/main/Battle/BattleSystem.cs)         | 전투 흐름과 턴 진행 관리     | 박민     |
| [EffectManger](https://github.com/pm2979/Flareborn_Code/blob/main/Battle/BattleEffect/EffectManger.cs)             | 전투 중 이펙트 관리       | 박민    |
| [BattleEntities](https://github.com/pm2979/Flareborn_Code/blob/main/Battle/Entities/BattleEntities.cs)   | 배틀 유닛 관리 | 박민   |
| [EncounterSystem](https://github.com/pm2979/Flareborn_Code/blob/main/Battle/EncounterSystem.cs)                         | 전투 진입 관리       | 유도균    |

## 캐릭터

| 스크립트 경로                                                                                                                                     | 설명                 | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------ | ------ |
| [CharacterInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Character/CharacterInstance.cs)         | 캐릭터 정보 관리     | 박민     |
| [EquipmentController](https://github.com/pm2979/Flareborn_Code/blob/main/Character/Equip/EquipmentController.cs)             | 장비 장착 관리       | 박민    |
| [SkillInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Character/Skill/SkillInstance.cs)   | 스킬 정보 관리 | 박민    |
| [TraitController](https://github.com/pm2979/Flareborn_Code/blob/main/Character/Traits/TraitController.cs) | 특성 관리       | 박민     |

## 적

| 스크립트 경로                                                                                                                                     | 설명                 | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------ | ------ |
| [EnemyInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Enemy/EnemyInstance.cs)   | 적 정보 관리 | 박민, 유도균   |
| [BaseEnemyAI](https://github.com/pm2979/Flareborn_Code/blob/main/Enemy/Battle/EnemyAI/BaseEnemyAI.cs)         | 적의 배틀 상태 추상 클래스     | 박민     |
| [OverWorldEnemy](https://github.com/pm2979/Flareborn_Code/blob/main/Enemy/OverWorld/OverWorldEnemy.cs)             | 적의 필드 상태 관리       | 유도균    |

## 던전

| 스크립트 경로                                                                                                                                     | 설명                 | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------ | ------ |
| [DungeonInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Dungeon/DungeonInstance.cs)         | 던전 정보 관리     | 유도균     |
| [DungeonPortal](https://github.com/pm2979/Flareborn_Code/blob/main/Dungeon/DungeonPortal.cs)             | 던전 포탈 관리       | 유도균    |

## 아이템

| 스크립트 경로                                                                                                                                     | 설명                 | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------ | ------ |
| [ItemInstance](https://github.com/pm2979/Flareborn_Code/blob/main/Item/ItemInstance.cs)         | 아이템 정보 관리     | 박민     |
| [Inventory](https://github.com/pm2979/Flareborn_Code/blob/main/Item/Inventory/Inventory.cs)             | 인벤토리 관리       | 김석호    |
| [RuneCrafting](https://github.com/pm2979/Flareborn_Code/blob/main/Item/Rune/RuneCrafting.cs)   | 룬 생성 | 박민    |
| [CurrencyWallet](https://github.com/pm2979/Flareborn_Code/blob/main/Item/Wallet/CurrencyWallet.cs) | 화폐 관리       | 김석호     |

## NPC 및 퀘스트

| 스크립트 경로                                                                                                                                     | 설명                 | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------ | ------ |
| [NPC](https://github.com/pm2979/Flareborn_Code/blob/main/Overworld/NPC/NPC.cs)         | NPC 관리     | 이자연     |
| [Quest](https://github.com/pm2979/Flareborn_Code/blob/main/Overworld/QuestSystem/Quest.cs)             | 퀘스트 관리       | 이자연    |
| [TutorialManager](https://github.com/pm2979/Flareborn_Code/blob/main/Overworld/QuestSystem/Tutorial/TutorialManager.cs)   | 튜토리얼 관리 | 이자연    |
| [DialogueActivator](https://github.com/pm2979/Flareborn_Code/blob/main/Overworld/DialogueSystem/DialogueActivator.cs) | 다이얼로그 관리        | 이자연     |

## UI

| 스크립트 경로                                                                                                                                     | 설명                 | 주요 기여자 |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------ | ------ |
| [BattleUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/BattleUI.cs)         | 배틀UI     | 박민     |
| [OverworldUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/OverworldUI.cs)             | 필드UI 관리       | 박민    |
| [EquipmentUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/EquipmentUI/EquipmentUI.cs)   | 장비 장착UI | 박민    |
| [InventoryUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/Inventory/InventoryUI.cs) | 인벤토리UI       | 박민     |
| [StatusUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/StatUI/StatusUI.cs)                         | 스테이터스UI      | 박민    |
| [SoundSettings](https://github.com/pm2979/Flareborn_Code/blob/main/UI/SoundOptionUI/SoundSettings.cs)         | 사운드 관리UI        | 김석호    |
| [QuestLogUI](https://github.com/pm2979/Flareborn_Code/blob/main/UI/QuestUI/QuestLogUI.cs)                         | 퀘스트UI        | 이자연     |
