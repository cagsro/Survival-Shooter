using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int startingHealth = 100;         
    public int currentHealth;
    public float flashSpeed = 5f;  


    Animator anim;
    AudioSource playerAudio;  

    public AudioClip deathClip; 
    public Image damageImage;
    public Slider healthSlider; 
    protected Joystick joystick;
    protected FixedButton fixedButton;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    bool damaged;
    public bool isDead=false; 


    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent <AudioSource> ();
        currentHealth = startingHealth;
        anim = GetComponent <Animator> ();
        joystick=FindObjectOfType<Joystick>();
        fixedButton= FindObjectOfType<FixedButton>();
        
    }

    void Update()
    {
         if(damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded) // Oyun baslamadiysa veya bittiyse
        {
            return;
        }
        transform.position = new Vector3(
                    transform.position.x + joystick.Horizontal*0.15f,
                    transform.position.y,
                    transform.position.z+joystick.Vertical*0.15f);
        if(joystick.Horizontal==0 && joystick.Vertical==0)
        {
            anim.SetBool ("IsWalking", false);
        }
        else
        {
            anim.SetBool ("IsWalking", true);
        }

        Vector3 frameMovement = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        if(frameMovement==Vector3.zero){
            return;
        }
        else{
            transform.rotation = Quaternion.LookRotation(frameMovement);
        }
        /*var rigidbody=GetComponent<Rigidbody>();
        rigidbody.velocity=new Vector3( joystick.Horizontal*10f,
                                        rigidbody.velocity.y,
                                        joystick.Vertical*10f);*/

       
    }

    public void TakeDamage (int amount)
    {
        damaged = true;
        playerAudio.Play ();
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        Debug.Log(""+currentHealth);
        if(currentHealth <= 0)
        {
            Death ();
            
        }
    }
    void Death ()
    {
        isDead = true;
        GameManager.instance.GameEnded();
        // Tell the animator that the player is dead.
        anim.SetTrigger ("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play ();
    }

    public void RestartLevel ()
        {
            GameManager.instance.OnLevelFailed();
        }
}
