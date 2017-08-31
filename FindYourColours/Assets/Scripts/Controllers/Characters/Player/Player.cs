using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterMotor2D))]
public class Player : MonoBehaviour {
	// Create motor object to handle movement and collision detection controls
	[System.NonSerialized] public CharacterMotor2D characterMotor;

	// Reference to input manager instance to handle input for player
	[System.NonSerialized] InputManager inputManager;

	// Movement specific variables
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
	int maxHealth;
	int currentHealth;

	// Initial initialization :D
	void Awake() {
		// Get instance of the input manager class
		inputManager = Object.FindObjectOfType<InputManager>();
	}

	// Initialization
	void Start () {

		// Get character motor object for handling movement
		characterMotor = GetComponent<CharacterMotor2D> ();

		// calculate gravity and jump velocity from jumpHeight and timeToJumpApex
		// this is because those types of variables are easier for fiddling with and such for game feel
		gravity = calculateGravity();
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;

		// Debug information
		print ("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
	}
	
	// Update is called once per frame
	void Update () {
		// check if grounded to not accumulate gravity
		if (characterMotor.collisions.above || characterMotor.collisions.below) {
			velocity.y = 0;
		}

		// We want a vector to store input
		Vector2 input = new Vector2(this.inputManager.horizontalAxis.GetInputRaw(), this.inputManager.verticalAxis.GetInputRaw());

		// Check for jumps
		if (this.inputManager.verticalAxis.GetInputRaw() == 1) {
			// The player input for a jump
			// Now we check for if the player is grounded or if it is a doubleJump;
			if (characterMotor.collisions.below) {
				velocity.y = jumpVelocity;
			}
		}


		// apply any input to player movement velocity with acceleration smoothing depending on whether airborne or grounded
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, characterMotor.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);

		// apply gravity to player
		velocity.y += gravity * Time.deltaTime;

		// call the move function for the character motor
		characterMotor.Move (velocity * Time.deltaTime);
	}

	// Do gravity calculations! Wow! So amazing!
	public float calculateGravity() { 
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		return gravity;
	}
}
