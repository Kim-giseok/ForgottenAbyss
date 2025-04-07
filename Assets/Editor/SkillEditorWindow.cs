using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class SkillEditorWindow : EditorWindow
{
    private SkillVisualSO selectedSkillVisualSO;
    private SkillDataList skillDataList;
    private SkillData currentSkillData;

    [MenuItem("Tools/Skill Editor")]
    public static void ShowWindow()
    {
        GetWindow<SkillEditorWindow>("Skill Editor");
    }

    private void OnEnable()
    {
        string path = Path.Combine(Application.dataPath, "Resources/Json/SkillData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            skillDataList = JsonUtility.FromJson<SkillDataList>(json);
        }
        else
        {
            Debug.LogError("SkillData.json not found in Resources/Json.");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Skill Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Skill"))
        {
            AddNewSkill();
        }

        selectedSkillVisualSO = (SkillVisualSO)EditorGUILayout.ObjectField("Skill Visual SO", selectedSkillVisualSO, typeof(SkillVisualSO), false);

        if (selectedSkillVisualSO != null)
        {
            string soName = selectedSkillVisualSO.name;
            currentSkillData = skillDataList.Skills.FirstOrDefault(s => s.VisualSOName == soName);

            if (currentSkillData != null)
            {
                currentSkillData.Name = EditorGUILayout.TextField("Skill Name", currentSkillData.Name);
                currentSkillData.Description = EditorGUILayout.TextField("Description", currentSkillData.Description);
                currentSkillData.CoolTime = EditorGUILayout.FloatField("CoolTime", currentSkillData.CoolTime);
                currentSkillData.DamageMultiplier = EditorGUILayout.FloatField("Damage Multiplier", currentSkillData.DamageMultiplier);
                currentSkillData.Type = (SkillType)EditorGUILayout.EnumPopup("Skill Type", currentSkillData.Type);
            }
            else
            {
                EditorGUILayout.HelpBox("No matching SkillData found for this SO name.", MessageType.Warning);
            }

            EditorGUILayout.Space();
            selectedSkillVisualSO.skillEffectPrefab = (GameObject)EditorGUILayout.ObjectField("Effect Prefab", selectedSkillVisualSO.skillEffectPrefab, typeof(GameObject), false);
            selectedSkillVisualSO.skillSound = (AudioClip)EditorGUILayout.ObjectField("Skill Sound", selectedSkillVisualSO.skillSound, typeof(AudioClip), false);

            if (GUILayout.Button("Save SkillData.json"))
            {
                SaveSkillDataJson();
                Debug.Log("SkillData.json saved.");
            }

            EditorUtility.SetDirty(selectedSkillVisualSO);
        }
    }

    private void SaveSkillDataJson()
    {
        string json = JsonUtility.ToJson(skillDataList, true);
        File.WriteAllText(Path.Combine(Application.dataPath, "Resources/Json/SkillData.json"), json);
        AssetDatabase.Refresh();
    }

    private void AddNewSkill()
    {
        int newId = skillDataList.Skills.Count > 0 ? skillDataList.Skills.Max(s => s.Id) + 1 : 0;
        string newSkillName = $"NewSkill_{newId}";

        // 1. Json 데이터 추가
        SkillData newSkillData = new SkillData
        {
            Id = newId,
            Name = newSkillName,
            Description = "New skill description",
            CoolTime = 1f,
            DamageMultiplier = 1f,
            VisualSOName = newSkillName
        };

        skillDataList.Skills.Add(newSkillData);
        SaveSkillDataJson();

        // 2. SkillVisualSO 생성
        string visualPath = "Assets/Resources/Skill/Visual";
        if (!Directory.Exists(visualPath))
            Directory.CreateDirectory(visualPath);

        SkillVisualSO newVisualSO = ScriptableObject.CreateInstance<SkillVisualSO>();
        AssetDatabase.CreateAsset(newVisualSO, Path.Combine(visualPath, newSkillName + ".asset"));

        // 3. SkillExecutionSO 생성
        string executionPath = "Assets/Resources/Skill/Execution";
        if (!Directory.Exists(executionPath))
            Directory.CreateDirectory(executionPath);

        SkillExecutionSO newExecutionSO = ScriptableObject.CreateInstance<SkillExecutionSO>();
        AssetDatabase.CreateAsset(newExecutionSO, Path.Combine(executionPath, newSkillName + "_Execution.asset"));

        // 4. VisualSO에 ExecutionSO 연결
        newVisualSO.executionSO = newExecutionSO;
        EditorUtility.SetDirty(newVisualSO);

        // 5. 마무리
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorGUIUtility.PingObject(newVisualSO);
        Selection.activeObject = newVisualSO;
        selectedSkillVisualSO = newVisualSO;

        Debug.Log($"Created new skill: {newSkillName} (Visual + Execution)");
    }
}