using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    [Range(0, 1)]
    public float volumeMin = 0.7f;
    [Range(0, 1)]
    public float volumeMax = 1;
    [Range(0, 2)]
    public float pitchMin = 1.2f;
    [Range(0, 2)]
    public float pitchMax = 0.7f;
    public float cooldown = 0.4f;
    public AudioClip[] clips;

    private float _timestamp;

    private void OnEnable()
    {
        _timestamp = 0;
    }

    public void Play()
    {
        if (AudioSourcePool.Instance == null) return;
        Play(AudioSourcePool.Instance.GetSource());
    }

    public void Play(AudioSource source)
    {
        if (_timestamp > Time.time) return;
        if (clips.Length == 0) return;
        _timestamp = Time.time + cooldown;

        source.volume = Random.Range(volumeMin, volumeMax);
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }
}
