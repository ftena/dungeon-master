//using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic; 		//Allows us to use Lists.

public class EnemyAttack : MonoBehaviour
{
	public float timeBetweenAttacks = 1.0f;
	public int attackDamage = 50;				//Default value. It is defined in the Inspector.

	GameObject player;							//Store the player via its game object.
	PlayerHealth playerHealth;					//Store a reference to the PlayerHealht object.

	private Animation anim;                     //Used to store a reference to the Player's animation component.
	private List<string> attackAnimations;      //A list of attack animations
	private bool enemyAttack = false;
	private float range = 10f;
	private float range2 = 10f;
	private float speed = 3f; 					//Move speed
	private float stop = 0.7f;
	private float timer = 0f;					//Time to control two consecutives attacks.

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		anim = transform.Find ("Demon").gameObject.GetComponent<Animation> ();
		attackAnimations = new List <string> ();

		attackAnimations.Add ("DemonAttack");
		attackAnimations.Add ("DemonAttack2");
		attackAnimations.Add ("DemonFury");
	}

	void FixedUpdate ()
	{
		// + info: http://answers.unity3d.com/questions/585035/lookat-2d-equivalent-.html
		Vector3 relativePos = player.transform.position - transform.position;
		float angle = Mathf.Atan2 (relativePos.y, relativePos.x) * Mathf.Rad2Deg;

		// + info: http://answers.unity3d.com/questions/26177/how-to-create-a-basic-follow-ai.html	
		float distance = Vector3.Distance (transform.position, player.transform.position);

		//First case is just for looking at the target.
		if (distance <= range2 && distance >= range) {			
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);	

			//Play the demon's wait animation.
			anim.CrossFade ("DemonIdle", 0.01f);
		//Stops.
		} else if (distance <= stop) {
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);	
		}
		//Target in range
		else if (distance <= range && distance > stop) {			
			// Debug.Log ("Demon B - if (distance <= range && distance > stop -- distance: " + distance  + " stop: " + stop);

			// + info about rotation: http://answers.unity3d.com/questions/630670/rotate-2d-sprite-towards-moving-direction.html
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);	

			if ( !enemyAttack ) {
				//If the demon is not attacking, then set isKinematic again
				//to false (so the enemy switch to physics)
				GetComponent <Rigidbody2D> ().isKinematic = false;
				//Move object
				Vector2 movement = transform.right * speed * Time.fixedDeltaTime;
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + movement);
				//Play the demon's walk animation.
				anim.CrossFade ("DemonWalk", 0.01f);
			}
		}

		if (enemyAttack) {
			timer += Time.fixedDeltaTime;

			Debug.Log ("Time.fixedDeltaTime: " + Time.fixedDeltaTime + " Time.deltaTime: " + Time.deltaTime);

			if( timer >= timeBetweenAttacks )
			{
				Attack ();
			}

		}
		
	}

	void Attack() {
		timer = 0f;

		//When the demon is hitting the player,
		//we set isKinematic to true (switches off the physical behaviour) so that
		//it will not react to gravity and collisions.
		//If not, the enemy could be moved by the player impact.
		GetComponent <Rigidbody2D> ().isKinematic = true;
		//Play the demon's attack animation.
		anim.Play ("DemonAttack");

		if ( playerHealth.currentHealth > 0 ) {
			playerHealth.TakeDamage (attackDamage);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.gameObject == player)
		{
			enemyAttack = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {		
		if (other.gameObject == player)
		{
			enemyAttack = false;
		}
	}
}
