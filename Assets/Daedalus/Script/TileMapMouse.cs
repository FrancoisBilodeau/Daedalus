using UnityEngine;

public enum CursorState
{
    draw,
    start,
    end,
    none
}

public enum ErrorType
{
    Start,
    End,
    Inc,
    NameTaken,
    NameWrong,
    NameEmpty,
    None
}

[RequireComponent(typeof(TileMapCreator))]
public class TileMapMouse : MonoBehaviour
{

    static CreatorModeSingleton creatorMode = CreatorModeSingleton.Instance;
    static CursorState state = CursorState.draw;
    GUISkin skin = null;
    static TileMapCreator tileMap;

    static Vector3 currentTileCoord;

    //bool pour ouvrir les GUI.Window	
    static bool savePopup;
    static bool helpPopup;
    static bool quitPopup;
    static bool confirm;
    static ErrorType err = ErrorType.None;
    Transform selectionCube = null;

    static string nom = "";
	
    void Start()
    {
        tileMap = GetComponent<TileMapCreator>();
        Maze maze = creatorMode.Maze;
        maze.start = new Vector2(-1, -1);
        maze.end = new Vector2(-1, -1);
    }

    // Update is called once per frame
    void Update()
    {
        Maze maze = creatorMode.Maze;

        //Camera Zoom
        Zooming();

        //Camera Move
        MoveCamera();
			
        //Mouse collider
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        //Highlight
        if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            int x = Mathf.FloorToInt(hitInfo.point.x / creatorMode.TileSize);
            int z = Mathf.FloorToInt(hitInfo.point.z / creatorMode.TileSize);

            Vector3 currentTile;
            currentTile.x = x + 0.5f;
            currentTile.y = 1f;
            currentTile.z = z + 0.5f;
            selectionCube.position = currentTile * creatorMode.TileSize;
        }
        else
        {
            //Cacher le highlight.
            Vector3 currentTile;
            currentTile.x = 10f;
            currentTile.y = -1f;
            currentTile.z = 10f;
            selectionCube.position = currentTile * creatorMode.TileSize;
        }

