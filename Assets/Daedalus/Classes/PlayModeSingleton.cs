using UnityEngine;
using System.Collections;
using System;

public class PlayModeSingleton {

	public float tileSize = 2.0f;
	public int wallHeight = 3;

	private static PlayModeSingleton instance;
	private SaveFile saveFile;
	private bool started;

	private PlayModeSingleton(){
		started = new bool();
		started = false;
	}

	public static PlayModeSingleton Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new PlayModeSingleton();
			}
			return instance;
		}
	}
	
	public SaveFile getSaveFile(){
		return saveFile;
	}

	public void setSaveFile(SaveFile saveFile){
		this.saveFile = saveFile;
	}

	public bool isStarted(){
		return started;
	}

	public void setStarted( bool isStarted){
		started = isStarted;
	}
}
