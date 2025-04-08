using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _sfxList;

    Dictionary<string, AudioClip> _sfxLookup = new();

    void Awake()
    {
        Instance = this;

        RegisterSfx();
    }

    void RegisterSfx()
    {
        foreach (var sfx in _sfxList)
        {
            var name = sfx.name.Split("sfx-")[1];
            _sfxLookup.Add(name, sfx);
        }
    }

    public static void PlaySfx(string sfxName, float volumeScale = 1)
    {
        if (Instance._sfxLookup.TryGetValue(sfxName, out AudioClip clip))
            Instance._audioSource.PlayOneShot(clip, volumeScale);
    }
}