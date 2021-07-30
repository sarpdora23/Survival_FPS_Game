using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeWooshSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] woosh_sounds;
    void PlayWooshSound()
    {
        audioSource.clip = woosh_sounds[Random.Range(0, woosh_sounds.Length)];
        audioSource.Play();
    }
}
