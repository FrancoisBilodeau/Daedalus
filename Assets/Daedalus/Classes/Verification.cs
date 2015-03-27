using UnityEngine;
using System.Collections;

public class Verification {

	public enum VerificationResult {Ok, StartInvalide, EndInvalide, Incomplete};

	private Maze maze;

	public Verification(Maze maze){
		this.maze = maze;
	}
	
	public VerificationResult verification(Maze maze){
		bool[,] mazeData = (bool[,])maze.mapData.Clone();

		if(maze.start.x < 0 || maze.start.y < 0){
			return VerificationResult.StartInvalide;
		}else if(mazeData[(int)maze.start.x, (int)maze.start.y] == true){
			return VerificationResult.StartInvalide;
		}

		if(maze.end.x < 0 || maze.end.y < 0){
			return VerificationResult.EndInvalide;
		}else if(mazeData[(int)maze.end.x, (int)maze.end.y] == true){
			return VerificationResult.EndInvalide;
		}
		if(travel (mazeData, maze.start)){
			return VerificationResult.Ok;
		}else{
			return VerificationResult.Incomplete;
		}
	}

  // fonction récursive appelée pour chaque case.
	private bool travel(bool[,] mazeData, Vector2 coor){
		if(coor.Equals(maze.end)){
			return true;
		}

		mazeData[(int)coor.x, (int)coor.y] = true;
		if( coor.x > 0 && mazeData[(int)coor.x -1, (int)coor.y] == false){
			if(travel(mazeData, new Vector2 ((int)coor.x -1, (int)coor.y))){
				return true;
			}
		}
		if(coor.x < maze.sizeX - 1 && mazeData[(int)coor.x + 1, (int)coor.y] == false){
			if(travel(mazeData, new Vector2 ((int)coor.x +1, (int)coor.y))){
				return true;
			}
		}
		if(coor.y > 0 && mazeData[(int)coor.x, (int)coor.y - 1] == false){
			if(travel(mazeData, new Vector2 ((int)coor.x, (int)coor.y - 1))){
				return true;
			}
		}
		if(coor.y < maze.sizeZ - 1 && mazeData[(int)coor.x, (int)coor.y + 1] == false){
			if(travel(mazeData, new Vector2 ((int)coor.x, (int)coor.y + 1))){
				return true;
			}
		}
		return false;		
	}
}
