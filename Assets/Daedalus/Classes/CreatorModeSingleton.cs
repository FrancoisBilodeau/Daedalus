public class CreatorModeSingleton
{

    const int WallHeight = 3; // Hauteur des murs du labyrinthe
    const float TileSize = 2.0f; // Tailles d'une tuile du labyrinthe
    Maze maze;

    static CreatorModeSingleton instance;
	
    CreatorModeSingleton()
    {
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

    public Maze getMaze()
    {
        return maze;
    }

    public void setMaze(Maze newMaze)
    {
        maze = newMaze;
    }

    public int getWallHeight()
    {
        return WallHeight;
    }
	
    public float getTileSize()
    {
        return TileSize;
    }
		
}
