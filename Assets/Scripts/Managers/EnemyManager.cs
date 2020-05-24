using UnityEngine;
using System.Collections;
using System.Collections.Generic; 		//Allows us to use Lists.

public class EnemyManager : MonoBehaviour {

	class Position
	{
		public int x  { get; set; }
		public int y  { get; set; }
	};

	public GameObject[] enemyTiles;                                     //Array of enemy prefabs.

	private List<Position> availablePositions;                          //A list of possible locations to place enemy tiles.

	//SetupEnemies initializes our enemies using the rooms available
	public void SetupEnemies (int level, List<Room> availableRooms)
	{
		availablePositions = new List<Position> ();

		//For each room, we get five positions: four corners plus the center of the room
		foreach (var room in availableRooms) {
			availablePositions.Add (new Position  { x = room.xPos, y = room.yPos });
			availablePositions.Add (new Position  { x = room.xPos + room.roomWidth -1, y = room.yPos });
			availablePositions.Add (new Position  { x = room.xPos, y = room.yPos + room.roomHeight -1});
			availablePositions.Add (new Position  { x = room.xPos + room.roomWidth -1, y = room.yPos + room.roomHeight -1});
			availablePositions.Add (new Position  { x = room.xPos + (room.roomWidth/2), y = room.yPos + (room.roomHeight/2)});
		}

		//Determine number of enemies based on current level number, based on a logarithmic progression
		int enemyCount = (int)Mathf.Log(level, 2f) + level;

		//This must be clamped to ensure that the max number of enemies <= the available positions.
		enemyCount = Mathf.Clamp (enemyCount, 0, availablePositions.Count);

		//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
	}

	//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
	{
		//Choose a random number of objects to instantiate within the minimum and maximum limits
		int objectCount = Random.Range (minimum, maximum+1);

		//Instantiate objects until the randomly chosen limit objectCount is reached
		for(int i = 0; i < objectCount; i++)
		{
			//Choose a position for randomPosition by getting a random position from our list of availablePositions
			Vector3 randomPosition = RandomPosition();

			//Choose a random tile from tileArray and assign it to tileChoice
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

			//Instantiate tileChoice at the position returned by RandomPosition
			Instantiate (tileChoice, randomPosition, tileChoice.transform.rotation);
		}
	}

	//RandomPosition returns a random position from our list availablePositions.
	Vector3 RandomPosition ()
	{
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List availablePositions.
		int randomIndex = Random.Range (0, availablePositions.Count);

		//Declare a variable of type Vector3 called enemyPos, set it's value to the entry at randomIndex from our List availablePositions.
		Vector3 enemyPos = new Vector3 (availablePositions[randomIndex].x, availablePositions[randomIndex].y, -5.0f);

		//Remove the entry at randomIndex from the list so that it can't be re-used.
		availablePositions.RemoveAt (randomIndex);

		//Return the randomly selected Vector3 position.
		return enemyPos;
	}
}
