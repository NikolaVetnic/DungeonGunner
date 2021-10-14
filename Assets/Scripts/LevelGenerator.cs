using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{

    public GameObject layoutRoom;

    public Color startColor;
    public Color endColor;
    public Color shopColor;
    public Color gunColor;
    public Color neutralColor;

    public int distanceToEnd;

    public bool includeShop;
    private int shopSelector = -1;
    public double minDistanceToShop;
    public double maxDistanceToShop;

    public bool includeGunRoom;
    private int gunRoomSelector = -1;
    public double minDistanceToGunRoom;
    public double maxDistanceToGunRoom;

    public Transform generatorPoint;

    public enum Direction { up, right, down, left };
    public Direction selectDirection;

    public float xOffset = 18f;
    public float yOffset = 10f;

    public LayerMask whatIsRoom;

    private GameObject startRoom;
    private GameObject endRoom;
    private GameObject shopRoom;
    private GameObject gunRoom;
    private List<GameObject> layoutRoomObjects = new List<GameObject>();
    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomPrefabs rooms;

    public RoomCenter centerStart;
    public RoomCenter centerEnd;
    public RoomCenter centerShop;
    public RoomCenter centerGunRoom;
    public RoomCenter[] potentialCenters;


    private void Start()
    {
        bool minLength0 = !includeShop && !includeGunRoom && distanceToEnd > 1;
        bool minLength1 = (includeShop ^  includeGunRoom) && distanceToEnd > 2;
        bool minLength2 = (includeShop && includeGunRoom) && distanceToEnd > 3;

        if (!minLength0 && !minLength1 && !minLength2)
            throw new System.ArgumentOutOfRangeException("Distance to end parameter is too low!");

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject currentRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            if (i == 0)
            {
                currentRoom.GetComponent<SpriteRenderer>().color = startColor;
                startRoom = currentRoom;
            }
            else if (i == distanceToEnd - 1)
            {
                currentRoom.GetComponent<SpriteRenderer>().color = endColor;
                endRoom = currentRoom;
            }
            else
            {
                currentRoom.GetComponent<SpriteRenderer>().color = neutralColor;
                layoutRoomObjects.Add(currentRoom);
            }

            selectDirection = (Direction) Random.Range(0, 4);

            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                MoveGenerationPoint();
        }

        if (includeShop)
        {
            this.shopSelector = Random.Range(
                (int) (minDistanceToShop * layoutRoomObjects.Count),
                (int) (maxDistanceToShop * layoutRoomObjects.Count) + 1);

            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);

            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        if (includeGunRoom)
        {
            int attempt = 0;

            do { this.gunRoomSelector = Random.Range(
                (int) (minDistanceToGunRoom * layoutRoomObjects.Count),
                (int) (maxDistanceToGunRoom * layoutRoomObjects.Count) + 1);

                attempt++;
                
            } while (includeShop && gunRoomSelector == shopSelector && attempt < 1000);

            if (gunRoomSelector == -1)
                gunRoomSelector = layoutRoomObjects.Count - 1;

            gunRoom = layoutRoomObjects[gunRoomSelector];
            layoutRoomObjects.RemoveAt(gunRoomSelector);

            gunRoom.GetComponent<SpriteRenderer>().color = gunColor;
        }

        // create room outlines
        CreateRoomOutline(Vector3.zero);

        foreach (GameObject room in layoutRoomObjects)
            CreateRoomOutline(room.transform.position);

        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
            CreateRoomOutline(shopRoom.transform.position);

        if (includeGunRoom)
            CreateRoomOutline(gunRoom.transform.position);

        foreach (GameObject outline in generatedOutlines)
        {
            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation)
                    .theRoom = outline.GetComponent<Room>();

                continue;
            }

            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation)
                    .theRoom = outline.GetComponent<Room>();

                continue;
            }

            if (includeShop && outline.transform.position == shopRoom.transform.position)
            {
                Instantiate(centerShop, outline.transform.position, transform.rotation)
                    .theRoom = outline.GetComponent<Room>();

                continue;
            }

            if (includeGunRoom && outline.transform.position == gunRoom.transform.position)
            {
                Instantiate(centerGunRoom, outline.transform.position, transform.rotation)
                    .theRoom = outline.GetComponent<Room>();

                continue;
            }

            Instantiate(potentialCenters[Random.Range(0, potentialCenters.Length)],
                outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
        }
    }


    private void Update()
    {
        #if !UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        #endif
    }


    public void MoveGenerationPoint()
    {
        switch (selectDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, +yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(+xOffset, 0f, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }


    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom);
        bool roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, whatIsRoom);

        int directionCount = 0;

        directionCount = roomUp ? directionCount + 1 : directionCount;
        directionCount = roomDown ? directionCount + 1 : directionCount;
        directionCount = roomRight ? directionCount + 1 : directionCount;
        directionCount = roomLeft ? directionCount + 1 : directionCount;

        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found no room exits!");
                break;

            case 1:
                if (roomUp)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }

                if (roomDown)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }

                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }

                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }

                break;

            case 2:
                if (roomUp && roomDown)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }

                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }

                if (roomUp && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }

                if (roomLeft && roomDown)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }

                if (roomDown && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }

                if (roomRight && roomUp)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }

                break;

            case 3:
                if (roomUp && roomRight && roomDown)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }

                if (roomRight && roomDown && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }

                if (roomDown && roomLeft && roomUp)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }

                if (roomLeft && roomUp && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }

                break;

            case 4:
                generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));

                break;
        }
    }
}


[System.Serializable]
public class RoomPrefabs
{
    public GameObject
        singleRight, singleUp, singleLeft, singleDown,
        doubleLeftRight, doubleUpDown, doubleUpRight, doubleRightDown,
        doubleDownLeft, doubleLeftUp, tripleUpRightDown, tripleRightDownLeft,
        tripleDownLeftUp, tripleLeftUpRight, fourway;
}