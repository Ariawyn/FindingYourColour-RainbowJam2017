using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : LivingCharacter {
	public Transform target;

	private AudioManager audioManager;

	private int maxHealth = 1;

	private int currentHealth = 1;

	public GameObject basicEnemyPrefab;
	private float timer;
	private bool timerIsCounting;
	private bool hasSpawnedForEnemyTime;
	private int maxEnemyGroupSpawnAmount;
	private int lastSpawnTime;
	private int enemySpawnTime;
	public int enemiesSpawned;
	private int maxEnemiesToSpawn;

	private int spawnOffsetDistance = 2;

	private int rotationAmount = 5;

	public void OnEnable()
	{
		this.audioManager = GameObject.FindObjectOfType<AudioManager>();

		this.timer = 0.0f;
		this.timerIsCounting = false;

		this.enemySpawnTime = 3;
		this.lastSpawnTime = 0;

		this.maxEnemiesToSpawn = 4;
		this.maxEnemyGroupSpawnAmount = 3;
		this.enemiesSpawned = 0;

		this.target = GameObject.Find("Player").transform;
	}

	public override void TakeHit(int damage, Vector2 hitDirection)
	{
		this.Die();
	} 

	public override void Die()
	{
		// TODO: Make sound effect happen
		base.Die();
	}
	
	void FixedUpdate() 
	{
		if(!target) 
		{
			Debug.Log("No target?");	
			return;
		}

		this.transform.Rotate(new Vector3(0, 0, this.rotationAmount));

		this.timer += Time.deltaTime;

		int roundedTimer = Mathf.CeilToInt(this.timer);

		// Check if we need to reset spawn timer variables
		if(this.lastSpawnTime != 0) 
		{
			if(((roundedTimer % this.lastSpawnTime) + 1) == this.enemySpawnTime) 
			{
				this.hasSpawnedForEnemyTime = false;
			}
		}

		// Check whether it is time to spawn next wave of basic enemies
		if((roundedTimer % this.enemySpawnTime == 0) && (roundedTimer != 0) && (!this.hasSpawnedForEnemyTime)) 
		{
			// We now need to spawn the basic enemies at the right amount

			// Calculate amount we wish to spawn based on the timer
			int amountOfEnemiesToSpawn = 1 * Mathf.FloorToInt(roundedTimer / this.enemySpawnTime) / 2;

			// Check if the amount we want is greater than the max amount allowed to spawn at once
			// If it is, then we just set the desired amount to the max amount
			amountOfEnemiesToSpawn = (amountOfEnemiesToSpawn > this.maxEnemyGroupSpawnAmount)? 
				this.maxEnemyGroupSpawnAmount : amountOfEnemiesToSpawn;

			// Set spawner variables
			this.hasSpawnedForEnemyTime = true;
			this.lastSpawnTime = roundedTimer;

			// Call spawn function with desired amount
			this.SpawnEnemies(amountOfEnemiesToSpawn);
		}
	}

	void SpawnEnemies(int amount)
	{
		Debug.Log("Spawning " + amount + " of basic enemies at " + this.timer);

		// Loop through amount
		for(int i = 0; i < amount; i++) {

			// Check if we would exceed the max basic enemy amount allowed
			if(this.enemiesSpawned + 1 < this.maxEnemiesToSpawn) {

				// Get random radian angle
				float radian = Random.Range(0f, Mathf.PI*2);
					
				// Calculate x and y pos of angle
				float xPos = Mathf.Cos(radian);
				float yPos = Mathf.Sin(radian);

				// Calculate spawn point from spawn offset distance and the player position
				Vector3 spawnPoint = new Vector3(xPos, yPos, 0) * this.spawnOffsetDistance;
				spawnPoint = this.transform.position + spawnPoint;

				// Instantiate the enemy
				GameObject instantiated = (GameObject)Instantiate(this.basicEnemyPrefab, spawnPoint, Quaternion.identity);
				BasicEnemy spawned = instantiated.GetComponent<BasicEnemy>();

				// Make spawn sound
				this.audioManager.Play("Spawned!");

				// Set the players position as target
				spawned.target = this.target;

				// Set the spawned enemies parent spawner
				spawned.parent = this;

				// Increase number of enemies total spawned
				this.enemiesSpawned++;

			} else {
				// If we would exceed the max basic enemy amount allowed, then we just return early and do nothing more
				return;
			}
		}
	}
}
