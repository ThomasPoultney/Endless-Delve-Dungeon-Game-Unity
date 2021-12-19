using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Script that is responsible for generating the world and spawning its contents based upon the current difficulty.
/// </summary>
public class WorldSpawner : MonoBehaviour
{
    ///The height of each room
    [SerializeField]
    private float roomHeight;

    ///The width of each room
    [SerializeField]
    private float roomWidth;
    ///The number of rooms spawned horizontally
    [SerializeField]
    private int numRoomsHor;
    ///The number of rooms spawned vertically
    [SerializeField]
    private int numRoomsVer;
    ///The miniumum offset that mobs can spawn from entrance
    [SerializeField]
    private int preventMobRadius = 5;

    ///percentage chance for critical path to move down at a given step
    [SerializeField]
    private int chanceToMoveDown = 10;
    ///The time between each room spawning in the map, used for visualizing the algorithm
    [SerializeField]
    private float timeBetweenRoomSpawns = 2.0f;

    ///The room templates for each room type LR,LRB,LRT,LRTB.
    [SerializeField]
    private GameObject[] rooms; 
    /// The border prefab to be spawned
    [SerializeField]
    private GameObject borderBlock;

    ///An empty game object that the map will be spawned as a child of.
    private Transform world;

    ///The time since the last room was spawned.
    private float timeElapsedSinceLastRoom; 
    ///The bottom left position of the entrance room.
    private int firstRoomPosition;
    ///Whether the critical path has finished generating.
    private bool reachedExit = false;
    ///The position to spawn the next room at.
    private Vector2 nextRoomPosition;

    ///Whether the critical path spawning algorithm can make a right move
    private bool canMoveRight = true;
    ///Whether the critical path spawning algorithm can make a left move
    private bool canMoveLeft = true;
    ///Whether the last room that was spawned has a bottom entrance.
    private bool lastRoomWasBottom = false;

    ///The cardinal directions the room spawning algorithm can move.
    private enum Direction { Down, Left, Right, ImpossibleMove };

    ///The layer of the room
    [SerializeField]
    private LayerMask Room;
    ///The layer of the basic building blocks that make up the world.
    [SerializeField]
    private LayerMask GroundTileLayer;

    ///The last room that the room spawning algorithm created.
    private GameObject lastRoomCreated;
    ///The Door prefab to be spawned at the entrance and exit.
    [SerializeField]
    private GameObject door; //the door prefab

    ///The room containing the entrance door.
    private GameObject entranceRoom;
    ///The room containing the exit door.
    private GameObject exitRoom;

    ///checks if the entrance door has already been spawned
    private bool spawnedEntranceDoor = false;
    ///checks if the exit door has already been spawned
    private bool spawnedExitDoor = false;
    ///checks if the mobs has already been spawned
    private bool spawnedMobs = false;
    ///checks if treasure has already been spawned
    private bool spawnedTresure = false;

    ///All the treasure objects to be randomly spawned
    [SerializeField]
    private GameObject[] treasurePrefabs;
    ///All the ground mobs to be randomly spawned
    [SerializeField]
    private GameObject[] groundMobPrefabs;
    ///All the flying mobs to be randomly spawned
    [SerializeField]
    private GameObject[] flyingMobPrefabs;

    ///The player gameobject that needs to be moved to the entranceway.
    public GameObject player;

    ///The number of treasure objects to spawn
    [SerializeField]
    private int numTreasureToSpawn = 0;
    ///The number of ground mobs to spawn
    [SerializeField]
    private int numGroundMobsToSpawn = 0;
    ///The number of flying mobs to spawn
    [SerializeField]
    private int numFlyingMobsToSpawn = 0;




    // Start is called before the first frame update
    void Start()
    {     
        SpawnWorld();
    }

