#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class EnemyTableEditor : EditorWindow
{
    private const string Path = "Assets/05.ScriptableObjects/Enemies/";
    private string GetPathByName(string fileName) => System.IO.Path.Combine(Path, fileName + ".asset");
    
    private void LoadData()
    {
        foreach (EnemyType enemy in Enum.GetValues(typeof(EnemyType)))
        {
            string path = GetPathByName($"{enemy}/StatSO");
            EnemyStatSO statSO = AssetDatabase.LoadAssetAtPath<EnemyStatSO>(path);

            if (statSO != null)
            {
                foreach (EnemyStatType stat in Enum.GetValues(typeof(EnemyStatType)))
                {
                    var field = _fields[(enemy, stat.ToString())];
                    var statInfo = statSO.stats.Find(info => info.statType == stat);
                    if (statInfo != null)
                    {
                        field.value = (int)statInfo.value;
                    }
                }
            }
            
            path = GetPathByName($"{enemy}/RewardSO");
            EnemyRewardSO rewardSO = AssetDatabase.LoadAssetAtPath<EnemyRewardSO>(path);
            
            if (rewardSO != null)
            { 
                _fields[(enemy, "Gold")].value = rewardSO.Gold;
                _fields[(enemy, "Experience")].value = rewardSO.Experience;
            }
            
        }
    }
    
    [MenuItem("Tools/EnemyStatTable")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<EnemyTableEditor>();
        wnd.titleContent = new GUIContent("Enemy Table");
    }

    private enum EnemyType { Agis, Archer, Ghost, GhostChild, Gunner, NightBone, SwordShadow, Wizard, MudEye, MudHand, Bringer, MoonStone }

    private readonly Dictionary<(EnemyType, string), IntegerField> _fields = new();

    private void CreateGUI()
    {
        // 전체 테이블
        var root = rootVisualElement;
        root.style.flexDirection = FlexDirection.Column;
        root.style.paddingTop = 10;
        root.style.paddingLeft = 10;

        // 헤더
        var headerRow = new VisualElement { style = { flexDirection = FlexDirection.Row } };
        headerRow.Add(new Label("Enemy") { style = { width = 102, unityFontStyleAndWeight= FontStyle.Bold, height = 22 } });

        foreach (EnemyStatType stat in Enum.GetValues(typeof(EnemyStatType)))
        {
            headerRow.Add(new Label(stat.ToString()) { style = { width = 106, unityFontStyleAndWeight = FontStyle.Bold, height = 20 } });
        }

        foreach (RewardType stat in Enum.GetValues(typeof(RewardType)))
        {
            headerRow.Add(new Label(stat.ToString()) { style = { width = 106, unityFontStyleAndWeight = FontStyle.Bold, height = 20 } });
        }
        
        root.Add(headerRow);

        // 테이블
        foreach (EnemyType enemy in Enum.GetValues(typeof(EnemyType)))
        {
            var row = new VisualElement { style = { flexDirection = FlexDirection.Row, marginBottom = 4 } };
            row.Add(new Label(enemy.ToString()) { style = { width = 100, unityTextAlign = TextAnchor.MiddleLeft, height = 20 } });

            // statSO
            foreach (EnemyStatType stat in Enum.GetValues(typeof(EnemyStatType)))
            {
                var field = new IntegerField { style = { width = 100 } };
                _fields[(enemy, stat.ToString())] = field;
                row.Add(field);
            }
            
            // rewardSO
            foreach (RewardType stat in Enum.GetValues(typeof(RewardType)))
            {
                var field = new IntegerField { style = { width = 100 } };
                _fields[(enemy, stat.ToString())] = field;
                row.Add(field);
            }
            root.Add(row);
        }
        
        // SO 생성 버튼
        var createButton = new Button(() =>
        {
            foreach (EnemyType enemy in Enum.GetValues(typeof(EnemyType)))
            {
                SOFileSystem.CreateSOAssetAtPath<EnemyStatSO>(GetPathByName($"{enemy}/StatSO"), (so) =>
                {
                    so.enemyName = enemy.ToString();
                    so.stats = new();
                    foreach (EnemyStatType stat in Enum.GetValues(typeof(EnemyStatType)))
                    {
                        int value = _fields[(enemy, stat.ToString())].value;
                        so.stats.Add(new EnemyStatInfo(stat, value));
                    }
                });

                SOFileSystem.CreateSOAssetAtPath<EnemyRewardSO>(GetPathByName($"{enemy}/RewardSO"), (so) =>
                {
                    so.enemyName = enemy.ToString();
                    so.Gold = _fields[(enemy, "Gold")].value;
                    so.Experience = _fields[(enemy, "Experience")].value;
                });
            }
        })
        {
            text = "Build",
            style =
            {
                marginTop = 10
            }
        };

        root.Add(createButton);
        
        // 데이터 로드
        LoadData();
    }
}
#endif
