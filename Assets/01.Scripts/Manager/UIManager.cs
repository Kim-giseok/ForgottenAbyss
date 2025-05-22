using System.Collections;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public DragManager DragManager { get; private set; }
    public DragItemPool dragItemPool;
    public QuickSlotController quickSlotController;

    [Header("ScreenUI")]
    public InventoryUIManager inventoryUI;
    public SettingsMenu settingsMenu;
    public WeaponSwapper weaponSwapper;
    public ConfirmationUI confirmationUI;
    public ShopUI shopUI;
    public StatUI statUI;
    public PassiveUI passiveUI;
    public ItemTooltip tooltip;
    public CanvasGroup ingameUI;
    public CanvasGroup DeathUI;
    public GameObject GuideUI;
    public BossHealthUI BossHealthUI;

    [Header("WorldUI")]
    public RectTransform worldSpaceCanvas;
    public GameObject npcText;
    public SentenceUI talkBox;

    public float fadeDuration = 1f;
    public float holdTime = 2f;

    private void Awake()
    {
        dragItemPool.Initialize();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        DragManager = new DragManager();
        Debug.Log("[UIManager] DragManager ������");

    }

    public void ToggleInventory() => inventoryUI?.ToggleInventory();
    public void CloseInventory() => inventoryUI?.Close();
    public void ToggleSettings() => settingsMenu?.ToggleSettingsMenu();
    public void SwapWeapons() => weaponSwapper?.SwapWeapons();
    public void OnStatUI() => statUI?.OnStatusUI();
    public void OnPassiveUI() => passiveUI?.OnPassiveUI();
    public void ShowTooltip(ITooltipData data, Vector3 position)
    {
        if (tooltip == null) return;

        tooltip.SetData(data);
        tooltip.Show((Vector2)position);
    }
    public void HideTooltip() => tooltip?.Hide();

    public void HideTooltipNextFrame()
    {
        StartCoroutine(HideTooltipDelayed());
    }

    private IEnumerator HideTooltipDelayed()
    {
        yield return null;
        tooltip?.Hide();
    }

    public void OnGuidUI(GameObject gameobject)
    {
        Vector3 position = gameobject.transform.position + Vector3.up * 1.5f * gameObject.transform.localScale.y;
        npcText.transform.position = position;

        if (!npcText.activeSelf)
            npcText.SetActive(true);
    }

    public void OffGuidUI()
    {
        if (npcText != null && npcText.activeSelf)
            npcText.SetActive(false);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void OnTalk(Transform parent, string sentence)
    {
        talkBox.Set(sentence).Forget();
        talkBox.transform.position = parent.position + new Vector3(2f, 2.6f, 0f);
    }

    public void OffTalk()
    {
        if (talkBox.gameObject.activeSelf)
            talkBox.gameObject.SetActive(false);
    }

    public void HideIngameUI()
    {
        ingameUI.alpha = 0f;
        ingameUI.interactable = false;
        ingameUI.blocksRaycasts = false;
    }

    public void ShowIngameUI()
    {
        ingameUI.alpha = 1f;
        ingameUI.interactable = true;
        ingameUI.blocksRaycasts = true;
    }

    public void HideGuideUI()
    {
        GuideUI.SetActive(false);
    }

    public void ShowGuideUI()
    {
        GuideUI.SetActive(true);
    }

    public void HideDeathUI()
    {
        DeathUI.alpha = 0f;
        DeathUI.interactable = false;
        DeathUI.blocksRaycasts = false;
    }

    public void ShowDeathUI()
    {
        StartCoroutine(DeathUIAnimation());
    }

    public IEnumerator DeathUIAnimation()
    {
        DeathUI.interactable = true;
        DeathUI.blocksRaycasts = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            DeathUI.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        DeathUI.alpha = 1f;

        yield return new WaitForSeconds(holdTime);
    }

    public void HIdeAll()
    {
        HideIngameUI();
        HideGuideUI();
        HideDeathUI();

        OffGuidUI();
        OffTalk();
        HideTooltip();

        inventoryUI.Close();
        settingsMenu.CloseSettingsMenuUI();
        statUI.OffStatUI();
        passiveUI.OffPassiveUI();

        Debug.Log("[UIManager] 모든 UI ?��? ?�료");
    }

    public void ToggleStatUI()
    {
        if (statUI != null)
        {
            if (statUI.statusUI != null && statUI.statusUI.activeSelf)
            {
                // If it's open, close it
                statUI.OffStatUI();
            }
            else
            {
                // If it's closed, open it
                statUI.OnStatusUI();
            }
        }
    }

    public void TogglePassiveUI()
    {
        if (passiveUI != null)
        {
            if (passiveUI.passiveUI != null && passiveUI.passiveUI.activeSelf)
            {
                // If it's open, close it
                passiveUI.OffPassiveUI();
            }
            else
            {
                // If it's closed, open it
                passiveUI.OnPassiveUI();
            }
        }
    }
}
