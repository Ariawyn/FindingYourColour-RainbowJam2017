using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	// Singleton instance
	public static GameManager instance;

	// Game state enum value
	private GAME_STATE gameState;

	// Manager instances
	private LevelManager levelManager;
	private AudioManager audioManager;
	private InputManager inputManager;

	// Variables that keep track of current level
	string currentLevelName = "TestLevel";
	bool currentLevelHasLoaded = false;
	bool currentLevelFinished = false;


	// Variables that keep track of game state
	[Range(0,1)] 
	private float grayscaleLevel = 1f;
	Material grayscalePostEffect;

	// Scoring stuff
	int goalColorPiecesToCollect = 16;
	int currentCollectedColorPieces = 0;


	// UI stuff
	private Text collectedGUItext;
	private Text goalGUItext;
    private readonly object SceneManagement;

    // Use this for initialization
    void Awake () 
	{
		// Check if we are violating the singleton rule
		if(instance != null) 
		{
			// There can be only one
			Destroy(this);
		}
		else
		{
			// Set instance
			instance = this;

			// Do other stuff
			this.gameState = GAME_STATE.MAIN_MENU;
		}
	}

	public void Start() 
	{
		// Find manager instances we need
		this.levelManager = GameObject.FindObjectOfType<LevelManager>();
		this.audioManager = GameObject.FindObjectOfType<AudioManager>();
		this.inputManager = GameObject.FindObjectOfType<InputManager>();

	}

	public void Play() 
	{
		// Load correct scene
		UnityEngine.SceneManagement.SceneManager.LoadScene("main");

		// Set game state
		this.gameState = GAME_STATE.RUNNING;
	}

	public void Menu()
	{
		// Load correct scene
		UnityEngine.SceneManagement.SceneManager.LoadScene("menu");

		// Set game state
		this.gameState = GAME_STATE.MAIN_MENU;
	}

	public void Tutorial()
	{
		// Load correct scene
		UnityEngine.SceneManagement.SceneManager.LoadScene("tutorial");

		// Set game state
		this.gameState = GAME_STATE.TUTORIAL;
	}

	public void Win()
	{
		// Load correct scene
		UnityEngine.SceneManagement.SceneManager.LoadScene("won");

		// Set game state
		this.gameState = GAME_STATE.WON;

		this.currentLevelHasLoaded = false;
		this.levelManager.hasSetPlayer = false;
		this.ResetScore();
	}

	public void Lose()
	{
		// Load correct scene
		UnityEngine.SceneManagement.SceneManager.LoadScene("lost");

		// Set game state
		this.gameState = GAME_STATE.LOST;

		this.currentLevelHasLoaded = false;
		this.levelManager.hasSetPlayer = false;
		this.ResetScore();
	}

	public void Pause()
	{
		// Set game state
		this.gameState = GAME_STATE.PAUSED;

		// Check if we have the pause menu
		/*if(!this.pauseMenu) {
			this.pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
		}

		// Enable pause menu
		this.pauseMenu.SetActive(true);*/

		// Pause game
		Time.timeScale = 0;

		// Pause the BGM in audio manager
		this.audioManager.PauseBGM();
	}

	public void Unpause()
	{
		// Set game state
		this.gameState = GAME_STATE.RUNNING;

		// Check if we have the pause menu
		/*if(!this.pauseMenu) {
			this.pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
		}

		// Disable pause menu
		this.pauseMenu.SetActive(false);*/

		// Unpause game
		Time.timeScale = 1;

		// Unpause the BGM in audio manager
		this.audioManager.UnpauseBGM();
	}
	

	public void ResetScore() {
		this.currentCollectedColorPieces = 0;

		this.grayscaleLevel = 1;

		if(this.grayscalePostEffect) 
		{
			Debug.Log(this.grayscaleLevel);
			Debug.Log("Is set");
			this.grayscalePostEffect.SetFloat("_EffectAmount", this.grayscaleLevel);
		}
	}


	public void UpdateGrayscaleValue(float amount) 
	{
		this.grayscaleLevel = this.grayscaleLevel + amount;
		// TODO: Update actual grayscale shader effect amount
		if(!this.grayscalePostEffect) 
		{
			Debug.Log(this.grayscaleLevel);
			Debug.Log("Not set");
			this.grayscalePostEffect = Camera.main.GetComponent<GrayscalePostEffect>().grayscalePostEffectMaterial;
		}

		if(this.grayscalePostEffect) 
		{
			Debug.Log(this.grayscaleLevel);
			Debug.Log("Is set");
			this.grayscalePostEffect.SetFloat("_EffectAmount", this.grayscaleLevel);
		}
	}

	public void IncrementCollectedScore()
	{
		if(this.currentCollectedColorPieces + 1 == this.goalColorPiecesToCollect)
		{
			this.Win();
		}
		else
		{
			this.currentCollectedColorPieces++;
			this.collectedGUItext.text = this.currentCollectedColorPieces.ToString();
		}
		
	}

	// In update, we mostly just check against our game state and see if we need to display any messages
	void Update() 
	{
		// Check if game is paused, this should only occur if the game is in the running or paused state, not menu or finished
		if(this.gameState == GAME_STATE.RUNNING || this.gameState == GAME_STATE.PAUSED) {
			// If the pause key is pressed
			if(this.inputManager.GetKeyDown("Pause")) {
				// We either pause or unpause
				if(this.isPaused()) {
					this.Unpause();
				} else {
					this.Pause();
				}
			}
		}

		// Attempt to locate the level container and level manager, and then generate map if we can if the game is running
		bool levelManagerHasLoaded = this.levelManager.CanLoadMap();
		if(this.gameState == GAME_STATE.RUNNING && levelManagerHasLoaded && !currentLevelHasLoaded) {
			Debug.Log("Attempting to load / generate level");
			// Load correct level
			this.levelManager.GenerateLevel(this.currentLevelName);
			this.audioManager.Play(this.levelManager.currentLevel.bgm);
			currentLevelHasLoaded = true;
		}

		// Attempt to locate the grayscale post effect of the camera if the game is running
		if(this.gameState == GAME_STATE.RUNNING && !this.grayscalePostEffect) {
			Camera cam = Camera.main;
			if(cam)
			{
				this.grayscalePostEffect = cam.GetComponent<GrayscalePostEffect>().grayscalePostEffectMaterial;
			}
			if(this.grayscalePostEffect) {
				this.grayscalePostEffect.SetFloat("_EffectAmount", 1);
			}
		}

		// Attempt to locate score text if the game is running
		if(this.gameState == GAME_STATE.RUNNING && !this.collectedGUItext) {
			//Debug.Log("The game is running and we do not have instance of scoreGUItext");
			GameObject temp = GameObject.FindGameObjectWithTag("NumberCollected");
			
			// See if we found an object with the score text tag
			if(temp) {
				// If we do then we set the text
				this.collectedGUItext = temp.GetComponent<Text>() as Text;
			}

			// Init
			if(this.collectedGUItext) {
				//Debug.Log("Found score text");
				this.collectedGUItext.text = this.currentCollectedColorPieces.ToString();
			}
		}

		// Attempt to locate score text if the game is running
		if(this.gameState == GAME_STATE.RUNNING && !this.goalGUItext) {
			//Debug.Log("The game is running and we do not have instance of scoreGUItext");
			GameObject temp = GameObject.FindGameObjectWithTag("NumberRequired");
			
			// See if we found an object with the score text tag
			if(temp) {
				// If we do then we set the text
				this.goalGUItext = temp.GetComponent<Text>() as Text;
			}

			// Init
			if(this.goalGUItext) {
				//Debug.Log("Found score text");
				this.goalGUItext.text = this.goalColorPiecesToCollect.ToString();
			}
		}
	}

	public bool isPaused() {
		return this.gameState == GAME_STATE.PAUSED;
	}
}
