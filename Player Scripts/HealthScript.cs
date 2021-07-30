using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{
    private EnemyAnimator enemy_anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_controller;
    public float health = 100f;
    public bool is_player, is_boar, is_cannibal;
    private bool is_dead;
    private EnemyAudio enemy_audio;
    private PlayerStats player_stats;
    // Start is called before the first frame update
    void Awake()
    {
        if (is_boar || is_cannibal)
        {
            enemy_anim = GetComponent<EnemyAnimator>();
            enemy_controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();
            enemy_audio = GetComponentInChildren<EnemyAudio>();
        }
        if (is_player)
        {
            player_stats = GetComponent<PlayerStats>();
        }
    }
    public void ApplyDamage(float damage)
    {
        if (is_dead)
            return;
        health -= damage;
        if (is_player)
        {
            player_stats.Display_HealthStats(health);
        }
        if (is_boar || is_cannibal)
        {
            if (enemy_controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_controller.chased_distance = 50f;
            }
        }
        if (health <= 0)
        {
            PlayerDead();
            is_dead = true;
        }
    }
    void PlayerDead()
    {
        if (is_cannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 50f);
            enemy_controller.enabled = false;
            navAgent.enabled = false;
            enemy_anim.enabled = false;
            StartCoroutine(DeadSound());
            EnemyManager.instance.EnemyDied(true);
        }
        if (is_boar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_controller.enabled = false;
            enemy_anim.Dead();
            StartCoroutine(DeadSound());
            EnemyManager.instance.EnemyDied(false);
        }
        if (is_player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyController>().enabled = false;
            }
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
            EnemyManager.instance.StopSpawnning();
        }
        if (tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }
    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemy_audio.Dead_Sound();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
