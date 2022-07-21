using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InitializeScene))]
public class InitializeSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InitializeScene initScene = (InitializeScene)target;

        if (GUILayout.Button("Generate Map"))
        {
            initScene.ClearMap();
            initScene.GenerateMap();
            initScene.GenerateWalls();
            initScene.GenerateItems();
        }
    }   
}
