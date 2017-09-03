using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : Pickup 
{

	public override void Collect() 
	{
		// TODO: Mess around with game manager thing maybe
		base.Collect();
	}

	public override void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.gameObject.name == "Player")
		{
			Debug.Log("Player collected " + this.gameObject.name);

			Debug.Log(other.gameObject.name);
			other.gameObject.GetComponent<Player>().SendMessage("UpdateColorHealth", true);

			this.Collect();
		}
	}
}
