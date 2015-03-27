using UnityEngine;
using System.Collections;

public class PlayCamera : MonoBehaviour {

	private PlayModeSingleton playMode = PlayModeSingleton.Instance;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;

		//create vector and get main camera
		Vector2 start = playMode.getSaveFile().getMaze().start;
		Vector3 cameraCoor = Camera.main.transform.position;

		// set position to the start point
		cameraCoor.x = (start.y+1) *playMode.tileSize + playMode.tileSize/2f;
		cameraCoor.y = 1.4f;
		cameraCoor.z = (start.x+1) *playMode.tileSize + playMode.tileSize/2f;

		//set the position to the camera
		Camera.main.transform.position = cameraCoor;

	}
}
