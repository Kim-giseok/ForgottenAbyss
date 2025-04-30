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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 시작 시 AudioClip 확인 (디버깅용)
        if (jumpSound == null)
            Debug.LogWarning("Jump sound is not assigned in PlayerSound component!");
        if (landSound == null)
            Debug.LogWarning("Land sound is not assigned in PlayerSound component!");
    }

    public void JumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
            Debug.Log("점프사운드");
        }
    }

    public void LandSound()
    {
        if (audioSource != null && landSound != null)
        {
            audioSource.PlayOneShot(landSound);
        }
    }
}
