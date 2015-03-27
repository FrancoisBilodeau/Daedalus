using UnityEngine;
using System.Collections;

public class CreatorModeSingleton {

	private int wallHeight = 3;
	private float tileSize = 2.0f;
	private Maze maze;

	private static CreatorModeSingleton instance;
	
	private CreatorModeSingleton(){
		maze = new Maze();
	}
	
	public static CreatorModeSingleton Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new CreatorModeSingleton();
			}
			return instance;
		}
	}

	//Pour pouvoir obtenir un objet de type labyrinthe
	public Maze getMaze(){
		return maze;
	}

	//Pour pouvoir modifier et travailler avec un objet de type labyrinthe
	public void setMaze(Maze maze){
		this.maze = maze;
	}

	//Pour get la hauteur des mur d'un labyrinthe
	public int getWallHeight(){
		return wallHeight;
	}

	//Pour set la hauteur d'un mur d'un labyrinthe
	public void setWallHeight(int wallHeight){
		this.wallHeight = wallHeight;
	}
	
	//Pour get la taille d'une tuile d'un labyrinthe
	public float getTileSize(){
		return tileSize;
	}
		
}
