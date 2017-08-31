using UnityEngine;

// Public sound class for handling BGM and SFX
// No constructor is needed as this will always have to be used through the constructor
[System.Serializable]
public class Sound 
{
	// Name of track
	public string name;

	// Boolean for seeing if we want the track to loop
	public bool isLooping;

	// Boolean for seeing if this sound instance is a sound effect
	public bool isSFX;

	// Random values for making effects not sound repetitive, only used if isSFX is true
	[Range(0f, 0.5f)] public float randomVolumeValue = 0.1f;
	[Range(0f, 0.75f)] public float randomPitchValue = 0.1f;

	// Audio Clip instance for the audio track
	public AudioClip clip;

	// Audio source, where the audio plays from
	[HideInInspector] public AudioSource source;

	// Volume as a range between 0 - 1
	[Range(0f, 1f)] public float volume;

	// Pitch as a range between 0.1 - 3, 1 is normal pitch
	[Range(0.1f, 3f)] public float pitch;
}
