using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    // 오디오 소스 참조
    //private AudioSource audioSource;

    

    // 다양한 동작 사운드들
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip dashSound;
    

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();

    }

    public void JumpSound()
    {
        if (jumpSound != null)
        {
            SoundManager.Instance.PlaySFX(jumpSound);
            
        }
    }

    public void LandSound()
    {
        if (landSound != null)
        {
            SoundManager.Instance.PlaySFX(landSound);
            
        }
    }

    public void DashSound()
    {
        if (dashSound != null)
        {
            SoundManager.Instance.PlaySFX(dashSound);

        }
    }

    
}
