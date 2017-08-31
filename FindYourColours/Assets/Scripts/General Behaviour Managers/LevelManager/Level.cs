using UnityEngine;

[System.Serializable]
public class Level {
	public string name;
	
	public string fileName;

	public string bgm;

	public CharacterToLevelTilePrefab[] availableLevelTiles;
	
	public int gridLength = 6;

	public int gridHeight = 3;

	public int roomWidth = 40;
	public int roomHeight = 25;
}
