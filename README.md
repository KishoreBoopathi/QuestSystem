# Quest System
**Unity · C# · Event-Driven Architecture**

A lightweight, event-driven quest framework supporting multiple objective types, optional failure timers, and reward dispatch — built around a decoupled observer pattern so gameplay systems never directly reference the quest manager.

---

## Overview

Quests are defined through a fluent factory API and tracked via a central event bus. Gameplay systems fire named events; the quest system listens and evaluates progress. No direct coupling between quest logic and the systems that trigger it.

---

## Architecture

```
GameEvents (Singleton — Event Bus)
└── Dictionary<string, UnityEvent<int>>
    ├── TriggerEvent(key, value)
    ├── StartListening(key, callback)
    └── StopListening(key, callback)

Quest (MonoBehaviour)
├── QuestObjective (struct)
│   ├── Type: Kill / Pickup
│   ├── Required / Acquired counts
│   └── IsCompleted flag
├── CreateQuest() — factory overloads for fluent setup
├── CheckObjectiveCompleted() — increments acquired, validates
├── CheckQuestSucceeded() — fires success/failure by name
└── Optional failure timer via coroutine

QuestLists (MonoBehaviour)
└── UI list + sample quest data
```

---

## Key Design Decisions

**Event bus over direct references** — `GameEvents` holds a `Dictionary<string, UnityEvent<int>>` as a central pub/sub hub. Any system can fire an event by key; quests subscribe only to what they need. Adding a new quest never requires touching gameplay code.

**Factory pattern for quest creation** — `Quest.CreateQuest()` overloads allow fluent, readable quest definitions. Quest data is assembled at runtime without ScriptableObject dependencies, keeping the system portable.

**Enum-typed objectives** — `Kill` and `Pickup` objective types are handled through a shared `CheckObjectiveCompleted()` path, making it straightforward to extend with new types.

**Optional failure timer** — Timed quests run a coroutine that fires a failure event if the objective isn't met within the window. Success and failure are both dispatched by name through the event bus, keeping reward/penalty logic external.

---

## Extending the System

To add a new objective type:
1. Add entry to the `ObjectiveType` enum
2. Handle it in `CheckObjectiveCompleted()`
3. Fire the corresponding event key from the relevant gameplay system

No changes needed to `GameEvents`, `QuestLists`, or any other quest instance.

---

## What This Demonstrates

- Observer pattern via a generic event bus
- Factory/builder pattern for runtime object configuration
- Decoupled architecture — quests and gameplay systems share no direct references
- Coroutine-based timer management

---

## Tech

- Unity 2020.3 · C#
- Google Play Games Services (GPGS) for authentication
- No additional third-party dependencies
