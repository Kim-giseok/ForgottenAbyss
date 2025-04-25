using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveUI : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public GameObject passiveUI;

    private void Update()
    {
      
    }
    public void OnPassiveUI()
    {
        passiveUI.SetActive(true);
    }

    public void OffPassiveUI()
    {
        passiveUI.SetActive(false);
    }
}
