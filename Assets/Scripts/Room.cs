using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public bool closeWhenEntered;

    public GameObject[] doors;

    public bool roomActive;

    public GameObject mapHider;


    public void OpenDoors()
    {
        foreach (GameObject door in doors)
            door.SetActive(false);

        closeWhenEntered = false;
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            CameraController.instance.ChangeTarget(transform);

            roomActive = true;

            if (closeWhenEntered)
                foreach (GameObject door in doors)
                    door.SetActive(true);

            mapHider.SetActive(false);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
            roomActive = false;
    }
}
