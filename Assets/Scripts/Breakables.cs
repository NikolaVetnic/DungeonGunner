using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{

    public GameObject[] brokenPieces;
    public int maxPieces = 3;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    public int boxBreakingSFX;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("PlayerBullet") || other.gameObject.tag.Equals("EnemyBullet"))
            DestroyBreakable();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player") && PlayerController.instance.IsDashing())
            DestroyBreakable();
    }


    private void DestroyBreakable()
    {
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(boxBreakingSFX);

        int numPieces = Random.Range(1, maxPieces);

        for (int i = 0; i < numPieces; i++)
            Instantiate(brokenPieces[Random.Range(0, brokenPieces.Length)], transform.position, transform.rotation);

        if (shouldDropItem)
        {
            if (Random.Range(0f, 100f) > itemDropPercent)
                return;

            Instantiate(itemsToDrop[Random.Range(0, itemsToDrop.Length)], transform.position, transform.rotation);
        }
    }
}
