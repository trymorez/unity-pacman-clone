
# Pacman Clone V 0.1

For the next project, I have chosen Pac-Man as the sequel to the retro game.

One of the most distinctive features of Pac-Man is the need to implement custom AI. If I use Unity's default pathfinding AI, NavMesh, the ghosts will always move in the shortest path and continuously adjust their routes, making it difficult for the player to survive. Additionally, during the research phase, I found that each ghost has its own personality and operates with different custom AIs. As I progress with the project, I plan to implement at least two types of AI if possible.

I intend to implement the main game routine using a state machine. Using a state machine allows for independent management of each state, making maintenance easier and enhancing scalability.

In my previous project, Space Invaders, I used coroutines as the main routine and controlled the progression logic with several boolean variables. Initially, I didn't notice any issues, but as the project progressed, I felt overwhelmed trying to add and manage flow. I believe that using coroutines and variables for the main routine can become problematic in large-scale projects.

Just as important as the ghosts' movement AI is the implementation of their behavior patterns. The ghosts in Pac-Man move with completely different and unique behavior patterns based on various AI states (idle, chase, scatter, frightened, and eye-only). Implementing these behaviors as state patterns will make maintenance easier, enhance scalability, and provide a basic framework for AI agents in future games.

- Recommended Unity version: Unity 6 (6000.0.32f1 and later)


## Authors (제작자)

- [@trymorez](https://www.github.com/trymorez)

## Screenshots (스크린샷)

![screenshot](Assets/Screenshot/Screenshot01.png)

## Version History (버전 내역)

**V 0.1 - (2025-01-17)**
- Project setup completed
- Level constructed with walls and obstacles (no code written yet)