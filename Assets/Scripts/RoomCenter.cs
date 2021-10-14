using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{

    public bool openWhenEnemiesCleared;

    public List<GameObject> enemies = new List<GameObject>();

    public Room theRoom;


    private void Start()
    {
        GetEnemiesFromHierarchy();

        if (openWhenEnemiesCleared)
            theRoom.closeWhenEntered = true;
    }


    private void Update()
    {
        if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
        {
            UIController.instance.miniMap.SetActive(false);

            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }

            if (enemies.Count == 0)
            {
                theRoom.OpenDoors();
                UIController.instance.miniMap.SetActive(true);
            }
        }
    }


    public void GetEnemiesFromHierarchy()
    {
        foreach (Transform enemyTransform in transform.GetChild(3))
            enemies.Add(enemyTransform.gameObject);
    }
}
