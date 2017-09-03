using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterMotor2D))]
public class Player : LivingCharacter 
{
	// Create motor object to handle movement and collision detection controls
	[System.NonSerialized] public CharacterMotor2D characterMotor;

	// Reference to input manager instance to handle input for player
	[System.NonSerialized] InputManager inputManager;
	[System.NonSerialized] GameManager gameManager;
	[System.NonSerialized] AudioManager audioManager;

	// Movement specific variables
	bool isFacingRight;
	[System.NonSerialized] public float jumpHeight = 4.5f;
	[System.NonSerialized] public float timeToJumpApex = .3f;
	float moveSpeed = 12;
	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float velocityXSmoothing;

	// Player stats
	private int maxHealth = 16;
	private int currentHealth = 0;
	private float colourToHealthValue;

	// Player bullet variables
	private float secondsBetweenBulletFire = 0.3f;
	private bool isRunningShootingCoroutine = true;
	public float bulletSpeed = 15f;
	public GameObject bulletPrefab;

	// Camera instance for calling on effects such as camera shake
	private CameraController cam;

	// Initial initialization :D
	void Awake() 
	{
		// Get instance of the input manager class
		this.inputManager = Object.FindObjectOfType<InputManager>();
		this.gameManager = Object.FindObjectOfType<GameManager>();
		this.audioManager = Object.FindObjectOfType<AudioManager>();

		this.cam = Object.FindObjectOfType<CameraController>();
	}

	// Initialization
	void Start () 
	{

		// Get character motor object for handling movement
		characterMotor = GetComponent<CharacterMotor2D> ();

		// Calculate gravity and jump velocity from jumpHeight and timeToJumpApex
		// this is because those types of variables are easier for fiddling with and such for game feel
		this.gravity = calculateGravity();
		this.jumpVelocity = Mathf.Abs (this.gravity) * this.timeToJumpApex;

		// Calculate value difference between health and game color value
		this.colourToHealthValue = 0.0625f;

		this.isFacingRight = true;
		StartCoroutine(Shoot());
	}
	
	// Update is called once per frame
	void Update () 
	{
		// check if grounded to not accumulate gravity
		if (characterMotor.collisions.above || characterMotor.collisions.below) 
		{
			velocity.y = 0;
		}

		// We want a vector to store input
		Vector2 input = new Vector2(this.inputManager.horizontalAxis.GetInputRaw(), this.inputManager.verticalAxis.GetInputRaw());

		// Check for jumps
		if (this.inputManager.verticalAxis.GetInputRaw() == 1) 
		{
			// The player input for a jump
			// Now we check for if the player is grounded or if it is a doubleJump;
			if (characterMotor.collisions.below) 
			{
				velocity.y = jumpVelocity;
			}
		}

		if (this.inputManager.horizontalAxis.GetInputRaw() == 1)
		{
			this.isFacingRight = true;
		}
		else if (this.inputManager.horizontalAxis.GetInputRaw() == -1)
		{
			this.isFacingRight = false;
		}

		// apply any input to player movement velocity with acceleration smoothing depending on whether airborne or grounded
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, characterMotor.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);

		// apply gravity to player
		velocity.y += gravity * Time.deltaTime;

		// call the move function for the character motor
		characterMotor.Move (velocity * Time.deltaTime);
	}

	public override void TakeHit(int damage, Vector2 hitDirection)
	{
		if(this.currentHealth - damage >= 0) 
		{
			// Decrease health by damage
			this.currentHealth = this.currentHealth - damage;
			this.gameManager.UpdateGrayscaleValue(this.colourToHealthValue * damage);

			// Shake the cam
			this.cam.Shake();

			//TODO: Something with hitDirection, perhaps knockback
		}
		else
		{
			this.Die();
		}
	} 

	public override void Die()
	{
		// TODO: Make sound effect happen
		base.Die();
		this.gameManager.Lose();
	}

	// Do gravity calculations! Wow! So amazing!
	public float calculateGravity() 
	{ 
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		return gravity;
	}

	public void UpdateColorHealth(bool positive) 
	{
		if(positive) 
		{
			// We would like to remove grayscale and add to health
			if(this.currentHealth < this.maxHealth) 
			{
				this.currentHealth += 1;
				Debug.Log(-this.colourToHealthValue);
				this.gameManager.UpdateGrayscaleValue(-this.colourToHealthValue);
				this.gameManager.IncrementCollectedScore();
			}
		}
		else
		{
			this.TakeHit(1, Vector2.zero);
		}
	}

	IEnumerator Shoot() {
		while(isRunningShootingCoroutine) {
			if (this.inputManager.GetKey ("Fire") || this.inputManager.GetKey("alt_Fire")) 
			{
				this.audioManager.Play("Fire!");
				
				GameObject bullet = (GameObject)Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
				bullet.GetComponent<Bullet> ().SetBulletSpeed (this.bulletSpeed);
				if(this.isFacingRight)
				{
					bullet.transform.Rotate(0, 0, -90);
					bullet.transform.position += bullet.transform.up * moveSpeed / 12;
				}
				else
				{
					bullet.transform.Rotate(0, 0, 90);
					bullet.transform.position += bullet.transform.up * moveSpeed / 12;
				}
				
				this.cam.Shake();

				yield return new WaitForSeconds(secondsBetweenBulletFire);
			} 
			else 
			{
				yield return null;
			}
		}
		yield return null;
	}
}
