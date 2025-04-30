using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    // 오디오 소스 참조
    private AudioSource audioSource;

    // 다양한 동작 사운드들
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip dashSound;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void JumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
            
        }
    }

    public void LandSound()
    {
        if (audioSource != null && landSound != null)
        {
            audioSource.PlayOneShot(landSound);
            
        }
    }

    public void DashSound()
    {
        if (audioSource != null && dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);

        }
    }

    
}
