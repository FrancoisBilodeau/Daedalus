using UnityEngine;

public class Countdown : MonoBehaviour
{

    static float totalTime = 3.0f;
    static float currentTime;
    static bool finished;
    public GUISkin skin;
	
    void Start()
    {	
        ((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("MouseLook"))).enabled = false;
        ((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("CharacterMotor"))).enabled = false;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            DecreaseTime();
        }
    }
	
    //Affichage du temps
    void OnGUI()
    {
        if (!finished)
        {
            GUI.skin = skin;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 200, 600, 600), "" + totalTime);
            GUI.depth = 1;
        }
    }
	
    // Calcul du temps
    static void DecreaseTime()
    {
        float delta = Time.deltaTime;
		
        currentTime += delta;
		
        if (currentTime >= 1)
        {
            if (totalTime - 1 <= 0)
            {
                totalTime = 0;          
                ((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("MouseLook"))).enabled = true;
                ((MonoBehaviour)(GameObject.Find("Main Camera").GetComponent("CharacterMotor"))).enabled = true;
                PlayModeSingleton playMode = PlayModeSingleton.Instance;
                playMode.Started = true;
                finished = true;
            }
            else
            {
                totalTime -= 1;
                currentTime = 0;
            }       
        }
    }
}
