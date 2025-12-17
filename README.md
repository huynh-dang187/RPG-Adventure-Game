Chào bro. Để trông "không AI" và giống dân kỹ thuật chuyên nghiệp (Senior/Mid-level), nguyên tắc là: Khô khan, tập trung vào kỹ thuật, không dùng tính từ sáo rỗng (amazing, stunning), không icon màu mè.

Dưới đây là bản README.md chuẩn chỉ, tập trung vào Architecture (Kiến trúc) và Logic (AI), đúng những gì bạn đã làm. Bạn chỉ cần Copy-Paste vào file README.md trên GitHub.

Blade of the Ancients
A 2D Top-down Action RPG developed with Unity and C#. This project focuses on implementing scalable game architecture using Singleton patterns and complex enemy behaviors using Finite State Machines (FSM).

Project Overview
Engine: Unity 2022.3 (LTS)

Language: C#

Perspective: Top-down 2D Pixel Art

Input: Keyboard & Mouse

Technical Highlights
1. Artificial Intelligence (Boss AI)
Implemented a modular AI system using Finite State Machines (FSM) to manage distinct boss phases and behaviors.

Mecha Golem (Complex FSM):

Uses a Hub-and-Spoke state architecture with Idle as the central decision node.

Phase 2 Logic: Triggers an Enrage state at <50% HP, activating temporary invulnerability (immune to damage) and increasing attack frequency.

Predictive Aiming: Calculates player velocity vectors to predict movement and fire projectiles at intercept points rather than current position.

Attack Patterns: Includes melee rushing, shotgun-style spread shots, and precision laser attacks based on distance thresholds.

The Reaper (Summoning Logic):

Implements a summoning state that spawns minion entities (Wisps/Skeletons) to alter combat pacing.

Alternates between ranged projectile attacks and chasing mechanics.

2. System Architecture
Singleton Pattern: Applied to core systems (GameManager, SoundManager, PlayerHealth) to ensure global access and prevent duplicate instances.

Scene Persistence: Utilized DontDestroyOnLoad for preserving player state (Health, Inventory, Currency) across scene transitions.

UI Management: Decoupled Global UI (Player HUD) from Local UI (Boss Health Bars) to prevent Canvas duplication errors during scene loading.

3. Combat System
DamageSource Component: A flexible, inspector-based damage handler attached to weapons and projectiles, allowing for easy balancing of damage values without code modification.

Hitbox/Hurtbox Logic: Precise collision detection using Unity's Physics2D and LayerMasks to differentiate between player, enemy, and environment interactions.

Installation
Clone the repository:

Bash

git clone https://github.com/yourusername/BladeOfTheAncients-UnityRPG.git
Open Unity Hub and add the project folder.

Open the project using Unity version 2022.3 or later.

Navigate to Assets/Scenes and open MenuStart.

Assets and Credits
Art: Pixel art assets adapted from [Source Name, e.g., itch.io packs].

Audio: Sound effects and music from Kenney Assets and Soul Knight OST (Educational use).
