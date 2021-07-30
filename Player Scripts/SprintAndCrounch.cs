using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAndCrounch : MonoBehaviour
{
    private Transform look_Root;
    private PlayerMovement player_movement;
    public float sprint_speed = 10f;
    public float crounc_speed = 2f;
    public float move_speed = 5f;
    private float stand_height = 1.6f;
    private float crounch_height = 1f;
    private bool is_Crouching;
    private Footsteps player_footsteps;
    private float sprint_volume = 1f;
    private float max_walk_volume = 0.6f, min_walk_volume = 0.2f;
    private float crouch_volume = 0.1f;
    private float walk_step_distance = 0.4f;
    private float sprint_step_distance = 0.25f;
    private float crouch_step_distance = 0.5f;
    private float sprint_Value = 100f;
    private PlayerStats player_stats;
    private float sprint_Treshold = 10f;
    // Start is called before the first frame update
    void Awake()
    {
        player_stats = gameObject.GetComponent<PlayerStats>();
        player_movement = gameObject.GetComponent<PlayerMovement>();
        look_Root = gameObject.transform.GetChild(0);
        player_footsteps = gameObject.GetComponentInChildren<Footsteps>();
    }
    private void Start()
    {
        player_footsteps.maxValume = max_walk_volume;
        player_footsteps.minValume = min_walk_volume;
        player_footsteps.step_Distance = walk_step_distance;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {

        if (sprint_Value > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
            {
                player_movement.speed = sprint_speed;
                player_footsteps.step_Distance = sprint_step_distance;
                player_footsteps.minValume = sprint_volume;
                player_footsteps.maxValume = sprint_volume;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {
            player_movement.speed = move_speed;
            player_footsteps.step_Distance = walk_step_distance;
            player_footsteps.maxValume = max_walk_volume;
            player_footsteps.minValume = min_walk_volume;
        }
        if (Input.GetKey(KeyCode.LeftShift) && !is_Crouching)
        {
            sprint_Value -= sprint_Treshold * Time.deltaTime;
            if (sprint_Value <= 0f)
            {
                sprint_Value = 0f;
                player_movement.speed = move_speed;
                player_footsteps.maxValume = max_walk_volume;
                player_footsteps.minValume = min_walk_volume;
                player_footsteps.step_Distance = walk_step_distance;
            }
            player_stats.Display_StaminaStats(sprint_Value);
        }
        else
        {
            if (sprint_Value != 100)
            {
                sprint_Value += (sprint_Treshold / 2f) * Time.deltaTime;
                player_stats.Display_StaminaStats(sprint_Value);
                if (sprint_Value > 100f)
                {
                    sprint_Value = 100f;
                }
            }
        }
    }
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (is_Crouching)
            {
                look_Root.localPosition = new Vector3(0f, stand_height, 0f);
                player_movement.speed = move_speed;
                is_Crouching = false;
                player_footsteps.step_Distance = walk_step_distance;
                player_footsteps.maxValume = max_walk_volume;
                player_footsteps.minValume = min_walk_volume;
            }
            else
            {
                look_Root.localPosition = new Vector3(0f, crounch_height, 0f);
                player_movement.speed = crounc_speed;
                is_Crouching = true;
                player_footsteps.step_Distance = crouch_step_distance;
                player_footsteps.maxValume = crouch_volume;
                player_footsteps.minValume = crouch_volume;
            }
        }
    }
}
