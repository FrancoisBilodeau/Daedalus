using UnityEngine;
using System.Collections;

public class NextButtonBehavior : MonoBehaviour {

	PageSingleton page = PageSingleton.Instance;
	
	void OnMouseEnter () {
		renderer.material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		renderer.material.color = Color.white;
	}
	
	void OnMouseUp () {
		page.showNextPage();
		
	}
	
}
