using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets;

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

        // Addressable 설정
        SetAddressable(assetPath);

        // 선택
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    private static void SetAddressable(string assetPath)
    {
        // Addressable 설정을 위한 AddressableAssetSettings 객체 가져오기
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        if (!settings)
        {
            Debug.LogError("Addressable settings not found!");
            return;
        }

        var group = settings.FindGroup("Default Local Group");
        // 자산에 대해 GUID를 가져오고, Addressable로 이동
        string guid = AssetDatabase.AssetPathToGUID(assetPath);
        var entry = settings.CreateOrMoveEntry(guid, group);

        // Addressable 그룹에 자산을 추가 후, "EnemySO" 태그를 설정
        entry.SetLabel("EnemySO", true);

        // 저장 및 갱신
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
