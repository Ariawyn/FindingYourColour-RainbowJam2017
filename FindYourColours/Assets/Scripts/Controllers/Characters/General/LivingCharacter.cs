using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingCharacter : MonoBehaviour, IDamageable 
{

	// Character information or stat variables
	/*public int maxHealth;
	public int currentHealth;*/
	protected bool dead;

	// Use this for initialization
	protected virtual void Start () 
	{
		//this.currentHealth = maxHealth;
	}

	public virtual void TakeHit(int damage, Vector2 hitDirection)
    {
        // TODO: Bounce the character in direction of hit

		// Take damage from hit
		//this.currentHealth -= damage;

		// Check if character died
		/*if(this.currentHealth < 0 && !dead) 
		{
			this.Die();
		}*/
    }

	public virtual void Die() 
	{
		this.dead = true;
		Destroy(this.gameObject);
	}
}
