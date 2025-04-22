using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGate : MonoBehaviour, IInteractable
{
    public ConfirmationUI confirmationUI;
    public int gateNumber; //문 고유 번호
    public int sceneToLoad; //각 문마다 로드 할 씬 번호

    private void Awake()
    {
        confirmationUI.gameObject.SetActive(false);
    }

    public void OnClickEnter()
    {
        Debug.Log(gateNumber.ToString());
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ReadyInteraction()
    {
    }

    public void ActiveInteraction()
    {
            confirmationUI.PopUpUI(OnClickEnter);
            Debug.Log(gateNumber);
    }
}
