# GameKit

모바일 게임 공통 기반 UPM 패키지 (`com.jiwon.gamekit`). Unity 6, Newtonsoft.Json 의존.

## 설치

`Packages/manifest.json`:

```json
"com.jiwon.gamekit": "https://github.com/jiwon000512/UnityGameKit.git"
```

개발 중 로컬 참조: `"com.jiwon.gamekit": "file:../../../UnityGameKit"` (Packages 폴더 기준 상대 경로).

## 모듈

| 어셈블리 | 클래스 | 용도 |
|---|---|---|
| GameKit.Unity | `MonoSingleton<T>` | Manager 베이스. 없으면 생성, 중복 파괴, 씬 유지 |
| GameKit.Unity | `EventManager` | 타입 키 발행·구독. `Subscribe<T>`가 돌려준 `IDisposable`로 해제 |
| GameKit.Unity | `PoolManager`, `PooledObject` | 프리팹 풀. `Get(prefab, parent)` / `Release(instance)` |
| GameKit.Unity | `TableManager` | `Resources/Data/{name}.json` 로드·캐시. `Load<TRow>(name)`, `LoadConfig<T>(name)` |
| GameKit.Core | `TableFile<TRow>` | 테이블 봉투 `{ table, version, notes, rows }` |
| GameKit.Unity | `DataManager` | `persistentDataPath/{key}.json` 저장·로드·삭제. 원자적 교체 |
| GameKit.Core | `SaveFile<T>` | 세이브 봉투 `{ Version, SavedAtUtc, Data }` |
| GameKit.Unity | `UIManager`, `UIView` | `Resources/UI/{TypeName}` 프리팹을 열고 닫는 스택. 루트 캔버스 자동 생성 |

## 사용

```csharp
IReadOnlyList<AnimalRecord> animals = TableManager.Instance.Load<AnimalRecord>("animals");

DataManager.Instance.Save("zoo", saveData, version: 1);
if (DataManager.Instance.TryLoad("zoo", out SaveFile<SaveData> file)) { /* file.Data, file.SavedAtUtc */ }

IDisposable token = EventManager.Instance.Subscribe<CoinsChanged>(Wallet_CoinsChanged);
EventManager.Instance.Publish(new CoinsChanged(120));
token.Dispose();

GameObject visitor = PoolManager.Instance.Get(visitorPrefab, parent);
PoolManager.Instance.Release(visitor);

GachaPopupView popup = UIManager.Instance.Open<GachaPopupView>(args);
UIManager.Instance.Close(popup);
```

## 규약

코드 규칙·프로그래밍 규약은 동물원 타이쿤 저장소(jiwon000512/IdleTycoon)의 `기획/코드-규칙.md`, `기획/프로그래밍-규약.md`를 따른다. 주석·로그 최소화, 핵심 기능만.
