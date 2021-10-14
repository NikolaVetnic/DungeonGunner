using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    public Gun theGun;

    public float waitToBeCollected = .5f;

    public int pickupGunSFX = 7;


    private void Update()
    {
        if (waitToBeCollected > 0)
            waitToBeCollected -= Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Equals("Player"))
            return;

        foreach (Gun gun in PlayerController.instance.availableGuns)
            if (gun.gunName.Equals(theGun.gunName))
                return;

        BuyWeapon();

        AudioManager.instance.PlaySFX(pickupGunSFX);
        Destroy(gameObject);
    }


    private void BuyWeapon()
    {
        Gun newGun = Instantiate(theGun, Vector3.zero,
                    PlayerController.instance.transform.rotation);
        newGun.transform.SetParent(                                             // place the gun where it should be in hierarchy
            PlayerController.instance.gunHand, false);

        PlayerController.instance.availableGuns.Add(newGun);
        PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
        PlayerController.instance.SwitchGun();
    }
}
