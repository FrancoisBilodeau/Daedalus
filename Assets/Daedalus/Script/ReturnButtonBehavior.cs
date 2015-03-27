using UnityEngine;
using System.Collections;

public class ReturnButtonBehavior : MonoBehaviour {

	GameObject cameraMenu;

	void OnMouseEnter () {
		renderer.material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		renderer.material.color = Color.white;
	}

	void OnMouseUp () {
		cameraMenu = GameObject.Find("Camera");
		cameraMenu.animation["cameranim"].speed = -1;
		cameraMenu.animation["cameranim"].time = cameraMenu.animation["cameranim"].length;
		cameraMenu.animation.Play();
	}

}
