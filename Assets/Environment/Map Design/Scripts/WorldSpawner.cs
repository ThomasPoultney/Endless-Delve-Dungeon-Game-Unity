using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{

    [SerializeField]
    private float roomHeight;
    [SerializeField]
    private float roomWidth;
    [SerializeField]
    private int numRoomsHor;
    [SerializeField]
    private int numRoomsVer;

    [SerializeField]
    private int chanceToMoveDown = 10; //percentage chance for critical path to move down
    [SerializeField]
    private float timeBetweenRoomSpawns = 2.0f;

    [SerializeField]
    private GameObject[] rooms; //room templates for each room type
    [SerializeField]
    private GameObject borderBlock;


    private float timeElapsedSinceLastRoom; 
    private int firstRoomPosition;
    private bool reachedExit = false;
    private Vector2 nextRoomPosition;

    private bool canMoveRight = true;
    private bool canMoveLeft = true;
    private bool lastRoomWasBottom = false;
    private enum Direction { Down, Left, Right, ImpossibleMove };

    [SerializeField]
    private LayerMask Room;
    [SerializeField]
    private LayerMask GroundTileLayer;

    private GameObject lastRoomCreated;
    [SerializeField]
    private GameObject door; //the door prefab

    private GameObject entranceRoom;
    private GameObject exitRoom;

    //variables to check what has been spawned
    private bool spawnedEntranceDoor = false;
    private bool spawnedExitDoor = false;
    private bool spawnedMobs = false;
    private bool spawnedDoor;
    private bool spawnedTresure = false;


    [SerializeField]
    private GameObject[] treasurePrefabs;
    [SerializeField]
    private GameObject[] groundMobPrefabs;
    [SerializeField]
    private GameObject[] flyingMobPrefabs;

    public GameObject player;

    [SerializeField]
    private int numTreasureToSpawn = 0;
    [SerializeField]
    private int numGroundMobsToSpawn = 0;
    [SerializeField]
    private int numFlyingMobsToSpawn = 0;




    // Start is called before the first frame update
    void Start()
    {
        GenerateBorders();
        SpawnEntranceRoom();
    }
    /// <summary>
    /// Generates the one tile border around the map
    /// </summary>
    private void GenerateBorders()
    {
        float mapWidth = roomWidth * numRoomsHor;
        float mapHeight = roomHeight * numRoomsVer;
        GameObject borders = new GameObject();
        borders.name = "Borders of Map";

        //bottom Border
        GameObject bottomBorder = Instantiate(borderBlock, new Vector2((roomWidth * numRoomsHor) / 2, transform.position.y - 0.5f), Quaternion.identity);
        bottomBorder.GetComponent<SpriteRenderer>().size = new Vector2(mapWidth + 2, 1);
        bottomBorder.transform.parent = borders.transform;

        //top Border
        GameObject topBorder = Instantiate(borderBlock, new Vector2((roomWidth * numRoomsHor) / 2, transform.position.y + roomHeight * numRoomsVer + 0.5f), Quaternion.identity);
        topBorder.GetComponent<SpriteRenderer>().size = new Vector2(mapWidth + 2, 1);
        topBorder.transform.parent = borders.transform;

        //Right Border
        GameObject rightBorder = Instantiate(borderBlock, new Vector2(transform.position.x - 0.5f, (roomHeight * numRoomsVer) / 2), Quaternion.identity);
        rightBorder.GetComponent<SpriteRenderer>().size = new Vector2(1, mapHeight);
        rightBorder.transform.parent = borders.transform;

        //Left Border
        GameObject leftBorder = Instantiate(borderBlock, new Vector2((roomWidth * numRoomsHor) + 0.5f, (roomHeight * numRoomsVer) / 2), Quaternion.identity);
        leftBorder.GetComponent<SpriteRenderer>().size = new Vector2(1, mapHeight);
        leftBorder.transform.parent = borders.transform;
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

       

    }


    /// <summary>
    /// Spawns Door in room at the given position and changes its name in inspector
    /// </summary>
    /// <param name="roomPosition">The bottom left position of the room to spawn door</param>
    /// <param name="objectName"> Name to set object in inspector</param>
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
        Vector3 startPosition = new Vector3(0.5f, 0.5f, 0);
       
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

        for (int x = 0; x < roomWidth * numRoomsHor; x++)
        {
            for (int y = 0; y < (roomHeight * numRoomsVer) - 2; y++)
            {
                Vector3 rayCastOrigin = startPosition + new Vector3(x, y, 0);
                RaycastHit2D checkForAirHit = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(Vector2.up), 0.4f);
                RaycastHit2D checkForSolidBlock = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(-Vector2.up), 0.6f, GroundTileLayer);

                if (checkForAirHit == false && checkForSolidBlock)
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

        for (int x = 0; x < roomWidth * numRoomsHor; x++)
        {
            for (int y = 0; y < (roomHeight * numRoomsVer) - 2; y++)
            {
                Vector3 rayCastOrigin = startPosition + new Vector3(x, y, 0);
                RaycastHit2D checkForAirHit = Physics2D.Raycast(rayCastOrigin, transform.TransformDirection(Vector2.up), 0.4f);
               

                if (checkForAirHit == false)
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
                        }
                        else
                        {
                            GameObject roomCreated = Instantiate(rooms[1], nextRoomPosition, Quaternion.identity);
                            roomCreated.name = "Critical Path Room";
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

            }
        }


    }
    /// <summary>
    /// Loops Over every room position and spawns a room if its empty
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

   
    // Update is called once per frame
    void Update()
    {

        if (spawnedEntranceDoor == false && reachedExit == true)
        {
            Vector3 doorLocation =  SpawnDoor(entranceRoom.transform.position, "Entrance Door");
            spawnedEntranceDoor = true;
            player.transform.position = doorLocation;
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


}
