using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGate : MonoBehaviour, IInteractable
{
    public int gateNumber; //문 고유 번호
    public string sceneName;
    [SerializeField] FLAGKEY gateOpenFlag;

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
        if(!ActivateFlag.CheckFlag(gateOpenFlag))
        {
            UIManager.Instance.confirmationUI.PopUpUI("You Can't go now");
            return;
        }

        UIManager.Instance.confirmationUI.PopUpUI(onClickOk:OnClickEnter);
    }
}
