using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// Singleton instance
	public static GameManager instance;

	// Game state enum value
	private GAME_STATE gameState;

	private LevelManager levelManager;
	private AudioManager audioManager;


	// Keep track of current level
	int currentLevel = 1;
	string currentLevelName = "TestLevel";

	bool currentLevelHasLoaded = false;
	bool currentLevelFinished = false;

	// Use this for initialization
	void Awake () {
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

	public void Start() {
		this.levelManager = GameObject.FindObjectOfType<LevelManager>();
		this.audioManager = GameObject.FindObjectOfType<AudioManager>();
	}

	public void Play() 
	{
		// Load correct scene
		UnityEngine.SceneManagement.SceneManager.LoadScene("main");

		// Set game state
		gameState = GAME_STATE.RUNNING;
	}
	
	// In update, we mostly just check against our game state and see if we need to display any messages
	void Update() {
		bool levelManagerHasLoaded = this.levelManager.CanLoadMap();
		if(this.gameState == GAME_STATE.RUNNING && levelManagerHasLoaded && !currentLevelHasLoaded) {
			// Load correct level
			//this.levelManager.LoadMap(this.currentLevelName);
			this.levelManager.GenerateLevel(this.currentLevelName);
			this.audioManager.Play(this.levelManager.currentLevel.bgm);
			currentLevelHasLoaded = true;
		}
	}
}
