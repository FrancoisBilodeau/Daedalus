using UnityEngine;

public class PlayCamera : MonoBehaviour
{

    PlayModeSingleton playMode = PlayModeSingleton.Instance;

    void Start()
    {
        Cursor.visible = false;

        //create vector and get main camera
        Vector2 start = playMode.SaveFile.Maze.start;
        Vector3 cameraCoor = Camera.main.transform.position;

        // set position to the start point
        cameraCoor.x = (start.y + 1) * playMode.TileSize + playMode.TileSize / 2f;
        cameraCoor.y = 1.4f;
        cameraCoor.z = (start.x + 1) * playMode.TileSize + playMode.TileSize / 2f;

        //set the position to the camera
        Camera.main.transform.position = cameraCoor;
    }

}
