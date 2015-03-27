using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {
	
	
	private float totalTime = 3.0f;
	private float currentTime = 0;
	private bool finished = false;
	public GUISkin skin;
	
	// Use this for initialization
	void Start () {	
		((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("MouseLook"))).enabled = false;
		((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("CharacterMotor"))).enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!finished){
			DegreaseTime();
		}
	}
	
	//Affichage du temps
	void OnGUI()
	{
		if(!finished){
			GUI.skin = skin;
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height /2 - 200, 600, 600), "" + totalTime);
			GUI.depth = 1;
		}
	}
	
	// Calcul du temps
	private void DegreaseTime()
	{
		float delta = Time.deltaTime;
		
		currentTime += delta;
		
		if (currentTime >= 1 )
		{
			
			if (totalTime - 1 <= 0)
			{
				totalTime = 0;          
				((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("MouseLook"))).enabled = true;
				((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("CharacterMotor"))).enabled = true;
				PlayModeSingleton playMode = PlayModeSingleton.Instance;
				playMode.setStarted(true);
				finished = true;
			}
			else
			{
				totalTime -= 1;
				currentTime = 0;
			}       
		}
		
	}
}