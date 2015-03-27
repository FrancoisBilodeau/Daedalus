﻿using UnityEngine;
using System.Collections;

public class PreviousButtonBehavior : MonoBehaviour {

	PageSingleton page = PageSingleton.Instance;
	
	void OnMouseEnter () {
		GetComponent<Renderer>().material.color = Color.cyan;
	}
	
	void OnMouseExit () {
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseUp () {
		page.showPreviousPage();

	}
	
}
