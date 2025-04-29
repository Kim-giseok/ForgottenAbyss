using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : Singleton<ScreenFader>
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Camera mainCamera;

    public float distanceFromCamera = 1.5f;
    public Vector2 screenSizeInWorldUnits = new Vector2(10f, 6f);

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        PositionInFrontOfCamera();
    }

    private void LateUpdate()
    {
        PositionInFrontOfCamera();
    }

    private void PositionInFrontOfCamera()
    {
        if (mainCamera == null) return;

        Vector3 camForward = mainCamera.transform.forward;
        transform.position = mainCamera.transform.position + camForward * distanceFromCamera;

        transform.rotation = Quaternion.LookRotation(camForward);

        rectTransform.sizeDelta = screenSizeInWorldUnits;
    }

    public IEnumerator FadeIn(float duration = 1f)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeOut(float duration = 1f)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
