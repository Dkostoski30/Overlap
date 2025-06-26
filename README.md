# Overlap
Project for the course Programming Video Games </br>
Authors: Daniel Kostoski 221189 & Dragana Usovikj 221043

## Requirements for activating the game
<ol>
  <li>Download Unity at https://unity.com/download</li>
  <li>Clone the project on your computer with git clone "https://github.com/Dkostoski30/Overlap.git"</li>
  <li>Open Unity Hub and load project from disk</li>
  <li>If the first scene that opens is not Menu scene, then go to Scenes > Menu and press the play button in the middle of the screne above the scene</li>
  <li>Follow the UI and play the game...</li>
</ol>

## Video of gameplay
The video should be in the root of this repository

## Screenshots of gameplay
The screenshots should be in a folder named screenshots-gameplay in the root of this repository

## How we made the game
We started by creating an empty project on Unity and adding version control on GitHub. We gathered the assets, such as car sprites, trees/bushes, racing tracks and background from https://wolfram-studio.itch.io/f1-race-car-asset-pack . First we added the camera, on which we added multiple scripts such as PositionHandler and SpawnCars. But, the most important part of this game is to have a drivable car, therefore we added the car sprite and attached the appropriate scripts and components to the car. We attached a Rigidbody2D to enable physics and collision and to simulate realistic arcade movements. With that, we added a Box Collider 2D that works with the RigidBody2D, defines the car’s collision area allowing it to bump into or interact with other objects. The scripts we wrote for the car to work are: TopDownCarController, CarInputHandler, CarLapCounter, CarAIHandler and AStarLite (these go on the game object car). The TopDownCarController handles the core vehicle dynamics. It simulates acceleration, braking, steering, and drifting for the car. The CarInputHandler  captures user inputs (like keyboard or joystick commands) and converts them into instructions that can be understood by the TopDownCarController. It acts as the interface between the player and the car mechanics. The CarAIHandler handles the AI-controlled cars. It receives waypoints or directions from the pathfinding system and translates them into movement commands. AStarLite is a simplified version of the A* pathfinding algorithm. It allows AI cars to navigate the track efficiently by calculating optimal paths through a waypoint system. CarLapCounter handles tracking each car’s progress around the track. It detects when a car crosses certain checkpoints or finish lines and counts laps. On the car sprite, we added skid marks and smoke particles to give a more realistic gameplay. We also added a text label to show the place of the car when passing a checkpoint. Finally, we saved the car as a prefab in order to be able to use it later on (and if we change it in the prefab, it is changed everywhere it is used). We configured the inputs for the player from keyboard or joystick by adding controls in the input manager in project settings. <br>
Next we added multiple test scenes to try driving the car and to add all of the other functionalities (more cars, AI bots, canvases with text, selectable car UI, lap counting). We added Edge colliders to the racing track and added checkpoints for progress and for checking if the game is finished. The game is finished when the player makes 3 laps. There he can view the leaderboard and choose to play again or exit to the main menu. To check this, we added a GameManager script that is instantiated when any of the scenes is played, to remember the game state and change it accordingly. <br>
On the main racing course, we added all of these functionalities together, configured the scene in the build profile to start after the menu of the selecting car UI is presented. We added the camera, arranged the checkpoints (it is crucial that in all of them the isTrigger is true and that the last checkpoint is the finish line). We added the AIPath waypoints and attached the appropriate scripts, built the racing tracks and added collision to the track. The EventSystem game object is there to handle the mouse input for the play again/exit buttons. Finally, we added the spawning points for the cars (3 AI and 1 player). With all of this, if we navigate to the Menu scene and push play, the game starts and covers all of the mentioned functionalities.

## References
We created this 2D F1 game mostly by watching tutorials on YouTube and reading the documentation of Unity. Below is a list of all of them:
<ul>
  <li>Unity documentation: https://docs.unity.com/en-us</li>
  <li>Top Down 2D Car Game: https://www.youtube.com/playlist?list=PLyDa4NP_nvPfmvbC-eqyzdhOXeCyhToko</li>
  <li>Racing Cars: https://www.youtube.com/watch?v=WS63kVcb6xk&ab_channel=LetsCodeGames</li>
</ul>
