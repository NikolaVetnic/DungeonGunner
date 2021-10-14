using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    public int coinValue = 1;

    public float waitToBeCollected = .5f;

    public int pickupCoinSFX = 5;


    private void Update()
    {
        if (waitToBeCollected > 0)
            waitToBeCollected -= Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player") && waitToBeCollected <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);
            AudioManager.instance.PlaySFX(pickupCoinSFX);
            Destroy(gameObject);
        }
    }


}
