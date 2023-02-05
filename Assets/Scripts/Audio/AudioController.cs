using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("UI Sounds")]
    [Space]
    public AudioClipGroup clickUIButton;
    public AudioClipGroup menuMusic;

    [Header("Game Sounds")]
    [Space]

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

    public void PlayButtonClick()
    {
        clickUIButton.Play();
    }

    IEnumerator AudioDelay()
    {
        yield return new WaitForSeconds(0.25f);
        menuMusic.Play();
    }
}
