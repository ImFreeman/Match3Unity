# Match3Unity

A Match-3 puzzle game built with Unity, demonstrating production-oriented architecture patterns: async command pipeline, MVP UI, object pooling, and interface-driven design.

---

## Gameplay

- Click on a group of **3 or more** adjacent tiles of the same type to remove them
- Tiles fall down to fill gaps; new tiles spawn from the top
- Matching **4 or more** tiles grants bonus moves
- The game ends when no valid moves remain
- High scores are saved to a persistent leaderboard

**Tile types:** Apple, Banana, Blueberry, Grape, Orange, Pear, Strawberry

---

## Tech Stack

| Tool | Purpose |
|---|---|
| Unity 2022+ | Engine |
| C# | Language |
| UniTask | Async/await without coroutines |
| DOTween | Tile and UI animations |
| TextMesh Pro | Text rendering |
| Unity Input System | Input handling |
| Newtonsoft JSON | Data serialization |
| PlayerPrefs | Score persistence |

---

## Architecture Overview

The project is split into feature modules under `Assets/Features/`. Each module has an `Interfaces/` and `Realization/` subfolder enforcing dependency inversion.

```
Assets/Features/
├── Core/               # Infrastructure: DI, command pipeline, scene management
├── GameLogic/          # Match-3 rules: matching, resolving, scoring, moves
├── Tile/               # Tile domain: models, storage, generation, animation
├── UI/                 # Screens and presenters (MVP)
└── SaveSystem/         # Leaderboard persistence
```

---

## Core Systems

### 1. Command Pipeline (`Core/`)

Application startup is modeled as a sequential async pipeline of commands.

```
ApplicationLauncher.Start()
    └── CommandBootstrapper.Execute(commands[])
            ├── InitTileSystemsCommand
            ├── InitGameWindowCommand
            ├── InitGameLogicCommand
            ├── StartGameSceneCommand
            └── TilesFlickeringCommand
```

Each command implements `ICommand`:

```csharp
public interface ICommand : IDisposable
{
    UniTask<CommandResult> Do();
    void Cancel();
}
```

Commands are composable — `LeaveGameSceneCommand` wraps `SaveRecordsCommand` and `ClearGameSceneCommand`. Each scene has its own launcher with its own command chain.

---

### 2. Service Locator (`Core/ServiceLocator/`)

Services are registered at startup and resolved by type throughout the app:

```csharp
ServiceLocator.Register<IMatchFinder>(new MatchFinder(...));
var finder = ServiceLocator.Get<IMatchFinder>();
```

All services implement `IDisposable`. Scene teardown calls `ServiceLocator.Clear()` to dispose and unregister everything.

---

### 3. Game Logic (`GameLogic/`)

The match-3 game loop is orchestrated by `GameField` through four collaborators:

```
User click
    └── TileClickHandler
            ├── GameField.CheckTile()      → MatchFinder (DFS algorithm)
            ├── GameField.ResolveTiles()   → MatchResolver → TileResolver
            │                                  (score, moves, despawn)
            └── GameField.UpdateTiles()    → TileShifter (gravity + DOTween)
                                           → TileGenerator (new tiles)
                                           → LayoutUpdated event
                                                └── GameFlow (end-game check)
```

**MatchFinder** uses recursive depth-first search across the tile grid to find all connected tiles of the same type. Minimum match size is configurable.

**MatchResolver** applies move logic: a match of exactly 3 costs 1 move; larger matches award bonus moves via a configurable threshold table.

**GameFlow** subscribes to `LayoutUpdated` and checks for end-game conditions after every turn: out of moves, or no valid matches remaining anywhere on the board.

---

### 4. Tile System (`Tile/`)

Tiles have a clean separation between model and view:

| Layer | Class | Responsibility |
|---|---|---|
| Model | `TileModel` | Type (enum), resolver reference |
| Storage | `TileStorage<T>` | Dictionary by ID, `TileAdded`/`TileRemoved` events |
| Layout | `TileLayout` | 2D jagged array `int[][]` (column-major) |
| Pool | `TilePool` | Reuse `TileModel` instances |
| Generator | `TileGenerator<T>` | Random generation, initial layout fill |
| Shifter | `TileShifter` | Animated tile gravity via DOTween |

IDs bridge the model and view layers: `TileStorage` fires events with an integer ID; `GameWindowPresentor` listens and manages the corresponding UI element.

---

### 5. UI Layer — MVP (`UI/`)

Each screen follows the Model-View-Presenter pattern:

- **View** (`UIGameWindow`, `UIMainMenuWindow`, …) — MonoBehaviour, exposes UI elements and input events
- **Presenter** (`GameWindowPresentor`, `PauseWindowPresentor`, …) — plain C# class, subscribes to model events and updates the view
- **Model** — `IScoreHandler`, `IMovesHandler`, `ITileStorage` — game domain interfaces

`UIService` manages window lifecycle with object pooling: inactive windows are parked in a hidden pool container and reactivated on demand.

---

### 6. Save System (`SaveSystem/`)

`RecordsTrack` stores a list of `RecordData` (score + timestamp). Persistence is handled through PlayerPrefs via `SaveRecordsCommand` and `LoadRecordsFromPlayerPrefs`. The records scene highlights newly set records with a configurable color.

---

## Scenes

| Scene | Entry Command Chain |
|---|---|
| `MainScene` | InitUIServiceCommand → InitMainMenuCommand |
| `GameScene` | InitTileSystemsCommand → InitGameWindowCommand → InitGameLogicCommand → StartGameSceneCommand |
| `RecordsScene` | InitRecordsSystemCommand → InitRecordsWindowCommand |
| `AboutScene` | AboutSceneLaunchCommand |

---

## Key Design Decisions

