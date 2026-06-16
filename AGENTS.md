# AGENTS.md - ClickerGameCaterpillars

## Project Overview

A Unity 6 (6000.3.10f1) clicker/idle game written in C# where a caterpillar evolves through touch interactions. Korean-language game content. Uses a custom DI container, ScriptableObject-driven data, and an NPOI-based Excel data pipeline.

## Build & Run Commands

This is a Unity project — there is no CLI build/test pipeline. Everything runs inside the Unity Editor.

- **Open project**: Launch Unity Hub → Open `ClickerGameCaterpillars/` folder
- **Main scene**: `Assets/Scenes/GamePlayScene.unity`
- **Play in Editor**: Press Play in Unity Editor

### Testing

Tests are MonoBehaviour-based scripts (NOT NUnit). They run in-scene and display results via `OnGUI()`.

- **Test scripts location**: `Assets/Scripts/Tests/` — `Phase1Test.cs`, `Phase2Test.cs`, `Phase3Test.cs`, `TouchFunctionTester.cs`
- **Running a single test**: Attach the desired test MonoBehaviour to a GameObject in the scene, enter Play mode, and click "Run Tests Again" in the on-screen GUI
- **There is no CLI test runner** — tests require the Unity runtime

### Editor Tools

Accessible via Unity menu: `Tools > Game` and `Tools > Data`. These are defined in `Assets/Editor/` scripts.

Key tools:
- `Tools > Game > Force Recreate All ScriptableObjects` — regenerates SO assets
- `Tools > Data > Convert Excel to ScriptableObject` — runs Excel → SO pipeline
- `Tools > Data > Validate Excel Data` — validates Excel data

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/           # DI container, data initializer, data manager
│   ├── Data/           # Data models, Excel service/converter, interfaces
│   │   └── Models/     # DataModels.cs, ScriptableObjectModels.cs
│   ├── Gameplay/       # Game logic (click, score, evolution, touch functions)
│   │   └── TouchFunction/  # ITouchFunction, factory, concrete implementations
│   ├── Managers/       # GameplayManager, AudioManager
│   ├── UI/             # UIManager, panels, list views
│   ├── Tests/          # In-scene MonoBehaviour test scripts
│   └── Utilities/      # (empty, reserved)
├── Editor/             # Unity Editor tools and setup scripts
├── Prefabs/            # Unity prefab assets
├── Resources/          # ScriptableObject assets loaded at runtime
├── ExcelData/          # Source Excel data files
├── Scenes/             # Unity scene files
└── Plugins/            # DLLs (NPOI, MathNet, BouncyCastle, etc.)
```

## Code Style Guidelines

### Namespaces

Every file must declare a namespace matching its folder location:

| Folder                    | Namespace                            |
|---------------------------|--------------------------------------|
| `Scripts/Core/`           | `ClickerGame.Core`                   |
| `Scripts/Data/`           | `ClickerGame.Data`                   |
| `Scripts/Data/Models/`    | `ClickerGame.Data.Models`            |
| `Scripts/Gameplay/`       | `ClickerGame.Gameplay`               |
| `Scripts/Gameplay/TouchFunction/` | `ClickerGame.Gameplay.TouchFunction` |
| `Scripts/UI/`             | `ClickerGame.UI`                     |
| `Scripts/Managers/`       | `ClickerGame.Core`                   |
| `Scripts/Tests/`          | `ClickerGame.Tests`                  |
| `Editor/`                 | `ClickerGame.EditorTools`            |

### Using Statements

Order: `System` namespaces first, then `UnityEngine`, then project namespaces. Example:

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ClickerGame.Data.Models;
```

### Braces & Formatting

- **Allman brace style** — opening brace on its own line
- **4-space indentation**
- Blank line between methods

### Naming Conventions

