using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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
#endif