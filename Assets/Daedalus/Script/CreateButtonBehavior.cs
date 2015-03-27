using UnityEngine;
using System.Collections;

public class CreateButtonBehavior : MonoBehaviour {
	
	void OnMouseEnter () {
		GetComponent<Renderer>().material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseUp () {
		CreatorModeSingleton.Instance.setMaze(new Maze());
		Application.LoadLevel(1);
	}

}
