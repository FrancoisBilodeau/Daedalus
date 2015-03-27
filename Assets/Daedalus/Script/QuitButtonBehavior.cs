using UnityEngine;
using System.Collections;

public class QuitButtonBehavior : MonoBehaviour {
	
	void OnMouseEnter () {
		renderer.material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		renderer.material.color = Color.white;
	}

	void OnMouseUp () {
		Application.Quit();
	}

}
