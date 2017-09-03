using UnityEngine;

[System.Serializable]
public class Level 
{
	public string name;
	
	public string fileName;

	public string bgm;
	
	public int gridLength = 6;

	public int gridHeight = 6;

	public int roomWidth = 40;

	public int roomHeight = 25;

	public CharacterToLevelTilePrefab[] availableLevelTiles;
}
