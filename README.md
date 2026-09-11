# 🎮 Frenzy Tanks — Roguelike Tank Shooter

> 🚧 **Work in Progress**
>
> Frenzy Tanks is currently under active development. Core gameplay and procedural generation systems are being implemented and refined before the project reaches a public playable release.

**Frenzy Tanks** is a **roguelike tank shooter** developed in **Unity (C#)**, focused on procedural generation, modular gameplay systems, enemy AI and backend integration.

The player navigates through a **procedurally generated node-based map**, choosing their path between encounters while fighting enemies, collecting upgrades and progressing through increasingly challenging rooms.

This project is part of my **Game Developer portfolio**, with a strong focus on gameplay programming, system architecture and procedural content generation.

---

## 🎮 Gameplay

Each run generates a different map, forcing the player to make decisions about which encounters to take and how to progress through the level.

The intended gameplay loop is:

**Explore → Choose an Encounter → Fight → Upgrade → Choose Your Next Path**

### Core Features

* 🗺️ **Procedurally generated node map**
* 🌍 **Procedurally generated combat arenas**
* 🤖 **Modular enemy AI using Finite State Machines**
* 🔫 **Tank movement, aiming and shooting**
* 💥 **Combat and damage systems**
* ⚡ **Upgrade and progression systems**
* 🎲 **Different encounter types**
* 🌲 **Procedurally placed environment and gameplay objects**
* 🧩 **Data-driven systems using ScriptableObjects**
* 🌐 **Backend integration through a custom REST API**

---

## 🧠 Technical Highlights

### 🗺️ Procedural Map Generation

The game uses a **layer-based node map** where each node represents a possible encounter.

The generator handles:

* Variable number of nodes per layer
* Parent/child relationships between nodes
* Multiple possible paths through a run
* Encounter type generation based on map progress
* Guaranteed connectivity between layers
* Configurable encounter probabilities
* Map visualization and debugging tools

Encounter generation is controlled through **ScriptableObjects**, allowing generation rules to be configured without modifying the generator itself.

The system is being designed to support increasingly complex map layouts while maintaining valid paths through the run.

---

### 🌍 Procedural Room Generation

Combat rooms are generated dynamically using procedural techniques to create different layouts while maintaining playable spaces.

The generation system can handle:

* Terrain generation
* Environmental decoration
* Walls and obstacles
* Enemy spawn positions
* Patrol points
* Object placement
* Different environment sizes and configurations

Gameplay-critical elements and visual decoration are generated as separate concerns, allowing room generation to be tuned without tightly coupling gameplay logic to visual elements.

---

### 🤖 Enemy AI

Enemies use a modular **Finite State Machine (FSM)** architecture.

Individual behaviours can be combined into different enemy configurations instead of implementing a separate AI system for every enemy.

This allows behaviours such as:

* Patrolling
* Detecting the player
* Chasing
* Attacking
* Searching
* Returning to previous states

The architecture is designed to make adding new behaviours and enemy types easier as the project grows.

---

### 🧩 Data-Driven Design

Several gameplay systems use **ScriptableObjects** to keep configuration separate from runtime logic.

This is used for systems such as:

* Encounter generation rules
* Room configuration
* Enemy configuration
* Environment settings
* Gameplay parameters

This approach allows gameplay values and generation rules to be tuned directly from the Unity Editor while keeping the underlying systems reusable.

---

## 🌐 Backend Integration

Frenzy Tanks is integrated with a separate backend service, [TanksAPI](https://github.com/Kaizzendev/TanksAPI), built with **`ASP.NET Core`**.

The API provides the foundation for persistent player data and future online functionality.

### Current Integration

The Unity client communicates with the API through HTTP requests for functionality such as:

* 👤 **User registration and authentication**
* 🔐 **JWT-based authentication**
* 💾 **Player save data**
* 📊 **Gameplay statistics**
* 🏆 **Leaderboard data**

The backend is kept as a separate project from the Unity client, allowing the game and API to evolve independently.

### Architecture

```text
┌─────────────────────┐
│    Unity Client     │
│       C# / Unity    │
└──────────┬──────────┘
           │
           │ HTTP / REST
           ▼
┌─────────────────────┐
│      TanksAPI       │
│   ASP.NET Core      │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│      Database       │
│   Persistent Data   │
└─────────────────────┘
```

The API is being developed as part of the same portfolio project to demonstrate experience building **client-server architectures** and integrating backend services into a game.

---

## 🛠️ Technologies

| Technology            | Usage                            |
| --------------------- | -------------------------------- |
| **Unity 6**           | Game engine                      |
| **C#**                | Gameplay and systems programming |
| **`ASP.NET Core`**      | Backend API                      |
| **REST API**          | Client-server communication      |
| **JWT**               | Authentication                   |
| **ScriptableObjects** | Data-driven game configuration   |
| **Cinemachine**       | Camera system                    |
| **Git / GitHub**      | Version control                  |

### Programming Concepts

* Object-Oriented Programming
* Finite State Machines
* Procedural Generation
* Event-Driven Architecture
* Data-Driven Design
* Modular Systems
* REST API Integration
* Client-Server Architecture
* Randomized Content Generation

---

## 📸 Screenshots & Gameplay

> **Visuals will be added as development progresses.**
>
> The current repository contains systems and prototypes that are still undergoing development. Screenshots and gameplay footage will be updated once the current procedural generation and gameplay systems reach a more representative state.

<!--
### Gameplay

![Gameplay](screenshots/...)

### Procedural Map

![Procedural Map](screenshots/...)

### Procedural Generation

![Procedural Generation](screenshots/...)
-->

---

## 🚧 Development Status

Frenzy Tanks is currently in **active development**.

The main focus is currently on building the underlying gameplay and procedural systems before moving into final content, presentation and polish.

### Current Focus

* 🗺️ Expanding the procedural map generation system
* 🌍 Improving procedural room generation
* 🎲 Expanding encounter types and generation rules
* 🤖 Developing additional enemy behaviours
* ⚡ Building the progression and upgrade systems
* 🌐 Expanding Unity ↔ API integration

### Upcoming

* [ ] More encounter types
* [ ] Expanded upgrade system
* [ ] Improved procedural path generation
* [ ] More enemy behaviours and variants
* [ ] Improved room generation and balancing
* [ ] Object pooling for projectiles and effects
* [ ] Complete progression system
* [ ] Main menu and pause/options screens
* [ ] Audio and sound effects
* [ ] Particle effects and visual polish
* [ ] Additional environments and encounters
* [ ] Public playable demo

---

## ⚙️ Getting Started

### Requirements

* **Unity 6000.3.12f1**
* Git

### Installation

Clone the repository:

```bash
git clone https://github.com/Kaizzendev/Tanks.git
```

Open the project using **Unity 6000.3.12f1**.

Load the main game scene:

```text
Game.unity
```

Press **Play** in the Unity Editor.

> ⚠️ The project is currently under development, so some systems may be incomplete or subject to change.

---

## 💡 What I Learned

This project focuses on solving **systemic gameplay and procedural generation problems** rather than building a single linear level.

Some of the main challenges have been:

* Designing a procedural map while guaranteeing that every node remains reachable.
* Generating multiple possible paths without creating disconnected sections.
* Separating procedural gameplay generation from environmental decoration.
* Creating configurable generation rules without tightly coupling them to the generator.
* Designing enemy AI that can be extended without rewriting existing behaviours.
* Building reusable systems that can support different encounter types.
* Integrating a Unity client with a separate backend service.
* Designing persistent player data and authentication between the game and API.

The project is still evolving, so these systems are being continuously refactored and expanded as new gameplay requirements emerge.

---

## 📎 Related Projects

### 🌐 TanksAPI

The backend service used by Frenzy Tanks.

**`ASP.NET Core` REST API** providing authentication, persistent player data, statistics and leaderboard functionality.

[View TanksAPI →](https://github.com/Kaizzendev/TanksAPI)

---

## 📎 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.
