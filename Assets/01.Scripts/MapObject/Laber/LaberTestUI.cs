using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

[CustomEditor(typeof(LaberInteractive))]
public class LaberTestUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("레버 작동"))
        {
            ((LaberInteractive)target).ActiveInteraction();
        }
    }
}
