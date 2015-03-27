public class PlayModeSingleton
{

    const int WallHeight = 3; // Hauteur des murs du labyrinthe
    const float TileSize = 2.0f; // Tailles d'une tuile du labyrinthe

    static PlayModeSingleton instance;
    SaveFile saveFile;
    bool started;

    PlayModeSingleton()
    {
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
	
    public SaveFile getSaveFile()
    {
        return saveFile;
    }

    public void setSaveFile(SaveFile newSaveFile)
    {
        saveFile = newSaveFile;
    }

    public bool hasStarted()
    {
        return started;
    }

    public void setStarted(bool isStarted)
    {
        started = isStarted;
    }

    public float getTileSize()
    {
        return TileSize;
    }
}
