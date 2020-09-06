using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public AudioClip hurtClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
	// PlayerMovement is the name of the script
    // TODO: ¿remove? PlayerMovement playerMovement;
	// PlayerShooting is the name of the script
	// TODO: ¿remove? PlayerShooting playerShooting;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        // TDODO: redo: anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
		// TODO: ¿remove? playerMovement = GetComponent <PlayerMovement> ();
		// PlayerShooting is on the GunBarrelEnd
		// TODO: ¿remove? playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.clip = hurtClip;
        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

		// TODO: ¿remove? playerShooting.DisableEffects ();

        // TODO: redo: anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

		// TODO: ¿remove? playerMovement.enabled = false;
		// TODO: ¿remove? playerShooting.enabled = false;
    }

	// TODO: OLD CODE
    // Method called by Player Death Animation.
    // Go to Models->Characters->Player->Death->Edit.
    // There we have an arrow (event) called RestartLevel
    //public void RestartLevel ()
    //{
    //    Application.LoadLevel (Application.loadedLevel);
    //}
}
