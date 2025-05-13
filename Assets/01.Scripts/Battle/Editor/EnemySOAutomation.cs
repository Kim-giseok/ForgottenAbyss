#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemySOEditorAutomation : EditorWindow
{
    [MenuItem("Tools/Enemy/Update EnemySO Names")]
    public static void AutoSetEnemyNameBasedOnFolder()
    {
        string[] guids = AssetDatabase.FindAssets("t:EnemySO");

        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid); // GUID로 경로 얻기
            EnemySO enemySO = AssetDatabase.LoadAssetAtPath<EnemySO>(assetPath); // 에셋 로드

            if (enemySO)
            {
                string folderPath = Path.GetDirectoryName(assetPath);
                string folderName = Path.GetFileName(folderPath);

                enemySO.enemyName = folderName;
                EditorUtility.SetDirty(enemySO);
            }
        }
    }
}

#endif