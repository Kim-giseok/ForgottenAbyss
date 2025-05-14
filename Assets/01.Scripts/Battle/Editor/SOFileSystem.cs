using System;
using UnityEditor;
using UnityEngine;

public class SOFileSystem
{
    public static void CreateSOAssetAtPath<T>(string assetPath, Action<T> initializer = null) where T : ScriptableObject
    {
        // 생성 및 초기 데이터 등록
        T asset = ScriptableObject.CreateInstance<T>();
        initializer?.Invoke(asset);

        // 폴더 없으면 생성
        string folderPath = System.IO.Path.GetDirectoryName(assetPath);
        if (!AssetDatabase.IsValidFolder(folderPath)) { System.IO.Directory.CreateDirectory(folderPath!); }
        
        // 이미 있으면 삭제
        if (AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath)) { AssetDatabase.DeleteAsset(assetPath); }


        // 저장
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 선택
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }   
}