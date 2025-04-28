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
    public ShopUI shopUI;
    public StatUI statUI;
    public PassiveUI passiveUI;
    public ItemTooltip tooltip;
    public GameObject ingameUI; 

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
        DontDestroyOnLoad(gameObject); // �� �Ѿ�� ����
    }

    public void ToggleInventory() => inventoryUI?.ToggleInventory();
    public void CloseInventory() => inventoryUI?.Close();
    public void ToggleSettings() => settingsMenu?.ToggleSettingsMenu();
    public void SwapWeapons() => weaponSwapper?.SwapWeapons();
    public void OnStatUI() => statUI?.OnStatusUI();
    public void OnPassiveUI() => passiveUI?.OnPassiveUI();
    public void ShowTooltip(Item item, Vector3 position) => tooltip?.Show(item, position);
    public void HideTooltip() => tooltip?.Hide();

    public void OnGuidUI(GameObject gameobject)
    {
        Vector3 position = gameobject.transform.position + Vector3.up * 1.5f * gameObject.transform.localScale.y;
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
        ingameUI.SetActive(false);
    }

    public void ShowAllUI()
    {
        ingameUI.SetActive(true);
    }
}
