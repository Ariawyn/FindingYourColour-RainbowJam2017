using UnityEngine;

public struct Axis 
{
	// Base keys
	KeyCode positive, negative;
	// Alt keys
	KeyCode alt_positive, alt_negative;

	// Constructor that sets all keycode values
	public Axis(KeyCode pos, KeyCode neg, KeyCode alt_pos, KeyCode alt_neg) 
	{
		// Init base keys
		this.positive = pos;
		this.negative = neg;

		// Init alternative keys
		this.alt_positive = alt_pos;
		this.alt_negative = alt_neg;
	}

	// Return whole value of the input
	public float GetInputRaw() {
		// Get base input
		bool basePositiveKeyInput = Input.GetKey(positive);
		bool baseNegativeKeyInput = Input.GetKey(negative);

		// Get alt input
		bool altPositiveKeyInput  = Input.GetKey(alt_positive);
		bool altNegativeKeyInput  = Input.GetKey(alt_negative);

		// Tie positive and negative inputs together
		bool totalPositiveInput = basePositiveKeyInput || altPositiveKeyInput;
		bool totalNegativeInput = baseNegativeKeyInput || altNegativeKeyInput;

		// Return appropriate values according to total input
		if(totalPositiveInput && !totalNegativeInput) 
		{
			// We are getting net positive input
			return 1;
		} 
		else if(totalNegativeInput && !totalPositiveInput) 
		{
			// We are getting net negative input
			return -1;
		}
		else 
		{
			// Here we are either getting no input, or we are getting both positive and negative input
			return 0;
		}
	}
}