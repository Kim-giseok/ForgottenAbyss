using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGate : MonoBehaviour
{
    public GameObject confirmationUI;

    private void Awake()
    {
        confirmationUI.SetActive(false);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            confirmationUI.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            confirmationUI.SetActive(false);
        }
    }

    public void OnClickEnter()
    {
        Debug.Log("¿‘¿Â");
        //SceneManager.LoadScene(""); 
    }

    public void OnClickExit()
    {
        confirmationUI.SetActive(false);
    }
}
