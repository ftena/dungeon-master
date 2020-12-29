using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;          //GameManager prefab to instantiate.
    public GameObject soundManager;         //SoundManager prefab to instantiate.


    void Awake()
    {
        //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
        if (GameManager.instance == null)
        {

            //Instantiate gameManager prefab
            Instantiate(gameManager);
        }

        //Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
        if (SoundManager.instance == null)
        {

            //Instantiate SoundManager prefab
            Instantiate(soundManager);
        }
    }

    public void NewGame()
    {
        //Call the InitGame function to initialize the first level 
        GameManager.instance.InitGame();
    }
    public void RestartGame()
    {
        //Call the InitGame function to initialize the first level 
        GameManager.instance.RestartGame();
    }

}