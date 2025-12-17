# Blade of the Ancients

> A 2D Top-down Action RPG developed with Unity and C#. This project focuses on implementing scalable game architecture using Singleton patterns and complex enemy behaviors using Finite State Machines (FSM).

---

## Project Overview

| Category | Details |
| :--- | :--- |
| **Engine** | Unity 2022.3 (LTS) |
| **Language** | C# |
| **Perspective** | Top-down 2D Pixel Art |
| **Input** | Keyboard & Mouse |

---

## Technical Highlights

### 1. Artificial Intelligence (Boss AI)
Implemented a modular AI system using **Finite State Machines (FSM)** to manage distinct boss phases and behaviors.

* **Mecha Golem (Complex FSM):**
    * **Hub-and-Spoke Architecture:** Uses `Idle` as the central decision node to transition between states.
    * **Phase 2 Logic:** Triggers an `Enrage` state at <50% HP, activating temporary **invulnerability** and increasing attack frequency.
    * **Predictive Aiming:** Calculates player velocity vectors to predict movement and fire projectiles at intercept points rather than current position.
    * **Attack Patterns:** Dynamic switching between melee rushing, shotgun-style spread shots, and precision laser attacks based on distance thresholds.

* **The Reaper (Summoning Logic):**
    * **Summoning Mechanics:** Spawns minion entities (Wisps/Skeletons) to alter combat pacing dynamically.
    * **Hybrid Combat:** Alternates between ranged projectile attacks and chasing mechanics based on player proximity.

### 2. System Architecture
* **Singleton Pattern:** Applied to core systems (`GameManager`, `SoundManager`, `PlayerHealth`) to ensure global access and prevent duplicate instances.
* **Scene Persistence:** Utilized `DontDestroyOnLoad` for preserving player state (Health, Inventory, Currency) across scene transitions.
* **UI Management:** Decoupled **Global UI** (Player HUD) from **Local UI** (Boss Health Bars) to prevent Canvas duplication errors during scene loading.

### 3. Combat System
* **DamageSource Component:** A flexible, inspector-based damage handler attached to weapons and projectiles, allowing for easy balancing of damage values without code modification.
* **Hitbox/Hurtbox Logic:** Precise collision detection using Unity's `Physics2D` and `LayerMasks` to differentiate between player, enemy, and environment interactions.

---

## Installation

1.  Clone the repository:
    ```bash
    https://github.com/huynh-dang187/BladeOfTheAncients-UnityRPG.git
    ```
2.  Open **Unity Hub** and add the project folder.
3.  Open the project using Unity version **2022.3** or later.
4.  Navigate to `Assets/Scenes` and open `MenuStart`.

---

## Assets and Credits

* **Art:** Pixel art assets adapted from itch.io packs (Educational use).
* **Audio:** Sound effects and music from Kenney Assets and Soul Knight OST (Educational use).
