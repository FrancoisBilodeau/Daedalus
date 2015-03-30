using UnityEngine;

public class MazesButtonBehavior : MonoBehaviour
{

    // Ce script est attaché aux 5 textes 3D de chaque page.
    enum Display
    {
        Nothing,
        InvalidMaze,
        InvalidTime
    }

    Display d = Display.Nothing;
    public GUISkin skin;

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.cyan;
    }
	
    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
	
    // Loader le labyrinthe choisi
    void OnMouseUp()
    {
        string mazeName = ((TextMesh)(GetComponent("TextMesh"))).text;
        var save = new SaveFile();
		
        SaveFileResult r = save.LoadFile(mazeName);

        if (r == SaveFileResult.MazeInvalid)
        {
            d = Display.InvalidMaze;
        }
        else if (r == SaveFileResult.TimeInvalid)
        {
            d = Display.InvalidTime;
        }
        else
        {
            PlayModeSingleton playMode = PlayModeSingleton.Instance;
            playMode.SaveFile = save;
            Application.LoadLevel(2);
        }
    }

    //Affichage des erreurs
    void OnGUI()
    {
        GUI.skin = skin;
        if (d == Display.InvalidMaze)
        {
            var errorBox = new Rect(20, 20, 224, 50);
            GUI.ModalWindow(0, errorBox, DoMyWindow, "Error: The maze file is invalid.");
        }
        else if (d == Display.InvalidTime)
        {
            var errorBox = new Rect(20, 20, 224, 50);
            GUI.ModalWindow(0, errorBox, DoMyWindow, "Error: The maze file has an invalid time.");
        }
    }

    void DoMyWindow(int windowID)
    {
        if (GUI.Button(new Rect(62, 20, 100, 20), "Ok"))
        {
            d = Display.Nothing;
        }
    }
}
