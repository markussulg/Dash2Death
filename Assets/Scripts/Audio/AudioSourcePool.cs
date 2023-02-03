using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public AudioSource audioSourcePrefab;
    public static AudioSourcePool Instance;
    private List<AudioSource> _audioSources;

    private void Awake()
    {
        Instance = this;
        _audioSources = new List<AudioSource>();
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource source in _audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource newSource = Instantiate(audioSourcePrefab, transform);
        _audioSources.Add(newSource);
        return newSource;
    }
}
