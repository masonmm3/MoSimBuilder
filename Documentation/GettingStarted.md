# Getting Started

## Downloading
  * unity is roughly 5.21 gb
  * Mo sim builder is roughly 1.65 gb
  * these are estimates and untested

### The first step is going to be to download unity hub
  * [Download Here](https://unity.com/download)
  * once installed open the app.
### The Second Step is to download the MoSimBuilder source. This can be done more than one way
  * simple install
    * install from the main page using Code -> Download Zip
    * go to your downloads folder and unzip to a location
  *  _advanced install_  (optionally fork first)
     * Open the github Desktop app
     * File -> clone Repository -> fill out fields
### The final Step is to let unity hub install the correct version of unity for you
  * in the unity hub app click the Add button in the top right
  * next add project from disk
  * select your project folder.
  * if you selected properly is will ask you if you want to install the correct version of unity, click yes.
  * you can now open the project once the download is complete.

## Getting Started

### Familiarizing
* in the photo below There is a photo of what is called the _Project Window_ this is where files and scenes are stored.
  ![image](https://github.com/user-attachments/assets/c43656f0-f2d7-4133-ae5d-ecb374d28579)
* in the photo above I have entered the Scenes folder, and double clicked the Field scene to open into the field.
* The Field scene is where all the magic happens all of the individual componenets come together to create the visualization of designs.
* Looking up to the center of the screen we are greeted by the Hierarchy on the left, and the Scene/Game View in the middle with the Play button above the scene view.
![image](https://github.com/user-attachments/assets/4eb4c310-33f1-4662-9b2c-350a3a5cac3f)
* In the Hierarchy we see a couple of things. GameHandler, GameManagement, DsColliders, AimFinder.
   * The Game Handler is where the field robots, and spawn points are set
   * The Game management houses ui and other static elements
   * DsColliders houses the non field side colliders for the driver stations
   * AimFinder is a blank object that can be used to find the cordinates of an aiming point.
* This brings us to the final thing, the Inspector window. When you select an object this window on the right side of the screen will populate with the "componenets" on the object.
   * When youe select the GameHandler object in the Heirarchy using a left click the inspector menu will populate as it is below
     ![image](https://github.com/user-attachments/assets/f35c028a-07b7-4fee-ae88-200d59513959)
   * This is where the bulk of the changes will occur.
   * The first "Componenet" of interest is the RobotSpawnController, on it we have
      * The robot name which will reference a prefab file in the Resources->Robots project folder.
      * Camera mode which selects which camera it is
      * alliance selects the alliance.
   * The second "componenet" is the FieldLoader. This is where you select your active field. The top field in the list is the one you will see before you. dragging a lower one to the top will result in the relevant field being selected
* finally we return to the Project view and open the Resources Folder, then the Robots folder and we can see all of the included robots
  ![image](https://github.com/user-attachments/assets/90746f51-7cff-4cb2-bb4e-3bd8c9a5f2f9) (photo was taken prior to completing this guide)
   * there is no requirement that names be numbers.
   * The file name is what you add to the Robot Spawn Controller in order to load the robot.
   * To play you select the Play button on the top of the scene view.
   * Controls? whatever you make them.

