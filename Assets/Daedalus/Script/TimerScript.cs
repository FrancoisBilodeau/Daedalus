using UnityEngine;
using System.Collections;
using System;

public class TimerScript : MonoBehaviour {
	private float time;
	public GUISkin skin;
	public GUISkin endSkin;
	private PlayModeSingleton playMode = PlayModeSingleton.Instance;
	private bool reachEnd = false;
	private bool scoreSaved = false;
	private string playerName = "";

	// Use this for initialization
	void Start () {
		
		//create vector and get cube
		Vector2 end = playMode.getSaveFile().getMaze().end;
		Vector3 cubeCoor = transform.position;
		
		// set position to the start point
		cubeCoor.x = (end.y+1) *playMode.tileSize + playMode.tileSize/2f;
		cubeCoor.y = playMode.wallHeight/2f;
		cubeCoor.z = (end.x+1) *playMode.tileSize + playMode.tileSize/2f;
		//set the position to the cube
		transform.position = cubeCoor;
	}
	
	// Update is called once per frame
	void Update () {
		if(playMode.isStarted()){
			time += Time.deltaTime;
		}
	}

	void OnGUI(){
		if(reachEnd){
			GUI.skin = endSkin;
			GUILayout.BeginArea( new Rect((Screen.width - 500) / 2, (Screen.height - 500) / 2 , 300, 200));
			GUILayout.Label("Congratulations\n");
			GUILayout.Label("You have finished the maze in " + ((int)(time/60)).ToString("00:") + (time%60).ToString ("00.00"));
			TimeSpan timeSpan = new TimeSpan(0, 0, 0, (int)(time), (int)((time*1000f)%1000));

			if(playMode.getSaveFile().isInBestTime(timeSpan) && !scoreSaved){
				playerName = GUILayout.TextField(playerName,30);
				if(playerName.Contains(":")){
					GUILayout.Label("DON'T PUT A : IN YOUR NAME");
				}
				if(GUILayout.Button("Save")){
					if(!playerName.Contains(":")){
						TimeRecorded highScore = new TimeRecorded(playerName, timeSpan);
						playMode.getSaveFile().addBestTime(highScore);
						scoreSaved = true;
					}
				}
			}
			if (GUILayout.Button ("Restart")) {
				playMode.setStarted(false);
				Time.timeScale = 1;
				((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("MouseLook"))).enabled = true;
				Cursor.visible = false;
				Application.LoadLevel("PlayMode");
			}
			if (GUILayout.Button ("Quit")) {
				playMode.setStarted(false);
				Time.timeScale = 1;
				((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("MouseLook"))).enabled = true;
				Application.LoadLevel("MainMenu");
			}
			GUILayout.EndArea();
			GUILayout.BeginArea( new Rect((Screen.width + 200) / 2, (Screen.height - 500) / 2 , 400, 500));
			string highScoreList = "";
			for(int i = 0; i < 10; i++){
				highScoreList += ((i+1) + ". " + playMode.getSaveFile().getTimeRecorded(i).getPlayerName().ToString()) + "\n";
				highScoreList += (playMode.getSaveFile().getTimeRecorded(i).getTime().ToString()) + "\n";
				highScoreList += ("----------------------------------\n");
			}
			GUILayout.Label(highScoreList);
			GUILayout.EndArea();
		}else{
			GUI.skin = skin;
			GUILayout.BeginArea( new Rect(10, 10, 400, 200));
			string timestring= ((int)(time/60)).ToString("00:") + (time%60).ToString ("00.00");
			GUILayout.Label(timestring);
			GUILayout.EndArea();
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "Main Camera"){
			playMode.setStarted(false);
			reachEnd = true;
			Time.timeScale = 0;
			Cursor.visible = true;
			((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("MouseLook"))).enabled = false;
		}
	}
}
