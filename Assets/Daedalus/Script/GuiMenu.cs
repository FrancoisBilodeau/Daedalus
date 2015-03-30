using UnityEngine;

public class GuiMenu : MonoBehaviour
{
	
    public GUISkin skin;
    GameObject cameraMenu;
    bool isButtonVisible;
	
    void OnGUI()
    {
		
        GUI.skin = skin;
		
        if (isButtonVisible && GUI.Button(new Rect(Screen.width - 125, Screen.height - 50, 125, 50), "Return to menu"))
        {
            cameraMenu = GameObject.Find("Camera");
            // Inverser l'animation pour revenir au main menu
            cameraMenu.GetComponent<Animation>() ["cameranim"].speed = -1;
            cameraMenu.GetComponent<Animation>() ["cameranim"].time = cameraMenu.GetComponent<Animation>() ["cameranim"].length;
            isButtonVisible = false;
            cameraMenu.GetComponent<Animation>().Play();
        }
    }
	
    public bool IsButtonVisible { get; set; }
	
    public GuiMenu()
    {
        isButtonVisible = false;
    }
	
	
}
