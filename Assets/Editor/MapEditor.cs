using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor {
    public override void OnInspectorGUI()
    {
      
        // base.OnInspectorGUI();
        if (DrawDefaultInspector()) {
            MapGenerator map = target as MapGenerator;
            map.GenerateMap();
           
        }
        if (GUILayout.Button("GenerateMap")) {
            MapGenerator map = target as MapGenerator;
            map.GenerateMap();
        }
    }
}
