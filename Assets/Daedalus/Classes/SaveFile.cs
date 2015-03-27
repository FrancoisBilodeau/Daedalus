using UnityEngine;
using System.Collections;
using System.IO;
using System;

public enum SaveFileResult {Ok, InvalidName, TakenName, EmptyName, MazeInvalid, TimeInvalid};

public class SaveFile{

	private Maze maze;
	private TimeRecorded[] times;

	public SaveFile(Maze maze){
		this.maze = maze;
		times = new TimeRecorded[10];
		for(int i = 0; i<10; i++){
			times[i] = new TimeRecorded("", new TimeSpan(0));
		}
	}

	public SaveFile(){
		maze = new Maze();
		times = new TimeRecorded[10];
	}
  
  public SaveFileResult writeMaze() {
    return writeMaze(false);
  }

	//Retourne l'état de l'écriture
	public SaveFileResult writeMaze (bool overwrite) {
		
		if(maze.mazeName.LastIndexOfAny(Path.GetInvalidFileNameChars()) != -1){
			return SaveFileResult.InvalidName;;
		}

		if(!overwrite && File.Exists("./mazes/" + maze.mazeName + ".maze")) {
			return SaveFileResult.TakenName;
		}
		
		if(maze.mazeName == "" || maze.mazeName == null) {
			return SaveFileResult.EmptyName;
		}

		using (StreamWriter writer = new StreamWriter("./mazes/" + maze.mazeName + ".maze"))
		{
			//Taille de format 000x000
			writer.WriteLine(maze.sizeX.ToString("D3") + "x" + maze.sizeZ.ToString("D3"));
			
			//Coordonnées du départ puis coordonnées d'arrivée de format (000,000)(000,000)
			writer.WriteLine(maze.start.ToString() + maze.end.ToString());
			
			//Remplissage du mapData
			//Chaque ligne du fichier représente une "ligne" du labyrinthe
			for(int i=0; i < maze.sizeX; i++) {
				for(int j=0; j < maze.sizeZ; j++) {
					if(maze.mapData[i,j] == true) {
						writer.Write ("1");
					}
					else {
						writer.Write ("0");
					}
					//Fin de la ligne
					if(j == maze.sizeZ-1) {
						writer.WriteLine ("");
					}
				}
			}

			for(int i=0; i<10; i++){
				writer.Write (times[i].getPlayerName());
				writer.Write (":");
				writer.Write (times[i].getTime().Ticks.ToString());
				writer.Write ("\n");
			}
		}
		return SaveFileResult.Ok;
	}
	
