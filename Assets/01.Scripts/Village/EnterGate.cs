using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGate : MonoBehaviour
{
    public ConfirmationUI confirmationUI;
    public int gateNumber; //문 고유 번호
    public int sceneToLoad; //각 문마다 로드 할 씬 번호

    private void Awake()
    {
        confirmationUI.gameObject.SetActive(false);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            confirmationUI.PopUpUI(OnClickEnter);
            Debug.Log(gateNumber);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || confirmationUI == null || !gameObject.activeInHierarchy)
            return;
        if (collision != null && collision.CompareTag("Player"))
        {
            if (confirmationUI != null)
                confirmationUI.gameObject.SetActive(false);
        }
    }

    public void OnClickEnter()
    {
        Debug.Log(gateNumber.ToString());
        SceneManager.LoadScene(sceneToLoad);
    }
}
