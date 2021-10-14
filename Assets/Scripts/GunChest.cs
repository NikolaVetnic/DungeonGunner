using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{

    public GunPickup[] potentialGuns;

    public SpriteRenderer theSR;
    public Sprite chestOpen;

    public GameObject notification;

    private bool canOpen;
    private bool isOpen;

    public float scaleSpeed = 2f;

    public Transform spawnPoint;


    private void Update()
    {
        if (canOpen && !isOpen && Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(potentialGuns[Random.Range(0, potentialGuns.Length)], spawnPoint.position, spawnPoint.rotation);
            theSR.sprite = chestOpen;
            isOpen = true;

            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        }

        if (isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            notification.SetActive(true);
            canOpen = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            notification.SetActive(false);
            canOpen = false;
        }
    }
}