	public SaveFileResult loadFile (string selectedMaze) {
		maze = new Maze();
		maze.mazeName = selectedMaze;
		string wSizeX = "";
		string wSizeZ = "";
		string wStartX = "";
		string wStartZ = "";
		string wEndX = "";
		string wEndZ = "";

		using (StreamReader reader = new StreamReader("./mazes/" + selectedMaze + ".maze")) {

			//Taille en x
			for(int i=0; i < 3; i++) {
				wSizeX += (char)(reader.Read());
			}

			//maze.sizeX = uint.Parse(wSizeX);
			if(!uint.TryParse(wSizeX, out maze.sizeX)) {
				return SaveFileResult.MazeInvalid;
			}
			if(maze.sizeX < 0 || maze.sizeX > 999) {
				return SaveFileResult.MazeInvalid;
			}

			//Échapper le 'x'
			if(reader.Read() != 'x') {
				return SaveFileResult.MazeInvalid;
			}
			
			//Taille en z
			for(int i=0; i < 3; i++) {
				wSizeZ += (char)reader.Read();
			}
			//maze.sizeZ = uint.Parse(wSizeZ);
			if(!uint.TryParse(wSizeZ, out maze.sizeZ)) {
				return SaveFileResult.MazeInvalid;
			}
			if(maze.sizeZ < 0 || maze.sizeZ > 999) {
				return SaveFileResult.MazeInvalid;
			}

			//Échapper le \n
			if(reader.ReadLine() != "") {
				return SaveFileResult.MazeInvalid;
			}
			
			//Échapper le '('
			if(reader.Read() != '(') {
				return SaveFileResult.MazeInvalid;
			}
			while((char)reader.Peek() != ','){ 
				wStartX += (char)reader.Read();
			}
			if(float.Parse(wStartX) < 0 || float.Parse(wStartX) > maze.sizeX) {
				return SaveFileResult.MazeInvalid;
			}
			
			//Échapper le ','
			if(reader.Read() != ',') {
				return SaveFileResult.MazeInvalid;
			}
			
			while((char)reader.Peek() != ')'){ 
				wStartZ += (char)reader.Read();
			}
			if(float.Parse(wStartZ) < 0 || float.Parse(wStartZ) > maze.sizeZ) {
				return SaveFileResult.MazeInvalid;
			}
			
			//Échapper le ')'
			if(reader.Read() != ')') {
				return SaveFileResult.MazeInvalid;
			}
			
			//Coordonnées de la tuile de départ
			maze.start = new Vector2(float.Parse(wStartX),float.Parse(wStartZ));
			
			//Échapper le '('
			if(reader.Read() != '(') {
				return SaveFileResult.MazeInvalid;
			}
			
			while((char)reader.Peek() != ','){
				wEndX += (char)reader.Read();
			}
			if(float.Parse(wStartX) < 0 || float.Parse(wStartX) > maze.sizeX) {
				return SaveFileResult.MazeInvalid;
			}
			//Échapper le ','
			if(reader.Read() != ',') {
				return SaveFileResult.MazeInvalid;
			}
			
			while((char)reader.Peek() != ')'){
				wEndZ += (char)reader.Read();
			}
			if(float.Parse(wStartX) < 0 || float.Parse(wStartX) > maze.sizeZ) {
				return SaveFileResult.MazeInvalid;
			}

			//Échapper le ')' puis changer de ligne
			if(reader.ReadLine() != ")") {
				return SaveFileResult.MazeInvalid;
			}

			//Coordonnées de la tuile de fin
			maze.end = new Vector2(float.Parse(wEndX),float.Parse(wEndZ));

			if(maze.start == maze.end) {
				return SaveFileResult.MazeInvalid;
			}
	
			bool[,] mazeData = new bool[maze.sizeX, maze.sizeZ];
			//Lecture du mapData
			for(int i=0; i < maze.sizeX; i++) {
				for(int j=0; j < maze.sizeZ; j++) {
					if((char)reader.Peek() == '1') {
						reader.Read();
						mazeData[i,j] = true;
					} else if ((char)reader.Peek() == '0'){
						reader.Read();
						mazeData[i,j] = false;
					} else {
						return SaveFileResult.MazeInvalid;
					}
					if(j == maze.sizeX-1) {
						if(reader.ReadLine() != "") {
							return SaveFileResult.MazeInvalid;
						}
					}
				}
			}
			maze.mapData = mazeData;
			//La liste des Temps
			for(int i = 0; i<10;i++){
				string nameTemp = "";
				long timeTemp;
				//get the name
				while((char)reader.Peek() != ':'){
					nameTemp += (char)reader.Read();
				}
				//Échapper le ':'
				if(reader.Read() != ':') {
					return SaveFileResult.TimeInvalid;
				}
				//prendre le temps
				if(!long.TryParse(reader.ReadLine(), out timeTemp)) {
					return SaveFileResult.TimeInvalid;
				}
				//timeTemp = long.Parse(reader.ReadLine());
				//ajout au tableau
				times[i] = new TimeRecorded(nameTemp, new TimeSpan(timeTemp));
			}
		}
		
		return SaveFileResult.Ok;
	}

	// ajoute le temps dans le tableau des meilleurs temps
	public void addBestTime(TimeRecorded time) {
		TimeRecorded timeTemp = time;
		for (int i = 0; i< 10; i++){
			if(timeTemp.getTime() < times[i].getTime()|| times[i].getTime().Ticks == 0){
				TimeRecorded temp = timeTemp;
				timeTemp = times[i];
				times[i] = temp;
			}
		}
		writeMaze();
	}

	// teste si le temps fait partie des meilleurs temps
	public bool isInBestTime(TimeSpan time){
		for (int i = 0; i< 10; i++){
			if(time < times[i].getTime() || times[i].getTime().Ticks == 0 ){
				return true;
			}
		}
			return false;
	}

	public Maze getMaze(){
		return maze;
	}

	public TimeRecorded getTimeRecorded(int i){
		return times[i];
	}

}
