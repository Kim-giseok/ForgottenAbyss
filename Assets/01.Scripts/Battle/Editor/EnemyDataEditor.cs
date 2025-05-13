#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTableEditor : EditorWindow
{
    [MenuItem("Tools/Enemy/StatTable")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<EnemyTableEditor>();
        wnd.titleContent = new GUIContent("Enemy Table");
    }

    private void CreateGUI()
    {
        var root = rootVisualElement;

        // 헤더
        var header = new VisualElement();
        header.style.flexDirection = FlexDirection.Row;

        header.Add(new Label("Monster Name") { style = { width = 150 } });
        header.Add(new Label("HP") { style = { width = 100 } });
        root.Add(header);

        // 데이터 입력 행
        var nameField = new TextField() { label = "", style = { width = 150 } };
        var hpField = new IntegerField() { label = "", style = { width = 100 } };

        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.Add(nameField);
        row.Add(hpField);
        root.Add(row);

        // 출력 버튼
        var button = new Button(() =>
        {
            string name = nameField.value;
            int hp = hpField.value;
            Debug.Log($"몬스터 이름: {name}, 체력: {hp}");
        })
        {
            text = "Print to Console"
        };

        root.Add(button);
    }
}
#endif