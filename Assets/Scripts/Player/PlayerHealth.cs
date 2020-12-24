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


    Animation anim;
    AudioSource playerAudio;
    PlayerAttacking playerAttacking; //PlayerAttacking is the name of the script
	Player player; //Player is the name of the script
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = transform.Find("Knight").gameObject.GetComponent<Animation>();
        //The next line means "plays back the animation".
        //When it reaches the end, it will keep playing the last frame (played has died) and never stop playing.
        anim["Dead"].wrapMode = WrapMode.ClampForever;
        playerAudio = GetComponent <AudioSource> ();
		playerAttacking = GetComponent <PlayerAttacking> ();
		player = GetComponent <Player> ();
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

	void FixedUpdate ()
    {
        if(isDead)
        {
            anim.Play("Dead");
        }
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
        playerAudio.clip = deathClip;
        playerAudio.Play ();

        //Disable player's scripts
		playerAttacking.enabled = false;
		player.enabled = false;
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
