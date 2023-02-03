using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("UI Sounds")]
    [Space]
    public AudioClipGroup clickUIButton;
    public AudioClipGroup hoverUIButton;

    [Header("Game Sounds")]
    [Space]

    public static AudioController Instance;

    private void Awake()
    {
        Instance = this;
    }
}
