using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUIBinder : MonoBehaviour
{
    public PlayerStatus playerStatus; // PlayerStatus 연결
    public Slider hpSlider; // HP 게이지바
    public TMPro.TextMeshProUGUI hpText; // HP텍스트
    public Slider mpSlider; // MP 게이지바
    public TMPro.TextMeshProUGUI mpText; // MP텍스트
    public Slider expSlider; // Exp 게이지바
    public TMPro.TextMeshProUGUI expText; // Exp 텍스트
    public TMPro.TextMeshProUGUI levelText; // 레벨 텍스트

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 이미 존재하는 경우 대비
        if (playerStatus != null)
            playerStatus.OnStatChanged += HandleStatChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (playerStatus != null)
            playerStatus.OnStatChanged -= HandleStatChanged;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var newStatus = FindObjectOfType<PlayerStatus>();
        if (newStatus != null)
        {
            BindStatus(newStatus);
        }
    }

    public void BindStatus(PlayerStatus status)
    {
        playerStatus = status;

        // 이벤트 구독
        playerStatus.OnStatChanged += HandleStatChanged;

        // 슬라이더 초기 설정
        hpSlider.minValue = 0f;
        hpSlider.maxValue = 1f;
        mpSlider.minValue = 0f;
        mpSlider.maxValue = 1f;
        expSlider.minValue = 0f;
        expSlider.maxValue = 1f;

        //초기값 반영
        UpdateHPSlider();
        UpdateMPSlider();
        UpdateExpSlider();
        UpdateLevelText();
    }

    private void HandleStatChanged(StatType type, float newValue)
    {
        if (type == StatType.CurrentHP || type == StatType.MaxHP)
        {
            UpdateHPSlider();
        }

        if (type == StatType.CurrentMP || type == StatType.MaxMP)
        {
            UpdateMPSlider();
        }
        if (type == StatType.EXP || type == StatType.MaxEXP)
        {
            //Debug.Log($"[UIBinder] 경험치 반영됨: {newValue}");
            UpdateExpSlider();
        }
        if (type == StatType.LEVEL)
        {
            //Debug.Log($"[UIBinder] level 반영됨: {newValue}");
            UpdateLevelText();
        }
    }

    // HP UI갱신 함수
    private void UpdateHPSlider()
    {
        if (playerStatus == null) return;
        if (!playerStatus.stats.ContainsKey(StatType.CurrentHP) || !playerStatus.stats.ContainsKey(StatType.MaxHP))
            return;

        float curHP = playerStatus.stats[StatType.CurrentHP];
        float maxHP = playerStatus.stats[StatType.MaxHP];

        hpSlider.value = Mathf.Clamp01(curHP / maxHP);

        // text UI
        if (hpText != null)
        {
            hpText.text = $"{(int)curHP} / {(int)maxHP}";
        }
    }

    // MP UI갱신 함수
    private void UpdateMPSlider()
    {
        if (playerStatus == null) return;
        if (!playerStatus.stats.ContainsKey(StatType.CurrentMP) || !playerStatus.stats.ContainsKey(StatType.MaxMP))
            return;

        float curMP = playerStatus.stats[StatType.CurrentMP];
        float maxMP = playerStatus.stats[StatType.MaxMP];

        mpSlider.value = Mathf.Clamp01(curMP / maxMP);

        if (mpText != null)
        {
            mpText.text = $"{(int)curMP} / {(int)maxMP}";
        }
    }

    // Exp UI갱신 함수
    private void UpdateExpSlider()
    {
        if (playerStatus == null) return;
        if (!playerStatus.stats.ContainsKey(StatType.EXP) || !playerStatus.stats.ContainsKey(StatType.MaxEXP))
            return;

        float curExp = playerStatus.stats[StatType.EXP];
        float maxExp = playerStatus.stats[StatType.MaxEXP];

        expSlider.value = Mathf.Clamp01(curExp / maxExp);

        if (expText != null)
        {
            expText.text = $"{(int)curExp} / {(int)maxExp}";
        }
    }

    private void UpdateLevelText()
    {
        if (playerStatus == null || levelText == null) return;

        int level = (int)playerStatus.stats[StatType.LEVEL];
        levelText.text = $"Lv. {level}";
    }
}
