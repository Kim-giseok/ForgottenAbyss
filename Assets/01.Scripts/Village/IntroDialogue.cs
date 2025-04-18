using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroDialogue : MonoBehaviour
{
    [SerializeField] private float delayBeforeDialogue = 0f;
    public string[] sentences;
    public Transform TalkPoint;
    public GameObject talkBoxPrefab;

    // 대화가 표시되었는지 확인하기 위한 키
    private const string INTRO_DIALOGUE_SHOWN_KEY = "IntroDialogueShown";
    // 이번 게임 세션을 위한 키
    private const string GAME_SESSION_KEY = "GameSession";

    private void Awake()
    {
        // 게임이 새로 실행되었는지 확인
        if (!PlayerPrefs.HasKey(GAME_SESSION_KEY))
        {
            // 새 게임 세션이므로 인트로 대화 상태 리셋
            PlayerPrefs.DeleteKey(INTRO_DIALOGUE_SHOWN_KEY);

            // 게임 세션 키 설정
            PlayerPrefs.SetInt(GAME_SESSION_KEY, 1);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        // 첫 번째로 1번 씬을 로드할 때에만 대사를 표시
        if (SceneManager.GetActiveScene().buildIndex == 1 && !PlayerPrefs.HasKey(INTRO_DIALOGUE_SHOWN_KEY))
        {
            Invoke("IntroSentence", delayBeforeDialogue);
        }

        // 대화가 표시되었음을 저장
        PlayerPrefs.SetInt(INTRO_DIALOGUE_SHOWN_KEY, 1);
        PlayerPrefs.Save();
    }

    public void IntroSentence()
    {
        if (FindObjectOfType<TalkSystem>() != null)
        {
            return; // 이미 대화창이 있으면 더 이상 생성X
        }

        GameObject gameObject = Instantiate(talkBoxPrefab);
        gameObject.GetComponent<TalkSystem>().Ondialogue(sentences, TalkPoint);
    }

    // 게임 종료 시 세션 키 제거
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey(GAME_SESSION_KEY);
        PlayerPrefs.Save();
    }
}