| Element                  | Convention                  | Example                          |
|--------------------------|-----------------------------|----------------------------------|
| Classes                  | PascalCase                  | `TouchCounter`                   |
| Interfaces               | IPascalCase                 | `ITouchFunction`                 |
| Private fields           | _camelCase                  | `_currentScore`, `_touchCounter` |
| Public fields (Inspector)| camelCase (no prefix)       | `allFunctions`, `bonusTriggerCount` |
| Properties               | PascalCase                  | `CurrentScore`, `Instance`       |
| Methods                  | PascalCase                  | `AddTouch()`, `HandleClick()`    |
| ScriptableObject classes | PascalCase + `SO` suffix    | `EvolutionStageListSO`           |
| Parameters               | camelCase                   | `touchCount`, `multiplier`       |
| Constants                | PascalCase or camelCase     | `ExcelDataPath`                  |
| Structs                  | PascalCase                  | `TouchEffect`                    |
| Data models              | PascalCase + `DataModel`    | `EvolutionStageDataModel`        |

### MonoBehaviour Patterns

**Inspector fields** — use `[SerializeField] private` with `[Header]` groups:

```csharp
[Header("Settings")]
[SerializeField] private int bonusTriggerCount = 50;

[Header("Events")]
public UnityEvent<int> OnTouchCountChanged;
```

**Singleton pattern** — used for managers. Follow this exact pattern:

```csharp
public static ManagerName Instance { get; private set; }

private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);
}

private void OnDestroy()
{
    if (Instance == this)
    {
        Instance = null;
    }
}
```

**UnityEvent initialization** — always null-check and create in `Awake()`:

```csharp
if (OnScoreChanged == null)
    OnScoreChanged = new UnityEvent<int>();
```

### Debug Logging

Use tagged log messages with `[ClassName]` prefix:

```csharp
Debug.Log($"[TouchCounter] Subtracted {amount}. Total: {_totalTouchCount}");
Debug.LogWarning("[TouchFunctionList] TouchFunctionManager not found!");
Debug.LogError($"[UIManager] SettingManager NOT FOUND!");
```

### Error Handling

- Use `Debug.LogError` for failures, `Debug.LogWarning` for recoverable issues
- Wrap editor operations in try-catch with user-facing `EditorUtility.DisplayDialog` for errors
- For runtime code, prefer null-checks with early return over exceptions
- Use `TryResolve<T>` (returns bool) over `Resolve<T>` (throws) when service may not exist

### Design Patterns Used

- **DI Container**: `GameContainer.Instance` — Register/Resolve/TryResolve services
- **Factory Pattern**: `TouchFunctionFactory.Create()` creates `ITouchFunction` instances from data
- **Strategy Pattern**: `ITouchFunction` implementations (`CriticalTouchFunction`, `SpeedBoostFunction`, etc.)
- **Singleton**: Manager MonoBehaviours (`GameplayManager`, `UIManager`, `TouchFunctionListManager`)
- **ScriptableObject**: Data storage (`EvolutionStageListSO`, `TouchFunctionListSO`) loaded from `Resources/`

### Data Pipeline

1. Excel files in `Assets/ExcelData/` are read via NPOI
2. `ExcelConverter` / `ExcelToScriptableObjectConverter` convert rows to ScriptableObjects
3. ScriptableObjects are stored in `Assets/Resources/`
4. Runtime loads them via `Resources.Load<T>(path)`

### C# Language Features

- **Expression-bodied members** for simple properties: `public int CurrentScore => _currentScore;`
- **Pattern matching** with `switch` expressions: `data.FunctionType switch { "Bonus" => ... }`
- **String interpolation** with `$"..."` for all log messages
- **Null-conditional operator**: `OnScoreChanged?.Invoke(value)`
- **Target-typed `new()`**: `new()` for collection initialization (e.g., `new List<T>()`, `new()`)
- `[Serializable]` attribute on all data model classes

### Important Rules

- **Never modify `.meta` files by hand** — Unity manages them
- **Korean text** is used for all user-facing game content (names, descriptions, UI labels)
- **Do not add comments** unless explicitly requested
- **Resources.Load** is the standard way to load assets at runtime — assets must be in `Assets/Resources/`
- **FindFirstObjectByType** (Unity 6 API) is used instead of the deprecated `FindObjectOfType`
- Event listeners added in `Awake()`/`SetupButtons()` must be removed in `OnDestroy()`
