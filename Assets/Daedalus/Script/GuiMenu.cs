using UnityEngine;
using System.Collections;


public class GuiMenu : MonoBehaviour {
	
	public GUISkin skin = null;
	GameObject cameraMenu;
	private bool isButtonVisible = false;
	
	void OnGUI(){
		
		GUI.skin = skin;
		
		if (isButtonVisible && GUI.Button (new Rect (Screen.width - 125,Screen.height - 50,125,50), "Return to menu")) {
			cameraMenu = GameObject.Find("Camera");
			// Inverser l'animation pour revenir au main menu
			cameraMenu.animation["cameranim"].speed = -1;
			cameraMenu.animation["cameranim"].time = cameraMenu.animation["cameranim"].length;
			Debug.Log (cameraMenu.animation ["cameranim"].time);
			isButtonVisible = false;
			cameraMenu.animation.Play();
		}
	}
	
	public void setButtonVisible(bool visible) {
		
		isButtonVisible = visible;
		
	}
	
	public GuiMenu () {
		isButtonVisible = false;
	}
	
	
}
