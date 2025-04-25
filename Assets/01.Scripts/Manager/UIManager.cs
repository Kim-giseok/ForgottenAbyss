using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("ScreenUI")]
    public InventoryUIManager inventoryUI;
    public SettingsMenu settingsMenu;
    public WeaponSwapper weaponSwapper;
    public ConfirmationUI confirmationUI;
    public GameObject shopUI;

    [Header("WorldUI")]
    public RectTransform worldSpaceCanvas;
    public GameObject npcText;
    public SentenceUI talkBox;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지
    }

    public void ToggleInventory() => inventoryUI?.ToggleInventory();
    public void CloseInventory() => inventoryUI?.Close();
    public void ToggleSettings() => settingsMenu?.ToggleSettingsMenu();
    public void SwapWeapons() => weaponSwapper?.SwapWeapons();

    public void OnGuidUI(MonoBehaviour gameobject)
    {
        Vector3 position = gameobject.transform.position + Vector3.up * 1.5f;
        npcText.transform.position = position;

        if (!npcText.activeSelf)
            npcText.SetActive(true);
    }

    public void OffGuidUI()
    {
        if (npcText.activeSelf)
            npcText.SetActive(false);
    }

    public void OnTalk(NpcSentence sentenceObj, string sentence)
    {
        talkBox.Ondialogue(sentence);
        talkBox.transform.position = sentenceObj.transform.position;
    }

    public void OffTalk()
    {
        if (talkBox.gameObject.activeSelf)
            talkBox.gameObject.SetActive(false);
    }

    public void HideAllUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowAllUI()
    {
        gameObject.SetActive(true);
    }
}
