using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class PlayButtonBehavior : MonoBehaviour {
	
	GameObject cameraMenu;
	
	void OnMouseEnter () {
		renderer.material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		renderer.material.color = Color.white;
	}
	
	void OnMouseUp () {
		cameraMenu = GameObject.Find("Camera");
		cameraMenu.animation["cameranim"].speed = 1;
		cameraMenu.animation.Play("cameranim");
		Invoke("reset", 2.45f);
		PageSingleton.Instance.refresh();
		PageSingleton.Instance.showPage();

	}
	
	void reset(){
		((GuiMenu)(GameObject.Find ("Plane").GetComponent ("GuiMenu"))).setButtonVisible (true);
	}
	
	
}
