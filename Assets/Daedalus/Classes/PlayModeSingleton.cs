public class PlayModeSingleton
{

    static PlayModeSingleton instance;

    PlayModeSingleton()
    {
        Started = new bool();
        Started = false;
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
	
    public SaveFile SaveFile { get; set; }

    public bool Started { get; set; }

    public float TileSize { get; set; }

    public int WallHeight { get; set; }
}
