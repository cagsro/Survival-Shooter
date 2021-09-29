using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public static PlayerShooting instance;
    public float damage =10f;
    public float range=20f;
    public GameObject GunBarrelEnd;
    LineRenderer gunLine;
    Light gunLight;
    public Light faceLight;
    ParticleSystem gunParticles;  
    AudioSource gunAudio; 



    private void Awake()
    {
        gunAudio = GetComponent<AudioSource> ();
        gunParticles = GetComponent<ParticleSystem> ();
        gunLight = GetComponent<Light> ();
        gunLine = GetComponent <LineRenderer> ();
        if (instance == null) 
            instance = this;
            
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded) // Oyun baslamadiysa veya bittiyse
        {
            return;
        }
        gunAudio.Play ();
        gunParticles.Stop ();
        gunParticles.Play ();
        faceLight.enabled=true;
        gunLight.enabled = true;
        gunLine.enabled = true;
        gunLine.SetPosition (0, GunBarrelEnd.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(GunBarrelEnd.transform.position,GunBarrelEnd.transform.forward, out hit,range))
        {
            if (hit.transform.tag.Equals("Enemy"))
            {
                EnemyManager enemyManager = hit.collider.GetComponent <EnemyManager> ();
                enemyManager.TakeDamage (damage);
                Debug.Log(hit.transform.name);
                gunLine.SetPosition (1, hit.point);
            }
            else
            {
                gunLine.SetPosition (1, GunBarrelEnd.transform.position+GunBarrelEnd.transform.forward*range);
            }
            
        }
        Invoke("DisableEffects", 0.1f);
    }

    public void DisableEffects ()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
		faceLight.enabled = false;
        gunLight.enabled = false;
    }


}
