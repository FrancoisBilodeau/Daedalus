public class CreatorModeSingleton
{

    const int wallHeight = 3; // Hauteur des murs du labyrinthe
    const float tileSize = 2.0f; // Taille d'une tuile du labyrinthe
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

    public Maze Maze { get; set; }

    public int WallHeight { get; set; }
	
    public float TileSize { get; set; }
		
}
