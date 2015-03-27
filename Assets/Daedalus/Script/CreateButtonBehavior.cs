using UnityEngine;
using System.Collections;

public class CreateButtonBehavior : MonoBehaviour {
	
	void OnMouseEnter () {
		renderer.material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		renderer.material.color = Color.white;
	}

	void OnMouseUp () {
		CreatorModeSingleton.Instance.setMaze(new Maze());
		Application.LoadLevel(1);
	}

}
