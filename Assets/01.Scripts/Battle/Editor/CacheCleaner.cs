#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class CacheCleaner : EditorWindow
{
    [MenuItem("Tools/Clear Unity Cache")]
    public static void ClearUnityCache()
    {
        var pathsToDelete = new string[]
        {
            "Library/ShaderCache",
            "Library/ScriptAssemblies",
            "Temp"
        };

        foreach (var path in pathsToDelete)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Debug.Log($"Deleted: {path}");
            }
        }

        EditorUtility.DisplayDialog("Cache Cleared", "Selected cache folders have been deleted.", "OK");
    }
}
#endif