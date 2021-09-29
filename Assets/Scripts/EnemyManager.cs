using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public float startingHealth = 100;          
    public float currentHealth;
    public float timeBetweenAttacks = 0.5f;
    public float sinkSpeed = 2.5f;
    float timer;

    public int attackDamage = 10; 
    public int scoreValue = 10; 
    
    bool playerInRange;
    bool isSinking;

    public AudioClip deathClip;
    public ParticleSystem hitParticles;
    public ParticleSystem DeathParticles;

    AudioSource enemyAudio;
    Animator anim;
    PlayerManager playerManager;
    GameObject player;
    Transform playerT;
    NavMeshAgent navmesh;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        player = GameObject.FindGameObjectWithTag ("Player");
        playerManager = player.GetComponent <PlayerManager> ();
        playerT = GameObject.FindGameObjectWithTag ("Player").transform;
        navmesh = GetComponent <NavMeshAgent> ();
        currentHealth = startingHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded) // Oyun baslamadiysa veya bittiyse
        {
            return;
        }
        if(isSinking)
            {
                transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
            }
        timer += Time.deltaTime;
        if(timer >= timeBetweenAttacks && playerInRange)
            {
                Attack ();
            }
        navmesh.SetDestination (playerT.position);
    }

    public void TakeDamage (float amount)
    {
        enemyAudio.Play ();
        currentHealth -= amount;
        if(currentHealth <= 0)
            {
                Death ();
        }
        hitParticles.Play();
    }

    void OnTriggerEnter (Collider other)
        {
            if(other.gameObject == player)
            {
                playerInRange = true;
            }
        }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    void Attack ()
    {
        timer = 0f;
        if(playerManager.currentHealth > 0)
        {
            playerManager.TakeDamage (attackDamage);
        }
    }

    void Death ()
    {
        anim.SetTrigger ("Dead");
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
        //Destroy (gameObject, 2f);
    }
    public void StartSinking () //????????????????????????????
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        GameManager.instance.score += scoreValue;
        Destroy (gameObject, 2f);
        DeathParticles.Play();
        }
}
