using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour
{
	public float speed = 10f;

	// Get the end position of the corridor based on it's start position and which direction it's heading.
	public float Speed
	{
		get	{
			return speed;
		}

		set {
			speed = value;
		}
			
	}

	void Update ()
	{
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}