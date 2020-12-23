using UnityEngine;
using System.Collections.Generic; 		//Allows us to use Lists.

public class PlayerAttacking : MonoBehaviour
{
    public int damagePerShot = 1;
    public float timeBetweenAttacks = 0.15f;

    public List<AudioClip> attackClips;
    public float lowPitchRange = .95f;				//The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;			//The highest a sound effect will be randomly pitched.
	
    float timer;
    AudioSource attackAudio;

    private Animation anim;                         //Used to store a reference to the Player's animation component.
    private bool playerTouchingEnemy = false;       //Used to check if the player is touching an enemy

    void Awake()
    {
        anim = transform.Find("Knight").gameObject.GetComponent<Animation>();
        attackAudio = GetComponent<AudioSource> ();
    }

    void Update()
    {
        /* Similar to EnemyAttack.cs */
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1")) 
        {
            Attack();
        }
    }

    void Attack()
    {
        // Reset time because we're firing now
        timer = 0f;
        //Play the player's attack animation.
        anim.CrossFade("Attack", 0.01f);
        RandomizeSfx(attackClips);
    }

    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(List<AudioClip> clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Count);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        if(!attackAudio.isPlaying)
        {
            //Set the pitch of the audio source to the randomly chosen pitch.
            attackAudio.pitch = randomPitch;

            //Set the clip to the clip at our randomly chosen index.
            attackAudio.clip = clips[randomIndex];

            //Play the clip.
            attackAudio.Play();
        }
    }
	void OnTriggerStay2D(Collider2D other) {		
        playerTouchingEnemy = true;
        if (timer <= timeBetweenAttacks) //Player is attacking.
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            /* We can also hit walls or legos, so we must check if enemyHealth has
             * a script. */
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        playerTouchingEnemy = false;
    }
}
