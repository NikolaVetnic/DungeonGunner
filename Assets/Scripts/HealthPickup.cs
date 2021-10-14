using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healAmount;
    public float waitToBeCollected = .5f;

    public int pickupHealthSFX = 7;


    private void Update()
    {
        if (waitToBeCollected > 0)
            waitToBeCollected -= Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player") && waitToBeCollected <= 0)
            if (PlayerHealthController.instance.currentHealth < PlayerHealthController.instance.maxHealth)
                UseHealthPickup();
    }


    private void UseHealthPickup()
    {
        PlayerHealthController.instance.HealPlayer(healAmount);
        AudioManager.instance.PlaySFX(pickupHealthSFX);
        Destroy(gameObject);
    }
}
