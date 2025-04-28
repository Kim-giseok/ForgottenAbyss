using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PressAnyKey : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1.5f;
    public string nextSceneName;

    private bool isFading = false;
    private float alpha = 0f;

    void Update()
    {
        bool isPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        if (!isFading && Input.anyKeyDown && !isPointerOverUI)
        {
            isFading = true;
        }

        if (isFading)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);

            if (alpha >= 1f)
                SceneManager.LoadScene(nextSceneName);
        }
    }
}
