using UnityEngine;
using System.Collections;

public class WeaponChest : MonoBehaviour, IInteractable
{
    public string chestId = "chest_start";
    private bool hasBeenOpened = false;
    public string[] sentences;
    public string[] alreadyOpenedSentences;
    public Transform talkPoint;
    public GameObject talkBoxPrefab;

    private void Awake()
    {
        //if (!PlayerPrefs.HasKey("GameSessionInitialized"))
        //{
        //    PlayerPrefs.DeleteKey($"ChestOpened_{chestId}");

        //    PlayerPrefs.SetInt("GameSessionInitialized", 1);
        //    PlayerPrefs.Save();
        //}

        PlayerPrefs.DeleteKey($"ChestOpened_{chestId}");
        PlayerPrefs.Save();
    }

    private void Start()
    {
        string key = $"ChestOpened_{chestId}";
        hasBeenOpened = PlayerPrefs.GetInt(key, 0) == 1;
    }

    public void ReadyInteraction()
    {
        // UI 띄우기
        Debug.Log("F키를 눌러 상자 열기");
    }

    public void ActiveInteraction()
    {
        if (FindObjectOfType<TalkSystem>() != null)
        {
            Debug.Log("이미 대화 중입니다.");
            return;
        }

        GameObject talkBox = Instantiate(talkBoxPrefab);
        TalkSystem talk = talkBox.GetComponent<TalkSystem>();

        if (hasBeenOpened)
        {
            talk.Ondialogue(alreadyOpenedSentences, talkPoint);
            return;
        }

        hasBeenOpened = true;
        PlayerPrefs.SetInt($"ChestOpened_{chestId}", 1);
        PlayerPrefs.Save();

        talk.Ondialogue(sentences, talkPoint);
        StartCoroutine(WaitForDialogueThenGiveWeapons(talkBox));
    }

    IEnumerator WaitForDialogueThenGiveWeapons(GameObject talkBox)
    {
        // talkBox가 파괴될 때까지 기다림 (대사 끝나면 자동으로 Destroy됨)
        while (talkBox != null)
        {
            yield return null;
        }

        // 대사 끝났으니 무기 지급
        WeaponManager.Instance.EquipDefaultWeapons();
        Debug.Log("무기 지급 완료!");
    }
}
