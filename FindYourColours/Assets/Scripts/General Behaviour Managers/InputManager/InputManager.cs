using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour 
{

	// Dictionary for holding keycodes with our own string names
	Dictionary<string, KeyCode> controls;

	// Add two default input axis
	public Axis horizontalAxis;
	public Axis verticalAxis;

	// Use this for initialization
	void Start () 
	{
		// Initialzie keys
		this.initializeControls();
	}

	private void initializeControls() 
	{
		// Attempt to load from saved preferences file
		this.controls = this.loadControlLayout();

		// If we return with an empty control layout, then we add instead the default values
		if(this.controls.Count == 0) 
		{
			// Default movement controls
			this.controls["Forward"] = KeyCode.W;
			this.controls["Backward"] = KeyCode.S;
			this.controls["Left"] = KeyCode.A;
			this.controls["Right"] = KeyCode.D;

			// Alternative movement controls
			this.controls["alt_Forward"] = KeyCode.UpArrow;
			this.controls["alt_Backward"] = KeyCode.DownArrow;
			this.controls["alt_Left"] = KeyCode.LeftArrow;
			this.controls["alt_Right"] = KeyCode.RightArrow;

			// Mouse interaction controls
			this.controls["Interact"] = KeyCode.Mouse0;
		}

		// Initialize horizontal input axis
		this.horizontalAxis = new Axis(this.controls["Right"], this.controls["Left"], 
			this.controls["alt_Right"], this.controls["alt_Left"]);

		// Initialize vertical input axis
		this.verticalAxis = new Axis(this.controls["Forward"], this.controls["Backward"],
			this.controls["alt_Forward"], this.controls["alt_Backward"]);
	}

	public bool GetKey(string keyName) 
	{
		// Check if the key exists
		if(this.controls.ContainsKey(keyName)) 
		{
			// Then we do have the key in the control layout
			return Input.GetKey(this.controls[keyName]);
		}

		// We do not have the key registered in our layout
		return false;
	}


	public bool GetKeyDown(string keyName) 
	{
		// Check if the key exists
		if(this.controls.ContainsKey(keyName)) 
		{
			// Then we do have the key in the control layout
			return Input.GetKeyDown(this.controls[keyName]);
		}

		// We do not have the key registered in our layout
		return false;
	}

	private Dictionary<string, KeyCode> loadControlLayout() 
	{
		// Check if player has key layout already saved
		// TODO: Load key layout from file

		// We did not return before this point, so we should just initialise an empty Dictionary as there was no previous saved layout
		return new Dictionary<string, KeyCode>();
	} 

	private void saveKeyLayout() 
	{
		// TODO: Implement
		return;
	}
}
