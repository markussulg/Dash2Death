using System.Collections;
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
        menuMusic.Play();
    }

    public void PlayButtonClick()
    {
        clickUIButton.Play();
    }
}
