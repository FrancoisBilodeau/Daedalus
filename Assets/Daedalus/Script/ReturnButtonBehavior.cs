using UnityEngine;
using System.Collections;

public class ReturnButtonBehavior : MonoBehaviour {

	GameObject cameraMenu;

	void OnMouseEnter () {
		GetComponent<Renderer>().material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseUp () {
		cameraMenu = GameObject.Find("Camera");
		cameraMenu.GetComponent<Animation>()["cameranim"].speed = -1;
		cameraMenu.GetComponent<Animation>()["cameranim"].time = cameraMenu.GetComponent<Animation>()["cameranim"].length;
		cameraMenu.GetComponent<Animation>().Play();
	}

}
