using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class PlayButtonBehavior : MonoBehaviour {
	
	GameObject cameraMenu;
	
	void OnMouseEnter () {
		GetComponent<Renderer>().material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseUp () {
		cameraMenu = GameObject.Find("Camera");
		cameraMenu.GetComponent<Animation>()["cameranim"].speed = 1;
		cameraMenu.GetComponent<Animation>().Play("cameranim");
		Invoke("reset", 2.45f);
		PageSingleton.Instance.refresh();
		PageSingleton.Instance.showPage();

	}
	
	void reset(){
		((GuiMenu)(GameObject.Find ("Plane").GetComponent ("GuiMenu"))).setButtonVisible (true);
	}
	
	
}
