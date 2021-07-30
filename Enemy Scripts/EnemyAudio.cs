using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioSource audio_source;
    [SerializeField]
    private AudioClip scream_clip, die_clip;
    [SerializeField]
    private AudioClip[] attack_clips;
    private void Awake()
    {
        audio_source = gameObject.GetComponent<AudioSource>();
    }
    public void Play_ScreamSound()
    {
        audio_source.clip = scream_clip;
        audio_source.Play();
    }
    public void Play_AttackSound()
    {
        audio_source.clip = attack_clips[Random.Range(0, attack_clips.Length)];
        audio_source.Play();
    }
    public void Dead_Sound()
    {
        audio_source.clip = die_clip;
        audio_source.Play();
    }
}
