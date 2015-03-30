using UnityEngine;
using System.IO;
using System;

public enum SaveFileResult
{
    Ok,
    InvalidName,
    TakenName,
    EmptyName,
    MazeInvalid,
    TimeInvalid
}


public class SaveFile
{
    
    Maze maze;
    TimeRecorded[] times;
    
    public SaveFile(Maze maze)
    {
        this.maze = maze;
        times = new TimeRecorded[10];
        for (int i = 0; i < 10; i++)
        {
            times [i] = new TimeRecorded("", new TimeSpan(0));
        }
    }
    
    public Maze Maze { get; set; }
    
    public SaveFile()
    {
        maze = new Maze();
        times = new TimeRecorded[10];
    }
    
    public SaveFileResult WriteMaze()
    {
        return WriteMaze(false);
    }
    
    public TimeRecorded getTimeRecorded(int i)
    {
        return times [i];
    }
    
    // Retourne l'état de l'écriture
    public SaveFileResult WriteMaze(bool overwrite)
    {
        
        if (maze.mazeName.LastIndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            return SaveFileResult.InvalidName;
        }
        
        if (!overwrite && File.Exists("./mazes/" + maze.mazeName + ".maze"))
        {
            return SaveFileResult.TakenName;
        }
        
        if (string.IsNullOrEmpty(maze.mazeName))
        {
            return SaveFileResult.EmptyName;
        }
        
        using (var writer = new StreamWriter("./mazes/" + maze.mazeName + ".maze"))
        {
            // Taille de format 000x000
            writer.WriteLine(maze.sizeX.ToString("D3") + "x" + maze.sizeZ.ToString("D3"));
            
            // Coordonnées du départ puis coordonnées d'arrivée de format (000,000)(000,000)
            writer.WriteLine(maze.start + maze.end);
            
            // Remplissage du mapData
            // Chaque ligne du fichier représente une "ligne" du labyrinthe
            for (int i = 0; i < maze.sizeX; i++)
            {
                for (int j = 0; j < maze.sizeZ; j++)
                {
                    if (maze.mapData [i, j])
                    {
                        writer.Write("1");
                    }
                    else
                    {
                        writer.Write("0");
                    }
                    // Fin de la ligne
                    if (j == maze.sizeZ - 1)
                    {
                        writer.WriteLine("");
                    }
                }
            }
            
            for (int i = 0; i < 10; i++)
            {
                writer.Write(times [i].PlayerName);
                writer.Write(":");
                writer.Write(times [i].Time.Ticks);
                writer.Write("\n");
            }
        }
        return SaveFileResult.Ok;
    }
    
