using UnityEngine;

public class PreloadBehaviour : MonoBehaviour {

	private GameManager gameManager;

	void Start () {
		this.gameManager = GetComponent<GameManager>();
		this.gameManager.Play();
	}
}