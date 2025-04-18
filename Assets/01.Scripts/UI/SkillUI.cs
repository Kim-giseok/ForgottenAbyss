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


    public event Action OnInitialized;
    public bool IsInitialized = false;

    private Dictionary<int, float[]> weaponCooldownTable = new Dictionary<int, float[]>();
    private Coroutine[] skillCoroutines; // 각 스킬에 대한 코루틴을 저장

    void Start()
    {
        skillCoroutines = new Coroutine[hideSkillButtons.Length];
        for( int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false); // 버튼 비활성화
        }

        IsInitialized = true;
        OnInitialized?.Invoke();
    }

    void Update()
    {
       // HideSkillCheck(); // 매 프레임마다 모든ㄴ 스킬의 쿨타임 체크 < 두번체크되서 뺏음
    }

    //스킬 버튼을 활성화하고, 쿨타임 시작
    public void HideSkillSetting(int skillNum, float coolTime)
    {
        if (!isHideSkills[skillNum]) //이미 활성화된 스킬에 대해 중복 설정 방지
        {
            hideSkillButtons[skillNum].SetActive(true); // 버튼 할성화
            getSkillTimes[skillNum] = coolTime; // 쿨타임 설정 (외부에서 받은 값)
            //skillTimes[skillNum] = coolTime;    // 총 쿨타임 기록도 갱신
            isHideSkills[skillNum] = true; // 스킬이 활성화됨

            // 해당 설정에 대한 코루틴 실행
            if (skillCoroutines[skillNum] != null)
            {
                StopCoroutine(skillCoroutines[skillNum]); // 기존 코루틴이 있으면 정지
            }
            skillCoroutines[skillNum] = StartCoroutine(SkillTimeCheck(skillNum)); // 새 코루틴 시작
        }
    }

    // 각 스킬의 쿨타임 체크 후 UI업데이트
    private void HideSkillCheck()
    {
        for (int i = 0; i <isHideSkills.Length; i++)
        {
            // 해당 스킬이 활성화됐음 쿨타임 체크
            if (isHideSkills[i] && getSkillTimes[i] > 0)
            {
                UpdateSkillUI(i); //쿨타임 업데이트
            }
        }
    }

    private void UpdateSkillUI(int skillNum)
    {
        getSkillTimes[skillNum] -= Time.deltaTime; // 남은 쿨타임 차감
        if (getSkillTimes[skillNum] <= 0) // 쿨타임이 끝나면
        {
            getSkillTimes[skillNum] = 0; // 0으로 설정
            isHideSkills[skillNum] = false; // 스킬이 비활성화됨
            hideSkillButtons[skillNum].SetActive(false); // 버튼 비활성화
        }
        // 남은 쿨타임 텍스트 업데이트
        hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("0.0");

        //쿨타임을 비율로 계산해 이미지 갱신
        float time = getSkillTimes[skillNum] / skillTimes[skillNum];
        hideSkillImages[skillNum].fillAmount = time;
    }

    // 쿨타임 처리하는 코루틴
    IEnumerator SkillTimeCheck(int skillNum)
    {
        while (getSkillTimes[skillNum] > 0)
        {
            yield return null; // 한 프래임 대기
            UpdateSkillUI(skillNum); //uI갱신
        }
    }

    public void SetSkillIcon(SkillSlotType slot, Sprite icon)
    {
        int idx = (int)slot;
        if (skillIcons[idx] != null)
            skillIcons[idx].sprite = icon;
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
                Debug.LogWarning($"[SkillUI] hideSkillButtons[{i}]가 null입니다.");

            if (hideSkillImages[i] != null)
                hideSkillImages[i].fillAmount = 0f;
            else
                Debug.LogWarning($"[SkillUI] hideSkillImages[{i}]가 null입니다.");

            if (hideSkillTimeTexts[i] != null)
                hideSkillTimeTexts[i].text = "";
            else
                Debug.LogWarning($"[SkillUI] hideSkillTimeTexts[{i}]가 null입니다.");
        }
    }
}
