using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum SkillSlotType
{
    Memory = 0,
    Basic = 1,
    Skill01 = 2,
    Skill02 = 3
}

public class SkillUI : MonoBehaviour
{

    public GameObject[] hideSkillButtons;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImages;
    public Image[] skillIcons;
    private bool[] isHideSkills = { false, false, false, false};
    private float[] skillTimes = { 12, 9, 9, 9 };
    private float[] getSkillTimes = { 0,0,0,0 };
    private float memoryCooldown;


    public event System.Action OnInitialized;
    public bool IsInitialized = false;

    private Dictionary<int, float[]> weaponCooldownTable = new Dictionary<int, float[]>();
    private Coroutine[] skillCoroutines; // �� ��ų�� ���� �ڷ�ƾ�� ����

    void Start()
    {
        skillCoroutines = new Coroutine[hideSkillButtons.Length];
        for( int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false); // ��ư ��Ȱ��ȭ
        }

        IsInitialized = true;
        OnInitialized?.Invoke();
    }

    void Update()
    {
       // HideSkillCheck(); // �� �����Ӹ��� ��礤 ��ų�� ��Ÿ�� üũ < �ι�üũ�Ǽ� ����
    }

    //��ų ��ư�� Ȱ��ȭ�ϰ�, ��Ÿ�� ����
    public void HideSkillSetting(int skillNum, float coolTime)
    {
        if (!isHideSkills[skillNum]) //�̹� Ȱ��ȭ�� ��ų�� ���� �ߺ� ���� ����
        {
            hideSkillButtons[skillNum].SetActive(true); // ��ư �Ҽ�ȭ
            getSkillTimes[skillNum] = coolTime; // ��Ÿ�� ���� (�ܺο��� ���� ��)
            //skillTimes[skillNum] = coolTime;    // �� ��Ÿ�� ��ϵ� ����  < ��Ÿ�� ui ������Ʈ�� ������ �־� ������ ���� �����۵���
            isHideSkills[skillNum] = true; // ��ų�� Ȱ��ȭ��

            // �ش� ������ ���� �ڷ�ƾ ����
            if (skillCoroutines[skillNum] != null)
            {
                StopCoroutine(skillCoroutines[skillNum]); // ���� �ڷ�ƾ�� ������ ����
            }
            skillCoroutines[skillNum] = StartCoroutine(SkillTimeCheck(skillNum)); // �� �ڷ�ƾ ����
        }
    }

    // �� ��ų�� ��Ÿ�� üũ �� UI������Ʈ
    private void HideSkillCheck()
    {
        for (int i = 0; i <isHideSkills.Length; i++)
        {
            // �ش� ��ų�� Ȱ��ȭ���� ��Ÿ�� üũ
            if (isHideSkills[i] && getSkillTimes[i] > 0)
            {
                UpdateSkillUI(i); //��Ÿ�� ������Ʈ
            }
        }
    }

    private void UpdateSkillUI(int skillNum)
    {
        getSkillTimes[skillNum] -= Time.deltaTime; // ���� ��Ÿ�� ����
        if (getSkillTimes[skillNum] <= 0) // ��Ÿ���� ������
        {
            getSkillTimes[skillNum] = 0; // 0���� ����
            isHideSkills[skillNum] = false; // ��ų�� ��Ȱ��ȭ��
            hideSkillButtons[skillNum].SetActive(false); // ��ư ��Ȱ��ȭ
        }
        // ���� ��Ÿ�� �ؽ�Ʈ ������Ʈ
        hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("0.0");

        //��Ÿ���� ������ ����� �̹��� ����
        float time = getSkillTimes[skillNum] / skillTimes[skillNum];
        hideSkillImages[skillNum].fillAmount = time;
    }

    // ��Ÿ�� ó���ϴ� �ڷ�ƾ
    IEnumerator SkillTimeCheck(int skillNum)
    {
        while (getSkillTimes[skillNum] > 0)
        {
            yield return null; // �� ������ ���
            UpdateSkillUI(skillNum); //uI����
        }
    }

    public void SetSkillIcon(SkillSlotType slot, Sprite icon)
    {
        int idx = (int)slot;
        if (skillIcons[idx] != null)
        {
            if (!skillIcons[idx].enabled)
                skillIcons[idx].enabled = true;

            skillIcons[idx].sprite = icon;
        }
    }

    public void SetSkillCooldownTime(SkillSlotType slot, float cooldown)
    {
        int idx = (int)slot;
        if (idx >= 0 && idx < skillTimes.Length)
            skillTimes[idx] = cooldown;
    }

    public void SaveCurrentCooldown(int weaponId)
    {
        float[] cooldownEndTimes = new float[getSkillTimes.Length];
        for (int i = 0; i < getSkillTimes.Length; i++)
        {
            if (i == (int)SkillSlotType.Memory)
            {
                memoryCooldown = Time.time + getSkillTimes[(int)SkillSlotType.Memory];
                continue;
            }

            cooldownEndTimes[i] = Time.time + getSkillTimes[i];
        }
        weaponCooldownTable[weaponId] = cooldownEndTimes;
    }

    public void LoadCooldownFromWeapon(int weaponId)
    {
        ResetAllCooldowns();

        if (weaponCooldownTable.TryGetValue(weaponId, out float[] savedEndTimes))
        {
            for (int i = 0; i < getSkillTimes.Length; i++)
            {
                float remainingTime = savedEndTimes[i] - Time.time;
                if (remainingTime > 0f)
                {
                    HideSkillSetting(i, remainingTime);
                }
            }
        }

        float remainingMemory = memoryCooldown - Time.time;
        if (remainingMemory > 0f)
        {
            HideSkillSetting((int)SkillSlotType.Memory, remainingMemory);
        }
    }

    public void ResetAllCooldowns()
    {
        Debug.Log($"[SkillUI] ResetAllCooldowns - hideSkillButtons: {hideSkillButtons?.Length}");
        for (int i = 0; i < isHideSkills.Length; i++)
        {
            if (skillCoroutines[i] != null)
            {
                StopCoroutine(skillCoroutines[i]);
                skillCoroutines[i] = null;
            }

            getSkillTimes[i] = 0f;
            isHideSkills[i] = false;
            if (hideSkillButtons[i] != null)
                hideSkillButtons[i].SetActive(false);
            else
                Debug.LogWarning($"[SkillUI] hideSkillButtons[{i}]�� null�Դϴ�.");

            if (hideSkillImages[i] != null)
                hideSkillImages[i].fillAmount = 0f;
            else
                Debug.LogWarning($"[SkillUI] hideSkillImages[{i}]�� null�Դϴ�.");

            if (hideSkillTimeTexts[i] != null)
                hideSkillTimeTexts[i].text = "";
            else
                Debug.LogWarning($"[SkillUI] hideSkillTimeTexts[{i}]�� null�Դϴ�.");
        }
    }

    public void ClearSkillIcon(SkillSlotType type)
    {
        int idx = (int)type;

        if (idx >= 0 && idx < skillIcons.Length && skillIcons[idx] != null)
        {
            skillIcons[idx].sprite = null; 
            skillIcons[idx].enabled = false;          
        }

        if (idx >= 0 && idx < hideSkillImages.Length && hideSkillImages[idx] != null)
        {
            hideSkillImages[idx].fillAmount = 0f;  
        }

        if (idx >= 0 && idx < hideSkillTimeTexts.Length && hideSkillTimeTexts[idx] != null)
        {
            hideSkillTimeTexts[idx].text = ""; 
        }

        if (idx >= 0 && idx < hideSkillButtons.Length && hideSkillButtons[idx] != null)
        {
            hideSkillButtons[idx].SetActive(false); 
        }

        if (idx >= 0 && idx < isHideSkills.Length)
        {
            isHideSkills[idx] = false; 
        }

        Debug.Log($"[SkillUI] ClearSkillIcon: {type} �ʱ�ȭ �Ϸ�");
    }
}
