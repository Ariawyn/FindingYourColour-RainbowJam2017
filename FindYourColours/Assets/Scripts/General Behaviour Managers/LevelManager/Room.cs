using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public bool isComplete;

	public Vector2 roomTopLeftPosition;

	public int height = 25;

	public int width = 40;

	public string roomFileName;

	public bool hasBeenVisited = false;

	public List<EnemySpawner> spawnersInRoom;

	public void EnableRoomSpawners()
	{
		if(spawnersInRoom != null)
		{
			foreach(EnemySpawner s in spawnersInRoom)
			{
				Debug.Log(s);
				s.enabled = true;
				s.gameObject.SetActive(true);
			}
		}
	}

}
