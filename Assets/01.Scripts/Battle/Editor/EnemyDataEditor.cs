#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TableEditorWindow : EditorWindow
{
    [MenuItem("Tools/Enemy Table Editor")]
    public static void ShowExample()
    {
        TableEditorWindow wnd = GetWindow<TableEditorWindow>();
        wnd.titleContent = new GUIContent("Enemy Table Editor");
    }

    private VisualElement root;
    
    private void CreateTable()
    {
        var table = new VisualElement();
        table.style.flexDirection = FlexDirection.Column;
        table.style.paddingTop = 10;
        table.style.paddingBottom = 10;
        table.style.paddingLeft = 10;
        table.style.paddingRight = 10;
        
        // 헤더 만들기
        var header = new VisualElement();
        header.style.flexDirection = FlexDirection.Row;
        header.style.alignItems = Align.Center;
        header.style.marginBottom = 5;

        // 각 헤더에 레이블 추가
        header.Add(new Label("Enemy Name") { style = { flexGrow = 1, width = 100 } });
        header.Add(new Label("Health") { style = { flexGrow = 1, width = 100 } });
        header.Add(new Label("Mana") { style = { flexGrow = 1, width = 100 } });

        table.Add(header);

        // 첫 번째 데이터 행 추가
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.alignItems = Align.Center;
        row.style.marginBottom = 5;

        // 각 셀에 텍스트 입력 필드 추가
        var enemyNameField = new TextField() { style = { flexGrow = 1, width = 100 } };
        var healthField = new TextField() { style = { flexGrow = 1, width = 100 } };
        var manaField = new TextField() { style = { flexGrow = 1, width = 100 } };

        // 기본값 설정 (선택사항)
        enemyNameField.value = "Dragon";
        healthField.value = "100";
        manaField.value = "50";

        row.Add(enemyNameField);
        row.Add(healthField);
        row.Add(manaField);

        table.Add(row);

        root.Add(table);
    }

    // Window에서 UI를 초기화하는 함수
    public void CreateGUI()
    {
        root = new VisualElement();
        root.style.flexGrow = 1;

        // 표 만들기
        CreateTable();

        // 루트에 추가
        rootVisualElement.Add(root);
    }
}
#endif
