using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private AudioSource footstep_sound;
    [SerializeField]
    private AudioClip[] footsteps_clips;
    [HideInInspector]
    public float minValume, maxValume;
    private CharacterController character_controller;
    private float acculumated_Distance;
    [HideInInspector]
    public float step_Distance;
    // Start is called before the first frame update
    void Awake()
    {
        footstep_sound = gameObject.GetComponent<AudioSource>();
        character_controller = gameObject.GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckToPlayFootstep();
    }
    void CheckToPlayFootstep()
    {
        if (!character_controller.isGrounded)
            return;
        if (character_controller.velocity.sqrMagnitude > 0)
        {
            acculumated_Distance += Time.deltaTime;
            if (acculumated_Distance > step_Distance)
            {
                footstep_sound.volume = Random.Range(minValume, maxValume);
                footstep_sound.clip = footsteps_clips[Random.Range(0, footsteps_clips.Length)];
                footstep_sound.Play();
                acculumated_Distance = 0f;
            }
        }
        else
        {
            acculumated_Distance = 0f;
        }
    }
}
