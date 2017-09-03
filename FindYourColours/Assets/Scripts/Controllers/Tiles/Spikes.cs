using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour 
{
	void Start()
	{

	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.gameObject.name == "Player") 
		{
			Debug.Log("Player jumpes on " + this.gameObject.name);

			Debug.Log(other.gameObject.name);
			other.gameObject.GetComponent<Player>().SendMessage("UpdateColorHealth", false);
		}
	}
}