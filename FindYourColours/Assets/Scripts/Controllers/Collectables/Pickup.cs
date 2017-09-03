using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Pickup : MonoBehaviour, ICollectable 
{
	public virtual void Collect() {
		Destroy(this.gameObject);
	}

	public virtual void OnTriggerEnter2D(Collider2D other) {
		this.Collect();
	}
}
