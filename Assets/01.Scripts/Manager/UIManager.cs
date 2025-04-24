using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("ScreenUI")]
    public InventoryUI inventoryUI;
    public SettingsMenu settingsMenu;
    public WeaponSwapper weaponSwapper;
    public ConfirmationUI confirmationUI;
    public GameObject shopUI;

    [Header("WorldUI")]
    public RectTransform worldSpaceCanvas;
    public GameObject npcText;
    public TalkSystem talkBox;

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
    public void ToggleSettings() => settingsMenu?.ToggleSettingsMenu();
    public void SwapWeapons() => weaponSwapper?.SwapWeapons();

    public void OnGuidUI(MonoBehaviour gameobject)
    {
        Vector3 position = gameobject.transform.position + Vector3.up * 1.5f;
        npcText.transform.position = position;
        npcText.SetActive(true);
    }

    public void OnTalk(NpcSentence sentence)
    {
        talkBox.gameObject.SetActive(true);
        talkBox.Ondialogue(sentence.sentences, sentence.transform);
    }
}
