The goal of this project is to be able to progress early season prototyping in FRC after crayon cadding by making an easy to use tool for making simulated versions of robots in unity. Allowing for FRC robots to be conceptualized then put through virtual representations that assume perfect execution of the design.

***Alpha 1 proggress***
- generate drive train (at Alpha 1 Goal)
    - set frame width and length seperatly
    - set bumper height (relative to center of frame)
    - set max speed
    - set acceleration force
- Generate Elevator (at Alpha 1 Goal)
    - One Script as many stages as you want
    - Set the height of the elevator in inches
    - set the width of the elevator in inches
    - Set the weight of the "stationary" stage
    - set the weight individually of the moving stages
    - Customizable setpoints with ability to choose toggle, hold, sequence
- generate Arm (at alpha 1 goal)
    - single stage only
    - setpoint control same as elevator
    - continuous aim mode for shooting games
- Changeable field (at Alpha 1 goal)
    - Swap between The Crescendo and Evergreen fields
    - Crescendo features a fully dynamic chain
    - Optimized Variant of Crescendo field with lower physics ticks on gamepieces and every 5th chain piece is dynamic
- Generate hook (at Alpha 1 Goal)
- Game Piece animation system (at alpha 1 goal)
    - 3 stage system consisting of intake, stow, and outake.
    - no delete game piece system means there is no need for "hidden" objects
    - Generic nature of the system allows for use in most years games.
    - Ability to form complex interacitons using the ability to have multiple outputs from a stow locatoin.
- File Cleanup (Complete)
- Example Robots (in progress)
- Early Documentation (in progress)

***Missing or future update features***
- metric support (non inch settings)
- non 1x2 tubing mechanisms
- delay options for game piece system
- Multi jointed arms
- different swerve module configs and shapes
- Generate Turret
- Decorative Generators
- Basic Motor simulation

***Non targeted features (unlikley/never)***
- Non unity editor support (including any kind of "game")
- Counted scoring for fields
- Robot to game piece physics
- color coded subsystems (unity gets mad due to the editor requirements)