        //Tant que le Click DROIT est enfoncé
        if (Input.GetMouseButton(1) && GUIUtility.hotControl == 0)
        {
            if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                int x = Mathf.FloorToInt(hitInfo.point.x / creatorMode.TileSize - 1);
                int z = Mathf.FloorToInt(hitInfo.point.z / creatorMode.TileSize - 1);

                //Vérif de l'état, si c'est pas un mur de CONTOUR, et s'il y a déja un mur
                if (((x > -1 && z > -1) && (x < (maze.sizeX) && z < (maze.sizeZ))) && state == CursorState.draw && !maze.mapData [z, x])
                {
                    AddWall(x, z, maze);
                }
                //Vérif de l'état et si c'est pas un mur de CONTOUR
                if (((x > -1 && z > -1) && (x < (maze.sizeX) && z < (maze.sizeZ))) && state == CursorState.start)
                {
                    PutStart(x, z, maze);
                }
                //Vérif de l'état et si c'est pas un mur de CONTOUR
                if (((x > -1 && z > -1) && (x < (maze.sizeX) && z < (maze.sizeZ))) && state == CursorState.end)
                {
                    PlaceEnd(x, z, maze);
                }
            }
        }

        //Tant que le Click GAUCHE est enfoncé
        if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
        {
            if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                int x = Mathf.FloorToInt(hitInfo.point.x / creatorMode.TileSize - 1);
                int z = Mathf.FloorToInt(hitInfo.point.z / creatorMode.TileSize - 1);
                //Vérif si c'est pas un mur de CONTOUR, du state et s'il n'y a pas de mur
                if (((x > -1 && z > -1) && (x < (maze.sizeX) && z < (maze.sizeZ))) && state == CursorState.draw && maze.mapData [z, x])
                    RemoveWall(x, z, maze);
                //Vérif si c'est pas un mur de CONTOUR et du state
                if (((x > -1 && z > -1) && (x < (maze.sizeX) && z < (maze.sizeZ))) && state == CursorState.start)
                    PutStart(x, z, maze);
                //Vérif si c'est pas un mur de CONTOUR et du state
                if (((x > -1 && z > -1) && (x < (maze.sizeX) && z < (maze.sizeZ))) && state == CursorState.end)
                    PlaceEnd(x, z, maze);
            }
        }
    }

    void OnGUI()
    {
        GUI.skin = skin;
        Maze maze = creatorMode.Maze;

        //Bouton qui ouvre la fenetre Quit
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 50, 100, 50), "Return"))
        {
            quitPopup = true;
            state = CursorState.none;
        }

        //Bouton qui ouvre la fenetre Save & Quit
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 100, 50), "Save & Quit"))
        {
            savePopup = true;
            state = CursorState.none;
        }

        //Bouton qui vide la grille
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 150, 100, 50), "Clear"))
        {
            if (state != CursorState.none)
                Empty(maze);
        }

        //Bouton qui change le state a END pour pouvoir ajouter la case END
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 200, 100, 50), "Add End"))
        {
            if (state != CursorState.none)
                state = CursorState.end;
        }

        //Bouton qui change le state a START pour pouvoir ajouter la case START
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 250, 100, 50), "Add Start"))
        {
            if (state != CursorState.none)
                state = CursorState.start;
        }

        //Bouton qui rend les murs des trous, et les trous des murs
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 300, 100, 50), "Invert"))
        {
            if (state != CursorState.none)
                InvertGrille(maze);
        }
		
        //Bouton qui ouvre la fenetre d'aide
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 350, 100, 50), "Help"))
        {
            helpPopup = true;
            state = CursorState.none;
        }

        //Fenetre de save and quit
        if (savePopup)
        {
            Rect windowMaze = new Rect((Screen.width / 2) - 165, (Screen.height / 2) - 120, 330, 120);
            windowMaze = GUI.Window(0, windowMaze, DoMyWindow, "SAVE & QUIT");				
        }

        //Fenetre d'erreur
        if (err != ErrorType.None)
        {
            Rect windowErr = new Rect((Screen.width / 2) - 165, (Screen.height / 2) - 120, 330, 120);
            windowErr = GUI.Window(0, windowErr, DoMyWindowErr, "ERROR");				
        }
		
        //Fenetre d'aide
        if (helpPopup)
        {
            Rect windowHelp = new Rect((Screen.width / 2) - 165, (Screen.height / 2) - 120, 330, 310);
            windowHelp = GUI.Window(0, windowHelp, DoMyWindowHelp, "HELP");				
        }

        //Fenetre de sortie
        if (quitPopup)
        {
            Rect windowQuit = new Rect((Screen.width / 2) - 165, (Screen.height / 2) - 120, 310, 150);
            windowQuit = GUI.Window(0, windowQuit, DoMyWindowQuit, "QUIT");				
        }
        //Sauvegarde dans le cas qu'on veut écraser l'ancienne sauvegarde d'un labyrinthe
        if (confirm)
        {
            maze.mazeName = name;
            SaveFile save = new SaveFile(maze);
            save.WriteMaze(true);
            Application.LoadLevel(0);
        }
    }

    static void Zooming()
    {
        float minFov = 10f;
        float maxFov = 70f;
        float sensitivity = 8.0f;
        float fov = Camera.main.fieldOfView;
		
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
	
    void AddWall(int x, int z, Maze maze)
    {
        currentTileCoord.x = x;
        currentTileCoord.z = z;
        maze.mapData [z, x] = true;
        tileMap.BuildMesh();
    }
	
    static void RemoveWall(int x, int z, Maze maze)
    {
        currentTileCoord.x = x;
        currentTileCoord.z = z;
        maze.mapData [z, x] = false;
        tileMap.BuildMesh();
    }
	
    static void PutStart(int x, int z, Maze maze)
    {
        currentTileCoord.x = x;
        currentTileCoord.z = z;
        maze.start = new Vector2(z, x);
        maze.mapData [z, x] = false;
        state = CursorState.draw;
        tileMap.BuildMesh();
		
    }
	
    static void PlaceEnd(int x, int z, Maze maze)
    {
        currentTileCoord.x = x;
        currentTileCoord.z = z;
        maze.end = new Vector2(z, x);
        maze.mapData [z, x] = false;
        state = CursorState.draw;
        tileMap.BuildMesh();
    }

    // Enlève tous les murs
    static void Empty(Maze maze)
    {
        for (int i = 0; i < 64; i++)
        {
            for (int k = 0; k < 64; k++)
            {
                maze.mapData [k, i] = false;
            }
        }
        maze.start = new Vector2(-1, -1);
        maze.end = new Vector2(-1, -1);
        tileMap.BuildMesh();

    }

    //Methode qui inverse la grille
    static void InvertGrille(Maze maze)
    {
		
        for (int i = 0; i < 64; i++)
        {
            for (int k = 0; k < 64; k++)
            {
                if (!maze.mapData [k, i])
                {
                    maze.mapData [k, i] = true;					
                }
                else
                if (maze.mapData [k, i])
                {
                    maze.mapData [k, i] = false;	
                }
            }
        }
        maze.start = new Vector2(-1, -1);
        maze.end = new Vector2(-1, -1);
        tileMap.BuildMesh();
		
    }

    //Contenu de la fenetre Save & Quit
    static void DoMyWindow(int windowID)
    {

        Maze maze = creatorMode.Maze;
        Verification verif = new Verification(maze);

        GUI.Label(new Rect(10, 20, 500, 300), "  Please enter a name for this maze\n                  Maximum Size: 16");
        nom = GUI.TextField(new Rect(90, 60, 150, 20), nom, 16);
        if (GUI.Button(new Rect(90, 90, 150, 20), "Save"))
        {
            if (verif.VerifyMaze(maze) == Verification.VerificationResult.Ok)
            {
                maze.mazeName = nom;
                SaveFile save = new SaveFile(maze);
                SaveFileResult result = save.WriteMaze();
                if (result == SaveFileResult.InvalidName)
                {
                    err = ErrorType.NameWrong;
                }
                else if (result == SaveFileResult.TakenName)
                {
                    err = ErrorType.NameTaken;
                    Object obj = new Object();
                    obj.name = nom;
                }
                else if (result == SaveFileResult.EmptyName)
                {
                    err = ErrorType.NameEmpty;
                }
                else
                {
                    save.WriteMaze();
                    Application.LoadLevel(0);
                }
            }
            else if (verif.VerifyMaze(maze) == Verification.VerificationResult.InvalidStart)
            {
                err = ErrorType.Start;
            }
            else if (verif.VerifyMaze(maze) == Verification.VerificationResult.InvalidEnd)
            {
                err = ErrorType.End;
            }
            else
            {
                err = ErrorType.Inc;
            }
            savePopup = false;
        }			
    }

    // Contenu de la fenetre ERROR
    static void DoMyWindowErr(int windowID)
    {
        if (err == ErrorType.Start)
        {
            GUI.Label(new Rect(45, 20, 500, 300), "Error: Invalid starting tile");
            if (GUI.Button(new Rect(80, 90, 150, 20), "OK"))
            {
                err = ErrorType.None;
                state = CursorState.draw;
            }
        }
        else if (err == ErrorType.End)
        {
            GUI.Label(new Rect(45, 20, 500, 300), "Error: Invalid ending tile");
            if (GUI.Button(new Rect(80, 90, 150, 20), "OK"))
            {
                err = ErrorType.None;
                state = CursorState.draw;
            }
        }
        else if (err == ErrorType.Inc)
        {
            GUI.Label(new Rect(45, 20, 500, 300), "Error: Maze cannot be completed");
            if (GUI.Button(new Rect(80, 90, 150, 20), "OK"))
            {
                err = ErrorType.None;
                state = CursorState.draw;
            }
        }
        else if (err == ErrorType.NameWrong)
        {
            GUI.Label(new Rect(45, 20, 500, 300), "Error: Invalid name");
            if (GUI.Button(new Rect(80, 90, 150, 20), "OK"))
            {
                err = ErrorType.None;
                state = CursorState.draw;
            }
        }
        else if (err == ErrorType.NameTaken)
        {
            GUI.Label(new Rect(45, 20, 600, 300), "Error: Name already taken\n\n             Overwrite?");
            if (GUI.Button(new Rect(60, 90, 100, 20), "YES"))
            {
                confirm = true;
                err = ErrorType.None;
            }
            else if (GUI.Button(new Rect(170, 90, 100, 20), "NO"))
            {
                err = ErrorType.None;
                state = CursorState.draw;
            }
        }
        else if (err == ErrorType.NameEmpty)
        {
            GUI.Label(new Rect(45, 20, 500, 300), "Error: Name space is empty");
            if (GUI.Button(new Rect(80, 90, 150, 20), "OK"))
            {
                err = ErrorType.None;
                state = CursorState.draw;
            }
        }
    }
	
    //Contenu de la fenetre HELP
    static void DoMyWindowHelp(int windowID)
    {
        GUI.Label(new Rect(45, 20, 500, 300), "\nClick to create pathways\n\nRight-click to create walls\n\n" +
            "Mousewheel to zoom in/out\n\nMove camera with WASD\n or arrow keys\n\n" +
            "You need to make a completable\n maze by adding a Start tile\n and an End tile\n\n" +
            "Create a path to link them");
        if (GUI.Button(new Rect(80, 280, 150, 20), "OK"))
        {
            state = CursorState.draw;
            helpPopup = false;
        }
    }

    //Contenu de la fenetre Return
    static void DoMyWindowQuit(int windowID)
    {
        GUI.Label(new Rect(45, 20, 500, 300), "\nAre you sure you want to\nreturn to main menu?\n\nYour maze will not be saved.");

        if (GUI.Button(new Rect(60, 120, 50, 20), "YES"))
        {
            Application.LoadLevel(0);
        }

        if (GUI.Button(new Rect(160, 120, 50, 20), "NO"))
        {
            state = CursorState.draw;
            quitPopup = false;
        }
    }

    //Fonction pour bouger la camera avec wasd ou les fleches et établir les limites
    static void MoveCamera()
    {
        float xAxisValue = Input.GetAxisRaw("Horizontal");
        float zAxisValue = Input.GetAxisRaw("Vertical");
		
        if (Camera.main.transform.position.x < -1.0f)
        {
            xAxisValue = Mathf.Clamp(xAxisValue, 0, 1);
        }  
        if (Camera.main.transform.position.x > 135.0f)
        {
            xAxisValue = Mathf.Clamp(xAxisValue, -1, 0);
        }
        if (Camera.main.transform.position.z < -1.0f)
        {
            zAxisValue = Mathf.Clamp(zAxisValue, 0, 1);	
        }
        if (Camera.main.transform.position.z > 135.0f)
        {
            zAxisValue = Mathf.Clamp(zAxisValue, -1, 0);
        }
        if (Camera.main != null)
        {
            var CamSpeed = 2.0f;
            Camera.main.transform.Translate(new Vector3(xAxisValue * CamSpeed, zAxisValue * CamSpeed, 0.0f)); 		
        }
    }
	
}
