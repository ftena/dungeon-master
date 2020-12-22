//Based on the Unity tutorial "Basic 2D Dungeon Generation"

using System.Collections;
using UnityEngine;
using System.Collections.Generic; 		//Allows us to use Lists.

public class BoardCreator : MonoBehaviour
{
	// The type of tile that will be laid in a specific position.
	public enum TileType
	{
		Wall, Floor,
	}

	// TODO: add new types: monsters, treasures, etc.


	public int columns = 100;                                 // The number of columns on the board (how wide it will be).
	public int rows = 100;                                    // The number of rows on the board (how tall it will be).
	public IntRange numRooms = new IntRange (15, 20);         // The range of the number of rooms there can be.
	public IntRange roomWidth = new IntRange (3, 10);         // The range of widths rooms can have.
	public IntRange roomHeight = new IntRange (3, 10);        // The range of heights rooms can have.
	public IntRange corridorLength = new IntRange (6, 10);    // The range of lengths corridors between rooms can have.
	public GameObject exit;									  // Prefab to spawn for exit.
	public GameObject[] floorTiles;                           // An array of floor tile prefabs.
	public GameObject[] wallTiles;                            // An array of wall tile prefabs.
	public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
	public GameObject key;                                    // Prefab to set the key

	public List<Room> AvailableRooms                          // To access the availableRooms member variable
	{
		get
		{
			return availableRooms;
		}
		set
		{
			availableRooms = value;
		}
	}

	private GameObject player;
	/* A jagged array is an array whose elements are arrays. */
	private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
	private List<Room> rooms;                                 // All the rooms that are created for this board.
	private List<Room> availableRooms;                        // Rooms available to set enemies & treasures
	private Corridor[] corridors;                             // All the corridors that connect the rooms.
	private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.


	//SetupScene initializes our level and calls the previous functions to lay out the game board
	public void SetupScene (int level)
	{
		// Create the board holder.
		boardHolder = new GameObject("BoardHolder");

		SetupTilesArray ();

		CreateRoomsAndCorridors ();

		SetTilesValuesForRooms ();
		SetTilesValuesForCorridors ();

		InstantiateTiles ();
		InstantiateOuterWalls ();
	}

	void SetupTilesArray ()
	{
		// Set the tiles jagged array to the correct width.
		tiles = new TileType[columns][];

		// Go through all the tile arrays...
		for (int i = 0; i < tiles.Length; i++)
		{
			// ... and set each tile array is the correct height.
			tiles[i] = new TileType[rows];
		}
	}


	void CreateRoomsAndCorridors ()
	{
		// Create the rooms array with a random size.
		// rooms = new Room[numRooms.Random];
		rooms = new List <Room> (numRooms.Random);

		// There should be one less corridor than there is rooms.
		corridors = new Corridor[rooms.Capacity - 1];

		// Create the first room and corridor.
		rooms.Add (new Room ());
		corridors[0] = new Corridor ();


		// Setup the first room, there is no previous corridor so we do not use one.
		// We pass the size of the board (columns & rows)
		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

		// Setup the first corridor using the first room.
		corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

		for (int i = 1; i < rooms.Capacity; i++)
		{
			// Create a room.
			rooms.Add(new Room ());

			// Setup the room based on the previous corridor.
			rooms[i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors[i - 1]);

			// If we haven't reached the end of the corridors array...
			if (i < corridors.Length)
			{
				// ... create a corridor.
				corridors[i] = new Corridor ();

				// Setup the corridor based on the room that was just created.
				corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
			}
		}

		availableRooms = new List<Room>(rooms);

		// Instantiate the player in a ramdom room
		// We will use this value to avoid the key in the same player's room
		int playerRoom = Random.Range (0, rooms.Count); 
		Vector3 playerPos = new Vector3 (rooms[playerRoom].xPos, rooms[playerRoom].yPos, -5.0f);
		//Get the player reference
		player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = playerPos;

		//Remove the entry playerRoom from the available rooms list.
		availableRooms.RemoveAt(playerRoom);

		// Instantiate the key in the center of a ramdom room
		int keyRoom = Random.Range (0, availableRooms.Count);
		Vector3 keyPos = new Vector3 (availableRooms[keyRoom].xPos + availableRooms[keyRoom].roomWidth/2f - 0.5f,
									  availableRooms[keyRoom].yPos + availableRooms[keyRoom].roomHeight/2f - 0.5f,
			                          -5.0f);
		// We don't use Quaternion.Identity as third param becasuse it's basically equal to no rotation.
		Instantiate(key, keyPos, key.transform.rotation);

		//Remove the entry keyRoom from the available rooms list.
		availableRooms.RemoveAt(keyRoom);

		//Instantiate the exit in the center of a random room
		int exitRoom = Random.Range (0, availableRooms.Count);
		Vector3 exitPos = new Vector3 (availableRooms[exitRoom].xPos + availableRooms[exitRoom].roomWidth/2f - 0.5f,
			                           availableRooms[exitRoom].yPos + availableRooms[exitRoom].roomHeight/2f - 0.5f,
			                           0f);
		// We don't use Quaternion.Identity as third param becasuse it's basically equal to no rotation.
		Instantiate (exit, exitPos, exit.transform.rotation);

		//Remove the entry exitRoom from the available rooms list.
		availableRooms.RemoveAt(exitRoom);
	}


