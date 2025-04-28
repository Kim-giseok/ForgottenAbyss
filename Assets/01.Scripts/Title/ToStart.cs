using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToStart : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1.25f;

    public string nextSceneName;
    public GameObject[] blockInputPanels;

    private bool isFading = false;
    private float alpha = 0f;

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
        }

        if (isFading)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);

            if (alpha >= 1f)
                SceneLoader.Instance.LoadScene(nextSceneName);
        }
    }
}
