using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuidePanelController : MonoBehaviour, IPointerClickHandler
{
    public GameObject guidePanel;

    private void Start()
    {
        guidePanel.SetActive(false);
    }

    public void ToggleGuidePanel()
    {
        guidePanel.SetActive(!guidePanel.activeSelf);
    }

    public void CloseGuidePanel()
    {
        guidePanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CloseGuidePanel();
    }
}
