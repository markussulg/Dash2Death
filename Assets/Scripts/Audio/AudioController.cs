using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("UI Sounds")]
    [Space]
    public AudioClipGroup clickUIButton;
    public AudioClipGroup hoverUIButton;
    public AudioClipGroup menuMusic;

    [Header("Game Sounds")]
    [Space]
    public AudioClipGroup dashAudio;
    public AudioClipGroup playerHitAudio;
    public AudioClipGroup solidHitAudio;

    public static AudioController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(AudioDelay());
        }
        else
        {
            menuMusic.Play();
        }
    }

    public void PlayHoverAudio()
    {
        hoverUIButton.Play();
    }

    public void PlayButtonClick()
    {
        clickUIButton.Play();
    }

    public void PlayDashAudio()
    {
        dashAudio.Play();
    }

    public void PlaySolidHitAudio()
    {
        solidHitAudio.Play(); 
    }

    public void PlayPlayerHitAudio()
    {
        playerHitAudio.Play();
    }

    IEnumerator AudioDelay()
    {
        yield return new WaitForSeconds(0.25f);
        menuMusic.Play();
    }
}