    public SaveFileResult LoadFile(string selectedMaze)
    {
        maze = new Maze();
        maze.mazeName = selectedMaze;
        
        using (var reader = new StreamReader("./mazes/" + selectedMaze + ".maze"))
        {
            
            // Taille en x
            string wSizeX = "";
            for (int i=0; i < 3; i++)
            {
                wSizeX += reader.Read();
            }
            
            if (!uint.TryParse(wSizeX, out maze.sizeX))
            {
                return SaveFileResult.MazeInvalid;
            }
            if (maze.sizeX < 0 || maze.sizeX > 999)
            {
                return SaveFileResult.MazeInvalid;
            }
            
            // Échapper le 'x'
            if (reader.Read() != 'x')
            {
                return SaveFileResult.MazeInvalid;
            }
            
            // Taille en z
            string wSizeZ = "";
            for (int i=0; i < 3; i++)
            {
                wSizeZ += reader.Read();
            }
            if (!uint.TryParse(wSizeZ, out maze.sizeZ))
            {
                return SaveFileResult.MazeInvalid;
            }
            if (maze.sizeZ < 0 || maze.sizeZ > 999)
            {
                return SaveFileResult.MazeInvalid;
            }
            
            // Échapper le \n
            if (reader.ReadLine() != "")
            {
                return SaveFileResult.MazeInvalid;
            }
            
            // Échapper le '('
            if (reader.Read() != '(')
            {
                return SaveFileResult.MazeInvalid;
            }
            string wStartX = "";
            while (reader.Peek() != ',')
            { 
                wStartX += reader.Read();
            }
            if (float.Parse(wStartX) < 0 || float.Parse(wStartX) > maze.sizeX)
            {
                return SaveFileResult.MazeInvalid;
            }
            
            // Échapper le ','
            if (reader.Read() != ',')
            {
                return SaveFileResult.MazeInvalid;
            }
            
            string wStartZ = "";
            while (reader.Peek() != ')')
            { 
                wStartZ += reader.Read();
            }
            if (float.Parse(wStartZ) < 0 || float.Parse(wStartZ) > maze.sizeZ)
            {
                return SaveFileResult.MazeInvalid;
            }
            
            // Échapper le ')'
            if (reader.Read() != ')')
            {
                return SaveFileResult.MazeInvalid;
            }
            
            // Coordonnées de la tuile de départ
            maze.start = new Vector2(float.Parse(wStartX), float.Parse(wStartZ));
            
            // Échapper le '('
            if (reader.Read() != '(')
            {
                return SaveFileResult.MazeInvalid;
            }
            
            string wEndX = "";
            while (reader.Peek() != ',')
            {
                wEndX += reader.Read();
            }
            if (float.Parse(wStartX) < 0 || float.Parse(wStartX) > maze.sizeX)
            {
                return SaveFileResult.MazeInvalid;
            }
            // Échapper le ','
            if (reader.Read() != ',')
            {
                return SaveFileResult.MazeInvalid;
            }
            
            string wEndZ = "";
            while (reader.Peek() != ')')
            {
                wEndZ += reader.Read();
            }
            if (float.Parse(wStartX) < 0 || float.Parse(wStartX) > maze.sizeZ)
            {
                return SaveFileResult.MazeInvalid;
            }
            
            // Échapper le ')' puis changer de ligne
            if (reader.ReadLine() != ")")
            {
                return SaveFileResult.MazeInvalid;
            }
            
            //Coordonnées de la tuile de fin
            maze.end = new Vector2(float.Parse(wEndX), float.Parse(wEndZ));
            
            if (maze.start == maze.end)
            {
                return SaveFileResult.MazeInvalid;
            }
            
            var mazeData = new bool[maze.sizeX, maze.sizeZ];
            
            // Lecture du mapData
            for (int i=0; i < maze.sizeX; i++)
            {
                for (int j=0; j < maze.sizeZ; j++)
                {
                    if (reader.Peek() == '1')
                    {
                        reader.Read();
                        mazeData [i, j] = true;
                    }
                    else if (reader.Peek() == '0')
                    {
                        reader.Read();
                        mazeData [i, j] = false;
                    }
                    else
                    {
                        return SaveFileResult.MazeInvalid;
                    }

                    if (j == maze.sizeX - 1)
                    {
                        if (reader.ReadLine() != "")
                        {
                            return SaveFileResult.MazeInvalid;
                        }
                    }
                }
            }
            maze.mapData = mazeData;
            
            // La liste des temps
            for (int i = 0; i<10; i++)
            {
                string nameTemp = "";
                long timeTemp;
                
                // Nom
                while (reader.Peek() != ':')
                {
                    nameTemp += reader.Read();
                }
                // Échapper le ':'
                if (reader.Read() != ':')
                {
                    return SaveFileResult.TimeInvalid;
                }
                // Prendre le temps
                if (!long.TryParse(reader.ReadLine(), out timeTemp))
                {
                    return SaveFileResult.TimeInvalid;
                }
                
                // Ajout au tableau
                times [i] = new TimeRecorded(nameTemp, new TimeSpan(timeTemp));
            }
        }
        
        return SaveFileResult.Ok;
    }
    
    // Ajoute le temps dans le tableau des meilleurs temps
    public void AddBestTime(TimeRecorded time)
    {
        TimeRecorded timeTemp = time;
        for (int i = 0; i < 10; i++)
        {
            if (timeTemp.Time < times [i].Time || times [i].Time.Ticks == 0)
            {
                TimeRecorded temp = timeTemp;
                timeTemp = times [i];
                times [i] = temp;
            }
        }
        WriteMaze();
    }
    
    // Teste si le temps fait partie des meilleurs temps
    public bool IsInBestTime(TimeSpan time)
    {
        for (int i = 0; i < 10; i++)
        {
            if (time < times [i].Time || times [i].Time.Ticks == 0)
            {
                return true;
            }
        }
        return false;
    }
    
}
