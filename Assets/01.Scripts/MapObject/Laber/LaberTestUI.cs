using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

[CustomEditor(typeof(LaberDamagerble))]
public class LaberTestUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("레버 작동"))
        {
            ((LaberDamagerble)target).GetDamage(10);
        }
    }
}
