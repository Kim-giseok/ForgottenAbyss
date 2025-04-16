using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public InventoryUI inventoryUI;
    public SettingsMenu settingsMenu;
    public WeaponSwapper weaponSwapper;

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
}