	void SetTilesValuesForRooms ()
	{
		// Go through all the rooms...
		for (int i = 0; i < rooms.Count; i++)
		{
			Room currentRoom = rooms[i];

			// ... and for each room go through it's width.
			for (int j = 0; j < currentRoom.roomWidth; j++)
			{
				int xCoord = currentRoom.xPos + j;

				// For each horizontal tile, go up vertically through the room's height.
				for (int k = 0; k < currentRoom.roomHeight; k++)
				{
					int yCoord = currentRoom.yPos + k;

					// The coordinates in the jagged array are based on the room's position and it's width and height.
					tiles[xCoord][yCoord] = TileType.Floor;
				}
			}
		}
	}


	void SetTilesValuesForCorridors ()
	{
		// Go through every corridor...
		for (int i = 0; i < corridors.Length; i++)
		{
			Corridor currentCorridor = corridors[i];

			// and go through it's length.
			for (int j = 0; j < currentCorridor.corridorLength; j++)
			{
				// Start the coordinates at the start of the corridor.
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;

				// Depending on the direction, add or subtract from the appropriate
				// coordinate based on how far through the length the loop is.
				switch (currentCorridor.direction)
				{
				case Direction.North:
					yCoord += j;
					break;
				case Direction.East:
					xCoord += j;
					break;
				case Direction.South:
					yCoord -= j;
					break;
				case Direction.West:
					xCoord -= j;
					break;
				}

				// Set the tile at these coordinates to Floor.
				tiles[xCoord][yCoord] = TileType.Floor;
			}
		}
	}


	void InstantiateTiles ()
	{
		// Go through all the tiles in the jagged array... (columns = x axis)
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length; j++)
			{
				// ... and instantiate a floor tile for it.
				// 1- First, we put a floor tile.
				// 2- After this, we can put other elements, so if those elements
				//    are destroyed, we will see the floor beneath them.
				// This is necessary if we have the capacity of destroy walls.
				InstantiateFromArray (floorTiles, i, j);

				// If the tile type is Wall...
				if (tiles[i][j] == TileType.Wall)
				{
					// ... instantiate a wall over the top.
					InstantiateFromArray (wallTiles, i, j);
				}
			}
		}
	}


	void InstantiateOuterWalls ()
	{
		// The outer walls are one unit left, right, up and down from the board.
		float leftEdgeX = -1f;
		float rightEdgeX = columns + 0f;
		float bottomEdgeY = -1f;
		float topEdgeY = rows + 0f;

		// Instantiate both vertical walls (one on each side).
		InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
		InstantiateVerticalOuterWall (rightEdgeX, bottomEdgeY, topEdgeY);

		// Instantiate both horizontal walls, these are one in left and right from the outer walls.
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
	}


	void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
	{
		// Start the loop at the starting value for Y.
		float currentY = startingY;

		// While the value for Y is less than the end value...
		while (currentY <= endingY)
		{
			// ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
			InstantiateFromArray(outerWallTiles, xCoord, currentY);

			currentY++;
		}
	}


	void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
	{
		// Start the loop at the starting value for X.
		float currentX = startingX;

		// While the value for X is less than the end value...
		while (currentX <= endingX)
		{
			// ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
			InstantiateFromArray (outerWallTiles, currentX, yCoord);

			currentX++;
		}
	}


	void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
	{
		// Create a random index for the array.
		int randomIndex = Random.Range(0, prefabs.Length);

		// The position to be instantiated at is based on the coordinates.
		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		// Create an instance of the prefab from the random index of the array.
		GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

		// Set the tile's parent to the board holder.
		tileInstance.transform.parent = boardHolder.transform;
	}
}