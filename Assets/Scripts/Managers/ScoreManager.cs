using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;

    Text text;


    void Awake ()
    {
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        text = GetComponent<Text>();
    }


    void Update ()
    {
        text.text = "Score: " + score;
    }
}
