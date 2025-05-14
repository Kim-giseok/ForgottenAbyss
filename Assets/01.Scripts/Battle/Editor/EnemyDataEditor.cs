#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class EnemyTableEditor : EditorWindow
{
    [MenuItem("Tools/Enemy/StatTable")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<EnemyTableEditor>();
        wnd.titleContent = new GUIContent("Enemy Table");
    }

    private enum EnemyType { NightBone, SwordShadow, Archer, Agis }

    private readonly string[] statNames = { "HP", "MP", "Attack", "Speed" };
    private readonly Dictionary<(EnemyType, string), IntegerField> fields = new();

    private void CreateGUI()
    {
        // 전체 테이블
        var root = rootVisualElement;
        root.style.flexDirection = FlexDirection.Column;
        root.style.paddingTop = 10;
        root.style.paddingLeft = 10;

        // --- Header Row (Stat Names) ---
        var headerRow = new VisualElement { style = { flexDirection = FlexDirection.Row } };
        headerRow.Add(new Label("Enemy") { style = { width = 102, unityFontStyleAndWeight= FontStyle.Bold, height = 22 } });

        foreach (var stat in statNames)
        {
            headerRow.Add(new Label(stat) { style = { width = 106, unityFontStyleAndWeight = FontStyle.Bold, height = 20 } });
        }

        root.Add(headerRow);

        // --- Rows per Enemy ---
        foreach (EnemyType enemy in Enum.GetValues(typeof(EnemyType)))
        {
            var row = new VisualElement { style = { flexDirection = FlexDirection.Row, marginBottom = 4 } };
            row.Add(new Label(enemy.ToString()) { style = { width = 100, unityTextAlign = TextAnchor.MiddleLeft, height = 20 } });

            foreach (var stat in statNames)
            {
                var field = new IntegerField { style = { width = 100 } };
                fields[(enemy, stat)] = field;
                row.Add(field);
            }

            root.Add(row);
        }

        // --- Print Button ---
        var button = new Button(() =>
        {
            foreach (EnemyType enemy in Enum.GetValues(typeof(EnemyType)))
            {
                string line = $"{enemy}:";
                foreach (var stat in statNames)
                {
                    int value = fields[(enemy, stat)].value;
                    line += $" {stat}={value}";
                }
                Debug.Log(line);
            }
        })
        {
            text = "Print Stats to Console"
        };

        button.style.marginTop = 10;
        root.Add(button);
    }
}
#endif
