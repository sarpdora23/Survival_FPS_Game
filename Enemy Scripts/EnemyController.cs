using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}
public class EnemyController : MonoBehaviour
{
    
    private EnemyAnimator enemy_anim;
    private NavMeshAgent nav_agent;
    private EnemyState enemy_state;
    public float walk_speed = 0.5f;
    public float run_speed = 4f;
    public float chased_distance = 20f;
    private float current_chased_distance;
    public float attack_distance = 2.2f;
    public float chase_after_attack_distance = 2f;
    public float patrol_radius_min = 20f, patrol_radius_max = 60f;
    public float patrol_for_this_time = 15f;
    private float patrol_timer;
    public float wait_before_attack = 2f;
    private float attack_timer;
    private Transform target;
    public GameObject attack_Point;
    private EnemyAudio enemy_Audio;
    private void Awake()
    {
        enemy_Audio = gameObject.GetComponentInChildren<EnemyAudio>();
        enemy_anim = gameObject.GetComponent<EnemyAnimator>();
        nav_agent = gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy_state = EnemyState.PATROL;
        current_chased_distance = chased_distance;
        patrol_timer = patrol_for_this_time;
        attack_timer = wait_before_attack;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_state == EnemyState.PATROL)
        {
            Patrol();
        }
        else if (enemy_state == EnemyState.CHASE)
        {
            Chase();
        }
        else if (enemy_state == EnemyState.ATTACK)
        {
            Attack();
        }
    }
    void Patrol()
    {
        nav_agent.isStopped = false;
        nav_agent.speed = walk_speed;
        patrol_timer += Time.deltaTime;
        if (patrol_timer > patrol_for_this_time)
        {
            SetRandomDestination();
            patrol_timer = 0f;
        }
        if (nav_agent.velocity.sqrMagnitude > 0)
        {
            enemy_anim.Walk(true);
        }
        else
        {
            enemy_anim.Walk(false);
        }
        if (Vector3.Distance(gameObject.transform.position,target.position) <= chased_distance)
        {
            enemy_anim.Walk(false);
            enemy_state = EnemyState.CHASE;
            enemy_Audio.Play_ScreamSound();
        }
    }
    void Chase()
    {
        nav_agent.isStopped = false;
        nav_agent.speed = run_speed;
        nav_agent.SetDestination(target.position);
        if (nav_agent.velocity.sqrMagnitude > 0)
        {
            enemy_anim.Run(true);
        }
        else
        {
            enemy_anim.Run(false);
        }
        if (Vector3.Distance(transform.position,target.position) <= attack_distance)
        {
            enemy_anim.Run(false);
            enemy_anim.Walk(false);
            enemy_state = EnemyState.ATTACK;
            if (chased_distance != current_chased_distance)
            {
                chased_distance = current_chased_distance;
            }
        }
        else if (Vector3.Distance(transform.position,target.position) > chased_distance)
        {
            enemy_anim.Run(false);
            enemy_state = EnemyState.PATROL;
            patrol_timer = patrol_for_this_time;
            if (chased_distance != current_chased_distance)
            {
                chased_distance = current_chased_distance;
            }
        }
    }
    void Attack()
    {
        nav_agent.velocity = Vector3.zero;
        nav_agent.isStopped = true;
        attack_timer += Time.deltaTime; 
        if (attack_timer > wait_before_attack)
        {
            enemy_anim.Attack();
            attack_timer = 0f;
            enemy_Audio.Play_AttackSound();
        }
        if (Vector3.Distance(transform.position, target.position) > attack_distance + chase_after_attack_distance)
        {
            enemy_state = EnemyState.CHASE;
        }
    }
    void SetRandomDestination()
    {
        float rand_radius = Random.Range(patrol_radius_min, patrol_radius_max);
        Vector3 randDir = Random.insideUnitSphere * rand_radius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randDir, out hit, rand_radius, -1);
        nav_agent.SetDestination(hit.position);
    }
    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }
    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }
    public EnemyState Enemy_State
    {
        get
        {
            return enemy_state;
        }
        set
        {
            enemy_state = value;
        }
    }
}