    /// <summary>
    /// updates the world variables and then spawns a new world based upon their new values..
    /// </summary>
    public void SpawnWorld()
    {
       
        resetWorldSpawnVariables();
        world = new GameObject("World").transform;
        SpawnEntranceRoom();
        GenerateBorders();
    }
    /// <summary>
    /// Generates the one tile border around the map
    /// </summary>
    private void GenerateBorders()
    {
        float mapWidth = roomWidth * numRoomsHor;
        float mapHeight = roomHeight * numRoomsVer;
        GameObject borders = new GameObject();
        borders.transform.parent = world;
        borders.name = "Borders of Map";

        //bottom Border
        GameObject bottomBorder = Instantiate(borderBlock, new Vector3((roomWidth * numRoomsHor) / 2, transform.position.y - 0.5f, 100), Quaternion.identity);
        bottomBorder.GetComponent<SpriteRenderer>().size = new Vector2(mapWidth + 2, 1);
        bottomBorder.GetComponent<BoxCollider2D>().size = new Vector2(mapWidth + 2, 1);
        bottomBorder.transform.parent = borders.transform;
        bottomBorder.layer = 9;

        //top Border
        GameObject topBorder = Instantiate(borderBlock, new Vector3((roomWidth * numRoomsHor) / 2, transform.position.y + roomHeight * numRoomsVer + 0.5f, 100), Quaternion.identity);
        topBorder.GetComponent<SpriteRenderer>().size = new Vector2(mapWidth + 2, 1);
        topBorder.GetComponent<BoxCollider2D>().size = new Vector2(mapWidth + 2, 1);
        topBorder.transform.parent = borders.transform;
        topBorder.layer = 9;

        //Right Border
        GameObject rightBorder = Instantiate(borderBlock, new Vector3(transform.position.x - 0.5f, (roomHeight * numRoomsVer) / 2, 100), Quaternion.identity);
        rightBorder.GetComponent<SpriteRenderer>().size = new Vector2(1, mapHeight);
        rightBorder.GetComponent<BoxCollider2D>().size = new Vector2(1, mapHeight);
        rightBorder.transform.parent = borders.transform;
        rightBorder.layer = 9;

        //Left Border
        GameObject leftBorder = Instantiate(borderBlock, new Vector3((roomWidth * numRoomsHor) + 0.5f, (roomHeight * numRoomsVer) / 2, 100), Quaternion.identity);
        leftBorder.GetComponent<SpriteRenderer>().size = new Vector2(1, mapHeight);
        leftBorder.GetComponent<BoxCollider2D>().size = new Vector2(1, mapHeight);
        leftBorder.transform.parent = borders.transform;
        leftBorder.layer = 9;
    }


    /// <summary>
    /// Function responsible for spawing entrace room on first row.
    /// </summary>
    private void SpawnEntranceRoom()
    {
        //pick entrance room location
        firstRoomPosition = Random.Range(0, numRoomsHor);

        int roomType = 0;
        Vector2 firstRoomSpawnPosition = new Vector2(transform.position.x + (firstRoomPosition * roomWidth), roomHeight * (numRoomsVer - 1));

        //spawn starting room at random location on first row
        GameObject entranceRoomObj = Instantiate(rooms[roomType], firstRoomSpawnPosition, Quaternion.identity);
        //sets name in object inspector
        entranceRoomObj.name = "Entrance";
        entranceRoom = entranceRoomObj;
        nextRoomPosition = firstRoomSpawnPosition;
        entranceRoomObj.transform.parent = world;

    }


    /// <summary>
    /// Spawns Door in room at the given position and changes its name in inspector
    /// </summary>
    /// <param name="roomPosition"></param>
    /// The bottom left position of the room to spawn door
    /// <param name="objectName"> </param>
    /// Name to set object in inspector
    /// <returns> The spawn location of the door</returns>

