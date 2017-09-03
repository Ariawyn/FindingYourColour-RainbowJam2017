using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : LivingCharacter {

	public Transform target;

	private int maxHealth = 1;

	private int currentHealth = 1;

	public EnemySpawner parent;

	private float speed = 1f;

	public override void TakeHit(int damage, Vector2 hitDirection)
	{
		this.Die();
	} 

	public override void Die()
	{
		// TODO: Make sound effect happen
		this.parent.enemiesSpawned--;
		base.Die();
	}
	
	void Update() {
		if(!target) {
			Debug.Log("No target?");	
			return;
		}

		// Get direction vector between target and this
		Vector3 direction = target.position - this.transform.position;

		// Calculate velocity
		Vector3 velocity = direction * speed * Time.deltaTime;

		// Move the player
		this.transform.Translate(velocity);
	}

	public void Move(Transform target, float minDistance, float maxDistance, float speed, ref bool inRange) {
		// Get direction vector between target and this
		Vector2 direction = target.position - this.transform.position;
		// Get magnitude for distance
		float magnitude = direction.magnitude;
		// Normalized direction
		direction.Normalize();

		// Calculate velocity
		Vector2 velocity = direction * speed * Time.deltaTime;

		// Move the player
		this.transform.Translate(velocity);
	}

	public void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.gameObject.name == "Player")
		{
			Debug.Log(this.gameObject.name + " hit the Player");

			Debug.Log(other.gameObject.name);
			other.gameObject.GetComponent<Player>().SendMessage("UpdateColorHealth", false);

			this.Die();
		}
	}
}
