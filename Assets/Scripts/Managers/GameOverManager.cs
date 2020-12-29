using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {        
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");

            GameManager.instance.GameOver();

            enabled = false; //Enabled Behaviours are Updated, disabled Behaviours are not.
        }        
    }
}
