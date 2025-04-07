using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager UIinstance;

    public static UIManager Instance
    {
        get
        {
            if (UIinstance == null)
            {
                UIinstance = FindObjectOfType<UIManager>();

                if (UIinstance == null)
                {
                    Debug.LogError("UIManager가 인스턴스에 존재하지 않습니다.");
                }
            }
            return UIinstance;
        }
    }

    private void Awake()
    {
        if (UIinstance == null)
        {
            UIinstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
