using UnityEngine;

public class CameraAnimation : MonoBehaviour
{

    //Animation créée à l'aide de l'Animation Tool de Unity 
    public void PlayAnimation()
    {
        GetComponent<Animation>().Play();
    }

}
