using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 11;
    public int currentHealth;
    public int scoreValue = 10;
    public AudioClip deathClip;

    Animation anim;
    AudioSource enemyAudio;
    EnemyAttack enemyAttack; //EnemyAttack is the name of the script
    bool isDead = false;

    void Awake ()
    {
		anim = transform.Find ("Demon").gameObject.GetComponent<Animation> ();
        //The next line means "plays back the animation".
        //When it reaches the end, it will keep playing the last frame (enemy has died) and never stop playing.
        anim["DemonDeath"].wrapMode = WrapMode.ClampForever;
		enemyAttack = GetComponent <EnemyAttack> ();
        enemyAudio = GetComponent <AudioSource> ();

        currentHealth = startingHealth;
    }
	void FixedUpdate ()
    {
        if(isDead)
        {
            anim.Play("DemonDeath");
        }
    }

    public void TakeDamage (int amount)
    {
        if(isDead)
            return;

        currentHealth -= amount;
            
        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        enemyAttack.enabled = false;
        isDead = true;

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();

        //TODO: ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
