using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{

    public GameObject[] hideSkillButtons;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImages;
    private bool[] isHideSkills = { false, false, false, false};
    private float[] skillTimes = { 12, 9, 9, 9 };
    private float[] getSkillTimes = { 0,0,0,0 };

    private Coroutine[] skillCoroutines; // 각 스킬에 대한 코루틴을 저장

    void Start()
    {
        skillCoroutines = new Coroutine[hideSkillButtons.Length];
        for( int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false); // 버튼 비활성화
        }
    }

    void Update()
    {
        HideSkillCheck(); // 매 프레임마다 모든ㄴ 스킬의 쿨타임 체크
    }

    //스킬 버튼을 활성화하고, 쿨타임 시작
    public void HideSkillSetting(int skillNum)
    {
        if (!isHideSkills[skillNum]) //이미 활성화된 스킬에 대해 중복 설정 방지
        {
            hideSkillButtons[skillNum].SetActive(true); // 버튼 할성화
            getSkillTimes[skillNum] = skillTimes[skillNum]; //쿨타임 설정
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
        hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("00");

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
}
