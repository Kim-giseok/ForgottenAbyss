using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToStart : MonoBehaviour
{
    public string nextSceneName;
    public GameObject[] blockInputPanels;

    private bool isFading = false;

    private bool IsBlockPanelActive()
    {
        foreach (var panel in blockInputPanels)
        {
            if (panel.activeSelf) return true;
        }
        return false;
    }

    void Update()
    {
        bool clickedUI = false;

        if (Input.GetMouseButtonDown(0))
        {
            clickedUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        bool anyKeyPressed = Input.anyKeyDown;

        if (!isFading && (anyKeyPressed || Input.GetMouseButtonDown(0)) && !clickedUI && !IsBlockPanelActive())
        {
            isFading = true;
            FadeScene.Instance.StartFadeOut();
        }

        if (isFading && FadeScene.Instance.fadeImage.color.a >= 1f)
        {
            SceneLoader.Instance.LoadScene(nextSceneName);
        }
    }
}
