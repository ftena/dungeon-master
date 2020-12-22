using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Completed
{
	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.
	
	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 5f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.1f;							//Delay between each Player turn.
		public int playerFoodPoints = 100;						//Starting value for Player food points.
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		private GameObject keyImage; 							//Key found by the player. 
		private Text levelText;									//Text to display current level number.
		private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		private BoardCreator boardCreator;						//Store a reference to our BoardCreator which will set up the level.
		private EnemyManager enemyManager;						//Store a reference to our EnemyManager which will set up the enemies.
		private int level = 0;									//Current level number, expressed in game as "Day 1".
		private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.

		//Awake is always called before any Start functions
		void Awake()
		{
			//Check if instance already exists
			if (instance == null)
				
				//if not, set instance to this
				instance = this;
			
			//If instance already exists and it's not this:
			else if (instance != this)
				
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);	
			
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);

			//Get a component reference to the attached BoardCreator script
			boardCreator = GetComponent<BoardCreator> ();

			//Get a component reference to the attached EnemyManager script
			enemyManager = GetComponent<EnemyManager> ();
		}

        void Start()
        {
            //Get a reference to our key image and, by default, hide it.
            keyImage = GameObject.Find("KeyImage");
            keyImage.SetActive(false);
        }

		void OnEnable()
		{
			//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		}

		void OnDisable()
		{

		}

		//Unity API. This is called each time a scene is loaded. 
		void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode)
		{
			//Add one to our level number.
			level++;

			if (level > 1) {
				if (level % 5 == 0) {
					// TODO ShowAd ();
				}

				//Call InitGame to initialize our level.
				InitGame ();
			}
		}
		
		//Initializes the game for each level.
		public void InitGame()
		{
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;

			//Disable the StoryImage gameObject.
			GameObject.Find("StoryImage").SetActive(false);

			//Get a reference to our key image and, by default, hide it.
			keyImage = GameObject.Find("KeyImage");
			if(keyImage)
			{
				keyImage.SetActive(false);
			}
			
			//Get a reference to our image LevelImage by finding it by name.
			levelImage = GameObject.Find("LevelImage");
			
			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			levelText = GameObject.Find("LevelText").GetComponent<Text>();
			
			//Set the text of levelText to the string "Day" and append the current level number.
			levelText.text = "Descending the dungeon / Level #" + level;
			
			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);
			
			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);

			//Call the SetupScene function of the BoardCreator script, pass it current level number.
			boardCreator.SetupScene(level);	

			//Call the SetupEnemies function of the EnemyManager script, pass it current level number.
			enemyManager.SetupEnemies(level, boardCreator.AvailableRooms);	
		}
		
		
		//Hides black image used between levels
		void HideLevelImage()
		{
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);
			
			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
		}
		
		//Update is called every frame.
		void Update()
		{
			
		}
		

		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{
			//Set levelText to display number of levels passed and game over message
			levelText.text = "After " + level + " days, you died.";
			
			//Enable black background image gameObject.
			levelImage.SetActive(true);
			
			//Disable this GameManager.
			enabled = false;
		}
	}
}

