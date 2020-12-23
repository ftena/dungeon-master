using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 3;
    public int currentHealth;
    public int scoreValue = 10;
    public AudioClip deathClip;

    Animation anim;
    AudioSource enemyAudio;
    bool isDead;

    void Awake ()
    {
		anim = transform.Find ("Demon").gameObject.GetComponent<Animation> ();
        enemyAudio = GetComponent <AudioSource> ();

        currentHealth = startingHealth;
    }
    public void TakeDamage (int amount)
    {
        if(isDead)
            return;

        //TODO: enemyAudio.Play ();

        currentHealth -= amount;
            
        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        //TODO: animation.SetTrigger ("Dead");

        //TODO: enemyAudio.clip = deathClip;
        //TODO: enemyAudio.Play ();

        //TODO: ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
