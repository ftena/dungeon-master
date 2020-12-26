using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	public float speed = 5f;
	public float rotSpeed = 5f;
	public float restartLevelDelay = 2f;		//Delay time in seconds to restart level.
    public AudioClip keyClip;
    public AudioClip doorClip;
    AudioSource playerAudio;
	private Animation anim;                     //Used to store a reference to the Player's animation component.
	private GameObject keyImage;                            //Key found by the player. 

    void Awake()
    {
		keyImage = GameObject.Find("KeyImage");
        playerAudio = GetComponent<AudioSource> ();
    }

	void Start () {
		anim = transform.Find ("Knight").gameObject.GetComponent<Animation> ();
	}

	void Update () {
		
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");

		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

		//Rotate object
		if (movement != Vector2.zero) {
			float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

			// + info: http://answers.unity3d.com/questions/630670/rotate-2d-sprite-towards-moving-direction.html
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			//Move object
			movement = movement.normalized * speed * Time.fixedDeltaTime;

			GetComponent<Rigidbody2D>().MovePosition (GetComponent<Rigidbody2D>().position + movement);

			//Play the player's walk animation.
			anim.CrossFade ("Walk", 0.01f);
		} else {
			//Play the player's wait animation.
			anim.CrossFade ("Wait", 0.01f);
		}
	}		

	void OnTriggerEnter2D(Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if(other.tag == "Exit")	
		{
			//Complete the level only if the key was found
			if(keyImage.activeSelf)
			{
        		playerAudio.clip = doorClip;
        		playerAudio.Play ();

				other.GetComponent<RandomRotator>().Speed = 200f;

                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke("Restart", restartLevelDelay);

                //Disable the player object since level is over.
                enabled = false;
			}
		}
		else if (other.tag == "Key")
		{
			//... the player got the key, then set the other object (key in this case) we just collided with to inactive.
			other.gameObject.SetActive(false);

			keyImage.SetActive(true);

        	playerAudio.clip = keyClip;
        	playerAudio.Play ();
		}
	}

	//Restart reloads the scene when called.
	private void Restart ()
	{
		//Load the last scene loaded, in this case Main, the only scene in the game.
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}
