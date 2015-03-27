using UnityEngine;
using System.Collections;

public class QuitButtonBehavior : MonoBehaviour {
	
	void OnMouseEnter () {
		GetComponent<Renderer>().material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseUp () {
		Application.Quit();
	}

}
