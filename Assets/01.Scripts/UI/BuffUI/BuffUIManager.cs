using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIManager : MonoBehaviour
{
    [SerializeField] private List<BuffUIElement> predefinedSlots;

    private Dictionary<StatType, BuffUIElement> activeBuffs = new();

    public void ShowBuff(StatType stat,float duration, Sprite icon)
    {
        if (activeBuffs.TryGetValue(stat, out var existingSlot))
        {
            existingSlot.Initialize(stat, duration, icon, () => activeBuffs.Remove(stat));
            return;
        }

        BuffUIElement available = predefinedSlots.FirstOrDefault(slot => !slot.gameObject.activeSelf);
        if (available == null)
        {
            Debug.LogWarning("[BuffUIManager] 사용 가능한 버프 슬롯이 부족함");
            return;
        }

        available.gameObject.SetActive(true);
        available.Initialize(stat, duration, icon, () => activeBuffs.Remove(stat));
        activeBuffs.Add(stat, available);
    }
}
