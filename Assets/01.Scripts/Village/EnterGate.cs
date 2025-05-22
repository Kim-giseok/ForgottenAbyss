using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGate : MonoBehaviour, IInteractable
{
    public int gateNumber; //문 고유 번호
    public string sceneName;
    [SerializeField] FLAGKEY gateOpenFlag;
    [SerializeField] FLAGKEY clearCheckFlag;

    private void Awake()
    {
        UIManager.Instance.confirmationUI.gameObject.SetActive(false);
    }

    public void OnClickEnter()
    {
        Debug.Log(gateNumber.ToString());
        SceneLoader.Instance.LoadScene(sceneName);
    }

    public void ReadyInteraction()
    {
    }

    public void ActiveInteraction()
    {
        if (!ActivateFlag.CheckFlag(gateOpenFlag))
        {
            UIManager.Instance.confirmationUI.PopUpUI("이전 단계를 클리어 해야 합니다.");
            return;
        }


        if (!ActivateFlag.CheckFlag(clearCheckFlag))
            UIManager.Instance.confirmationUI.PopUpUI(onClickOk: OnClickEnter);
        else
        {
            StageTimeInfo timeinfo = new();
            timeinfo = timeinfo.LoadData(clearCheckFlag.ToString() + ".json");

            UIManager.Instance.confirmationUI.PopUpUI($"최근 기록 {timeinfo.cleartimeinfo}", OnClickEnter);
        }
    }
}
