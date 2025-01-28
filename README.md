
# Pacman Clone V 1.3

[**[Play Game!!!]**](https://trymorez.github.io/unity-retro-game-series-02/Build/index.html)

For the next project, I have chosen Pac-Man as the sequel to the retro game.

One of the most distinctive features of Pac-Man is the need to implement custom AI. If I use Unity's default pathfinding AI, NavMesh, the ghosts will always move in the shortest path and continuously adjust their routes, making it difficult for the player to survive. Additionally, during the research phase, I found that each ghost has its own personality and operates with different custom AIs. As I progress with the project, I plan to implement at least two types of AI if possible.

I intend to implement the main game routine using a state machine. Using a state machine allows for independent management of each state, making maintenance easier and enhancing scalability.

In my previous project, Space Invaders, I used coroutines as the main routine and controlled the progression logic with several boolean variables. Initially, I didn't notice any issues, but as the project progressed, I felt overwhelmed trying to add and manage flow. I believe that using coroutines and variables for the main routine can become problematic in large-scale projects.

Just as important as the ghosts' movement AI is the implementation of their behavior patterns. The ghosts in Pac-Man move with completely different and unique behavior patterns based on various AI states (idle, chase, scatter, frightened, and eye-only). Implementing these behaviors as state patterns will make maintenance easier, enhance scalability, and provide a basic framework for AI agents in future games.

- Recommended Unity version: Unity 6 (6000.0.32f1 and later)

## Screenshots

![screenshot](Assets/Screenshot/screenshot-V1.1(1).png)
![screenshot](Assets/Screenshot/screenshot-V1.1(2).png)
                                                             

## Version History
**V 1.3 - (2025-01-28)**
- Increased the number of intersection nodes to make Pac-Man's control smoother

**V 1.2 - (2025-01-27)**
- Level progression has been implemented

**V 1.1 - (2025-01-27)**
- Display Pac-Man's lives (default is 3)
- Now Pac-Man can die if caught by a ghost

**V 1.0 - (2025-01-26)**
- Display points when Pac-Man eats ghosts

**V 0.9 - (2025-01-25)**
- Added background sound effect
- Modified to prevent eating the same ghost multiple times
- Displayed a READY message when starting a level
- Ghosts can exit the home even when they are in a blue state

**V 0.8 - (2025-01-24)**
- Started testing pacman eating ghosts routine

**V 0.7 - (2025-01-23)**
- Started testing power beans routine

**V 0.6  - (2025-01-22)**
- Score system has been implemented
- Pacman can now eat beans

**V 0.5  - (2025-01-22)**
- Pacman movement routine implemented
- Ghost chasing routine implemented

**V 0.45  - (2025-01-21)**
- Started testing the movement of Pacman

**V 0.4  - (2025-01-20)**
- Ghost scattering logic has been implemented. The ghosts will now move to predetermined positions using custom AI

**V 0.3  - (2025-01-20)**
- The ghosts can now exit from home one by one, starting with the center ghost, followed by the left ghost, and then the right ghost

**V 0.2  - (2025-01-19)**
- Began testing the basic movement and wall detection of the ghost in the ready state

**V 0.15  - (2025-01-18)**
- Stage editing is nearly complete
- Currently structuring the main game routine
- Wrote a superclass for the singleton

**V 0.1 - (2025-01-17)**
- Project setup completed
- Level constructed with walls and obstacles (no code written yet)
