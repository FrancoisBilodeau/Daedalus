using UnityEngine;

public class Maze
{

    // Nom du labyrinthe
    public string mazeName;

    // Grandeur par défaut du labyrinthe
    public uint sizeX = 64;
    public uint sizeZ = 64;

    // Points de départ et d'arrivée
    public Vector2 start;
    public Vector2 end;

    // Données de la grille qui spécifient les trous et les murs
    // true = mur, false = vide
    public bool[,] mapData;

    public Maze()
    {

        mazeName = "";
        sizeX = 64;
        sizeZ = 64;
        start = new Vector2(0, 0);
        end = new Vector2(0, 0);
        mapData = new bool[64, 64];
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                mapData [i, j] = true;
            }
        }
    }

    public Maze(Maze maze)
    {
        this.mazeName = maze.mazeName;
        this.sizeX = maze.sizeX;
        this.sizeZ = maze.sizeZ;
        this.start = maze.start;
        this.end = maze.end;
        this.mapData = maze.mapData;
    }	
}
