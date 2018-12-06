using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BigEnemy : MonoBehaviour
{
    public AudioClip EnemyDeathSound;
    public float attackDelay;
    private float attackTimer;
    public float damage;
    public GameObject EnemyDeathParticles; 

    public float maxHealth;
    public float currentHealth;
    protected NavMeshAgent navMesh;
    protected GameObject player;
    bool HasDied = false;
    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        navMesh.SetDestination(player.transform.position);
        attackTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject ot = other.gameObject;
            if (attackTimer <= 0)
            {
                ot.GetComponent<Movement>().TakeDamage(damage);
                attackTimer = attackDelay;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject ot = other.gameObject;
            if (attackTimer <= 0)
            {
                ot.GetComponent<Movement>().TakeDamage(damage);
                attackTimer = attackDelay;
            }
        }
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            // EnemyDeathSound.Play();
            // gameObject.SetActive(false);
            if (!HasDied)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Attack>().Score += 1;
                SpawnSound();
                DeathParticles();
            }

            HasDied = true;
            Debug.Log("score");
        }

    }

    void SpawnSound()
    {
        GameObject gameobject = new GameObject();
        gameobject.AddComponent<DestroySound>();
        AudioSource aud = gameobject.AddComponent<AudioSource>();

        aud.clip = EnemyDeathSound;
        Instantiate(gameobject, transform.position, Quaternion.identity);
    }

    void DeathParticles()
    {
        GameObject go = Instantiate(EnemyDeathParticles, transform.position, Quaternion.identity); ;
        go.GetComponent<ParticleSystem>().Play();
        
    }
}