**Jagged array for tile layout** — `int[][]` instead of `Dictionary<Vector2Int, int>` for cache-friendly random access during match search.

**UniTask over coroutines** — better composability, cancellation support via `CancellationTokenSource`, and cleaner async/await syntax.

**Interface-first modules** — every module exposes only interfaces to consumers. Concrete types are wired in init commands, keeping cross-feature coupling at the interface level.

**Object pooling at two levels** — `TilePool` reuses tile models; `UIService` reuses window GameObjects. Reduces GC pressure during gameplay.

**Event-driven UI updates** — `ScoreHandler` and `MovesHandler` raise C# events; the presenter updates text fields reactively without polling.

---

## Project Structure

```
Assets/
├── Features/
│   ├── Core/
│   │   ├── ApplicationLauncher/      # Scene launchers (GameSceneLauncher, etc.)
│   │   ├── Bootstrapper/             # CommandBootstrapper
│   │   ├── Command/                  # ICommand + all command implementations
│   │   └── ServiceLocator/           # ServiceLocator static registry
│   ├── GameLogic/
│   │   ├── Scripts/Interfaces/       # IGameField, IMatchFinder, IMatchResolver, ...
│   │   └── Scripts/Realization/      # GameField, GameFlow, MatchFinder, ...
│   ├── Tile/
│   │   ├── Scripts/Interfaces/       # ITileStorage, ITileLayout, ITileShifter, ...
│   │   └── Scripts/Realization/      # TileModel, TilePool, TileShifter, ...
│   ├── UI/
│   │   ├── Scripts/                  # UIService, UIWindow base class
│   │   ├── GameWindow/               # In-game HUD
│   │   ├── MainWindow/               # Main menu
│   │   ├── PauseWindow/              # Pause overlay
│   │   ├── RecordsWindow/            # Leaderboard
│   │   ├── GameOverPopUp/            # Game over dialog
│   │   └── UIGraphicElement/         # Reusable image element + pool
│   └── SaveSystem/
│       ├── Scripts/Interfaces/       # IRecordsTrack
│       └── Scripts/Realization/      # RecordsTrack, CsvDataLoader
├── Scenes/                           # MainScene, GameScene, RecordsScene, AboutScene
├── Resources/                        # UI prefabs loaded at runtime
└── Plugins/
    └── Demigiant/DOTween/
```


# Техническое задание

Должны быть реализованы 4 сцены

## 1. Сцена “Главный экран”

Главный экран должен содержать меню, состоящее из следующих пунктов

- **а) Новая игра**  
  переходит на вторую сцену (с геймплеем)
- **б) Таблица рекордов**  
  переходит на третью сцену (с рекордами)
- **в) О программе**  
  переходит на четвёртую сцену (с описанием)
- **г) Выход**  
  Выводит подтверждающее окно с кнопками “Выйти” и “Остаться”.  
  При нажатии на кнопку “Выйти” приложение закрывается.  
  При нажатии на кнопку “Остаться” окно закрывается.

## 2. Сцена “Геймплей”

Сцена должна содержать

### а) Игровое поле

**Начало игры**  
В начале игры должна быть показана механика игры.  
Подсветить первый ход.

**Игровой процесс**  
Тач на “шарик” он взрывается. Списывается один ход. Если Совпадает три одинаковых шарика они взрываются, начисляется 2 хода. Если взрываются 4 шарика начисляется 3 хода, 5-4. Вышестоящие шарики сдвигаются вниз. Шарики считаются совпавшими, если имеют тот же цвет и находятся по соседству с активным шариком или прочими совпавшими шариками в любом из 8 направлений.

**Окончание игры**  
Игра завершается если нет возможных ходов или закончилось количество ходов  
Проверяется количество набранных очков.  
Если игрок попадает в таблицу рекордов, то  
выполняется переход на третью сцену с выделением внесённой строки.  
Если не набрано необходимое количество очков, то выдается грустное сообщение с кнопкой “ОК” при нажатии на которую выполняется переход на первую сцену.

### б) кнопку “в меню”. При нажатии:
  Игра переходит в режим паузы  
  Выводится окно с предложением выйти в основное меню  
  и кнопками “В меню” и “Остаться”  
  При нажатии на кнопку “В меню” выполняется переход на первую сцену.  
  При нажатии на кнопку “Остатсья” игра оживает

### в) набранное количество очков

### г) Оставшееся количество ходов

## 3. Сцена “Таблица рекордов”

Содержит  
Список игр с датами в порядке убывания количества очков.  
Кнопку “в меню” при нажатии выполняется переход на первую сцену  
При первом запуске программы таблица рекордов заполняется из csv-файла

## 4. Сцена “О программе”

Сцена должна содержать краткое описание программы и краткое руководство к игре. А также активную ссылку на профиль разработчика в одной из социальных сетей.

## Дополнительные требования

- Unity3D 2021 версии. Одной из последних ревизий.
- Язык программирования C#
- Одна из парадигм паттернового программирования использующихся в создании игр. Рекомендуется к прочтению “Game Programming Patterns”
  - Оригинал: http://gameprogrammingpatterns.com/
  - Перевод: http://live13.livejournal.com/462582.html
- Сцены unity
- Префабы - для игровых объектов
- UserPrefs - для хранения данных таблицы рекордов
- Простейшая анимация взрыва шарика (возможна любая другая, то как уменьшение, вылет итд)
- Асинхронные методы работы с объектами


- Простейшая оптимизация работы с игровыми объектами
- Один из твинеров для перемещения и оживления объектов (например DOTween)
- Настоятельно не рекомендуется использовать фреймворки, инъекторы, контейнеры и т.п. (Odin, ZenJect, vContainer … )
- не использовать физику юнити и рейкасты
