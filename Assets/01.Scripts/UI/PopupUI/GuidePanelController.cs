using UnityEngine;
using UnityEngine.EventSystems;

public class GuidePanelController : MonoBehaviour, IPointerClickHandler
{
    public GameObject guidePanel;

    private void Start()
    {
        guidePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ToggleGuidePanel()
    {
        bool isActive = !guidePanel.activeSelf;
        guidePanel.SetActive(isActive);
        Time.timeScale = isActive ? 0f : 1f;
    }

    public void CloseGuidePanel()
    {
        guidePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CloseGuidePanel();
    }
}
