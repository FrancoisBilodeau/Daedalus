using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TileMapCreator))]
public class TileMapCreatorInspector : Editor {
	
	public override void OnInspectorGUI() {
		//base.OnInspectorGUI();
		DrawDefaultInspector();
		
		if(GUILayout.Button("Regenerate")) {
			TileMapCreator tileMap = (TileMapCreator)target;
			tileMap.BuildMesh();
		}
	}
}