    private Vector3 SpawnDoor(Vector3 roomPosition, string objectName)
    {
        //offsets roomSpawnPosition to position of first block
        roomPosition += new Vector3(0.5f, 0.5f, 100);

        
        List<Vector3> potentialDoorSpawnLocations = new List<Vector3>();

        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight - 2; y++)
            {
                Vector3 rayCastOrigin = roomPosition + new Vector3(x, y, 0);
                //checks if there is a solid block below tile
                RaycastHit2D checkForAirHit = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(Vector2.up), 1f);
                //checks if there is a empty block above tile
                RaycastHit2D checkForSolidBlock = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(-Vector2.up), 0.6f, GroundTileLayer);

                if (checkForAirHit == false && checkForSolidBlock)
                {
                    potentialDoorSpawnLocations.Add(rayCastOrigin);
                }
            }
        }

        //select a random spawn location from all possible spawn Points
        int randDoor = Random.Range(0, potentialDoorSpawnLocations.Count);

        //spawns the door
        GameObject doorObj = Instantiate(door, (potentialDoorSpawnLocations[randDoor]), Quaternion.identity);

        //sets name to name given to function
        doorObj.name = objectName;
        doorObj.transform.parent = world;
        return potentialDoorSpawnLocations[randDoor];

    }
    /// <summary>
    /// Spawns treasure at random positions around the map where there is space
    /// </summary>
    private void SpawnTresure()
    {
        //creates a parent object to group all the treasure spawns
        GameObject TreasureParent = new GameObject();
        TreasureParent.name = "Treasure";

        //offsets startPosition to the position of first block
        Vector3 startPosition = new Vector3(0.5f, 0.5f, 5f);
       
        List<Vector3> potentialTreasureSpawnLocations = new List<Vector3>();

        for (int x = 0; x < roomWidth * numRoomsHor; x++)
        {
            for (int y = 0; y < (roomHeight*numRoomsVer) - 2; y++)
            {
                Vector3 rayCastOrigin = startPosition + new Vector3(x, y, 0);
                //checks if there is a solid block below the spawn position
                RaycastHit2D checkForAirHit = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(Vector2.up),0.4f);
                //checks if there is no block above the spawn position
                RaycastHit2D checkForSolidBlock = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(-Vector2.up), 0.6f, GroundTileLayer);

                if (checkForAirHit == false && checkForSolidBlock)
                {
                    potentialTreasureSpawnLocations.Add(rayCastOrigin);
                }
            }
        }

        
        int numSpawns = Mathf.Min(numTreasureToSpawn, potentialTreasureSpawnLocations.Count);
        for (int i = 0; i < numSpawns; i++)
        
        {
 
            int randPrefab = Random.Range(0, treasurePrefabs.Length);
            int randSpawnPoint = Random.Range(0, potentialTreasureSpawnLocations.Count);
            GameObject treasure = Instantiate(treasurePrefabs[randPrefab], potentialTreasureSpawnLocations[randSpawnPoint], Quaternion.identity);
            treasure.transform.parent = TreasureParent.transform;
            TreasureParent.transform.parent = world;
            treasure.layer = 11; //sets layer to Loot
            //remove this spawn location so we dont get multiple treasure in same spot
            potentialTreasureSpawnLocations.RemoveAt(randSpawnPoint);
        }
       

    }


    /// <summary>
    /// Spawns ground mobs at random positions around the map where there is space
    /// </summary>
    private void SpawnGroundMobs()
    {
        GameObject groundMobParent = new GameObject();
        groundMobParent.name = "Ground Mobs";
        Vector3 startPosition = new Vector3(0.5f, 0.5f, 0);
        List<Vector3> potentialMobSpawnLocations = new List<Vector3>();
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        for (int x = 0; x < roomWidth * numRoomsHor; x++)
        {
            for (int y = 0; y < (roomHeight * numRoomsVer) - 2; y++)
            {
                
                Vector3 rayCastOrigin = startPosition + new Vector3(x, y, 0);
                RaycastHit2D checkForAirHit = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(Vector2.up), 0.4f);
                RaycastHit2D checkForSolidBlock = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(-Vector2.up), 0.6f, GroundTileLayer);

                // Fiddly arithmetic, but it checks if the current region is within the players "spawn box". If true, then we do not add mob spawns here.
                bool isPlayerRoom = (playerX + preventMobRadius >= x || playerX - preventMobRadius <= x) && (playerY + preventMobRadius <= y || playerY - preventMobRadius >= y) ? false : true;

                if (checkForAirHit == false && checkForSolidBlock && isPlayerRoom == false)
                {
                    potentialMobSpawnLocations.Add(rayCastOrigin);
                }
            }
        }

        int numSpawns = Mathf.Min(numGroundMobsToSpawn, potentialMobSpawnLocations.Count);
        for (int i = 0; i < numSpawns; i++)

        {
            int randPrefab = Random.Range(0, groundMobPrefabs.Length);
            int randSpawnPoint = Random.Range(0, potentialMobSpawnLocations.Count);
            GameObject mob = Instantiate(groundMobPrefabs[randPrefab], potentialMobSpawnLocations[randSpawnPoint], Quaternion.identity);
            mob.transform.parent = groundMobParent.transform;
            mob.layer = 15;
            groundMobParent.transform.parent = world;
            
            potentialMobSpawnLocations.RemoveAt(randSpawnPoint);
        }


    }

    /// <summary>
    /// Spawns flying mobs at random positions around the map where there is space
    /// </summary>
    private void SpawnFlyingMobs()
    {
        GameObject flyingMobParent = new GameObject();
        flyingMobParent.name = "Flying Mobs";
        Vector3 startPosition = new Vector3(0.5f, 0.5f, 0);
        List<Vector3> potentialMobSpawnLocations = new List<Vector3>();
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        for (int x = 0; x < roomWidth * numRoomsHor; x++)
        {
            for (int y = 0; y < (roomHeight * numRoomsVer) - 2; y++)
            {
                Vector3 rayCastOrigin = startPosition + new Vector3(x, y, 0);
                RaycastHit2D checkForAirHit = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(Vector2.up), 0.4f);
                
                // Fiddly arithmetic, but it checks if the current region is within the players "spawn box". If true, then we do not add mob spawns here.
                bool isPlayerRoom = (playerX + preventMobRadius >= x || playerX - preventMobRadius <= x) && (playerY + preventMobRadius <= y || playerY - preventMobRadius >= y) ? false : true;

                if (checkForAirHit == false && isPlayerRoom == false)
                {
                    potentialMobSpawnLocations.Add(rayCastOrigin);
                }
            }
        }

        int numSpawns = Mathf.Min(numFlyingMobsToSpawn, potentialMobSpawnLocations.Count);
        for (int i = 0; i < numSpawns; i++)

        {
            int randPrefab = Random.Range(0, flyingMobPrefabs.Length);
            int randSpawnPoint = Random.Range(0, potentialMobSpawnLocations.Count);
            GameObject mob = Instantiate(flyingMobPrefabs[randPrefab], potentialMobSpawnLocations[randSpawnPoint], Quaternion.identity);
            mob.transform.parent = flyingMobParent.transform;
            flyingMobParent.transform.parent = world;
            mob.layer = 15;
            potentialMobSpawnLocations.RemoveAt(randSpawnPoint);
        }


    }




    /// <summary>
    /// Generates Critical path for room spawning and spawns the rooms along the path.
    /// </summary>
    private void GenerateNextMove()
    {
        //random chance to move in a direction
        int directionToMove = Random.Range(0, 100);
        //selects a random room type
        //int roomType = Random.Range(0, rooms.Length);
        int roomType = 2;
        Direction direction = Direction.ImpossibleMove;

        if (directionToMove > chanceToMoveDown)
        {
            //if we can either move left or right we choose randomly
            if ((canMoveLeft == true && nextRoomPosition.x > 0) && canMoveRight == true && nextRoomPosition.x < (roomWidth * (numRoomsHor - 1)))
            {
                int leftOrRight = Random.Range(0, 2);
                if (leftOrRight == 0)
                {
                    direction = Direction.Left;
                    canMoveRight = false;
                }
                else
                {
                    direction = Direction.Right;
                    canMoveLeft = false;
                }
            }
            //if we can only move left we set direction to left
            else if (canMoveLeft == true && nextRoomPosition.x > 0) //left
            {
                direction = Direction.Left;
                canMoveRight = false;
            }
            //if we can only move right we set direction to right
            else if (canMoveRight == true && nextRoomPosition.x < (roomWidth * (numRoomsHor - 1))) //right
            {
                direction = Direction.Right;
                canMoveLeft = false;
            }
            else
            {
                direction = Direction.Down;
                //if we move down we can move either left or right again next room
                canMoveLeft = true;
                canMoveRight = true;
            }

        }
        else
        {
            //if we move down we can move either left or right again next
            canMoveLeft = true;
            canMoveRight = true;
            direction = Direction.Down;
            roomType = Random.Range(0, 2);
        }


        if (direction == Direction.Left)
        {
            nextRoomPosition.x -= roomWidth;
            lastRoomWasBottom = false;
        }
        else if (direction == Direction.Right)
        {
            nextRoomPosition.x += roomWidth;
            lastRoomWasBottom = false;
        }
        else if (direction == Direction.Down)
        {

            /*checks if the last room spawned has a bottom exit, 
            if not we destory it and replace it with one that does */
            if (nextRoomPosition.y > 0)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(nextRoomPosition, 1, Room);
                if (roomDetection != null)
                {


                    if (roomDetection.GetComponent<RoomType>().roomType != RoomType.RoomEntrances.LRB &&
                        roomDetection.GetComponent<RoomType>().roomType != RoomType.RoomEntrances.LRTB)
                    {
                        roomDetection.GetComponent<RoomType>().destroyRoom();

                        int randomRoomWithBottom = Random.Range(0, 2);
                        if (randomRoomWithBottom == 0)
                        {
                            GameObject roomCreated = Instantiate(rooms[0], nextRoomPosition, Quaternion.identity);
                            roomCreated.name = "Critical Path Room";
                            roomCreated.transform.parent = world;
                        }
                        else
                        {
                            GameObject roomCreated = Instantiate(rooms[1], nextRoomPosition, Quaternion.identity);
                            roomCreated.name = "Critical Path Room";
                            roomCreated.transform.parent = world;
                        }

                    }
                }
            }


            nextRoomPosition.y -= roomHeight;
            lastRoomWasBottom = true;
            roomType = 0;

        }

        if (direction != Direction.ImpossibleMove)
        {
            //if me the next room we would spawn is outside of the bounds we end room creation
            if (nextRoomPosition.y < 0)
            {
                reachedExit = true;
                Debug.Log("Finished Critical Path Generation");
                lastRoomCreated.name = "Exit";
                exitRoom = lastRoomCreated;
                generateNonCriticalRooms();
            
            }
            else
            {
                //Creates the room 
                lastRoomCreated = Instantiate(rooms[roomType], nextRoomPosition, Quaternion.identity);
                lastRoomCreated.name = "Critical Path Room";

                lastRoomCreated.transform.parent = world;

            }
        }


    }
    /// <summary>
    /// Loops Over every room spawn position and spawns a room if its empty
    /// </summary>
    private void generateNonCriticalRooms()
    {
        for (int x = 0; x < numRoomsHor; x++)
        {
            for (int y = 0; y < numRoomsVer; y++)
            {
                Vector2 checkRoomPostion = new Vector2(x * roomWidth, y * roomHeight);
                Collider2D roomDetection = Physics2D.OverlapCircle(checkRoomPostion, 1, Room);
                int randomRoom = Random.Range(0, 4);
                if (roomDetection == null)
                {
                    GameObject fillerRoom = Instantiate(rooms[randomRoom], checkRoomPostion, Quaternion.identity);
                    fillerRoom.name = "Padding Room";
                    fillerRoom.transform.parent = world;
                }
            }
        }
    }
    /// <summary>
    /// Spawns a room at a given spawn point and the type of room to be spawned.
    /// Currently Unused
    /// </summary>
    /// <param name="roomSpawnPoint"></param>
    /// <param name="roomType"></param>  
    private void SpawnRoom(GameObject roomSpawnPoint, int roomType = 0)
    {
        Instantiate(rooms[roomType], roomSpawnPoint.transform.position, Quaternion.identity);

    }

   
    //Update is called once per frame
    void Update()
    {

        if (spawnedEntranceDoor == false && reachedExit == true)
        {
            Vector3 doorLocation =  SpawnDoor(entranceRoom.transform.position, "Entrance Door");
            spawnedEntranceDoor = true;
            player.transform.position = new Vector3(doorLocation.x, doorLocation.y, 0f);
            player.transform.rotation = Quaternion.identity;

        }

        if (spawnedExitDoor == false && reachedExit == true)
        {

            Vector3 doorLocation = SpawnDoor(exitRoom.transform.position, "Exit Door");
            spawnedExitDoor = true;

        }

        if(spawnedTresure == false && reachedExit == true)
        {
            SpawnTresure();
            spawnedTresure = true;
        }

        if (spawnedMobs == false && reachedExit == true)
        {
            SpawnGroundMobs();
            SpawnFlyingMobs();
            spawnedMobs = true;
        }

        if (timeElapsedSinceLastRoom <= 0 && reachedExit == false)
        {
            GenerateNextMove();
            timeElapsedSinceLastRoom = timeBetweenRoomSpawns;
        }
        else
        {
            timeElapsedSinceLastRoom -= Time.deltaTime;
        }
    }


    /// <summary>
    /// Resets what has already been spawned and recalculated the number of mobs to spawn based upon the new difficulty
    /// </summary>
    public void resetWorldSpawnVariables()
    {
        reachedExit = false;
        spawnedMobs = false;
        spawnedTresure = false;
        spawnedExitDoor = false;
        spawnedEntranceDoor = false;

        float difficulty = MapVariables.calcDifficulty();
        numGroundMobsToSpawn = (int)(numGroundMobsToSpawn * difficulty);
        numFlyingMobsToSpawn = (int)(numFlyingMobsToSpawn * difficulty);

    


    }


}
