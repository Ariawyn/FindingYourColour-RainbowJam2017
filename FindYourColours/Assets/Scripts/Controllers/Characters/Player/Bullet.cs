using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float moveSpeed = 15;
	public float destroyTimer = 5;
	public float currentTimer;



	void Start() 
	{
		currentTimer = destroyTimer;
	}

	void FixedUpdate () 
	{
		currentTimer -= Time.fixedDeltaTime;
		if (currentTimer <= 0) 
		{
			Destroy (this.gameObject);
		}
		transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
	}

	public void SetBulletSpeed(float f){
		moveSpeed = f;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Enemy") 
		{
			BasicEnemy e = other.gameObject.GetComponent<BasicEnemy> ();
			other.gameObject.SendMessage ("Die");

			Destroy(this.gameObject);
		}
		else if (other.gameObject.layer == 8)
		{
			// Hit obstacle
			Destroy(this.gameObject);
		}
	}

}
