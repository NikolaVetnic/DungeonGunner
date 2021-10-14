using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public string gunName;
    public Sprite gunUI;

    public GameObject bulletToFire;
    public Transform firePoint;
    public GameObject muzzleFlashEffect;

    public float timeBetweenShots;
    private float shotCounter;

    public int itemCost;
    public Sprite gunShopSprite;

    public int playerShootSFX = 17;


    private void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            if (shotCounter > 0)
                shotCounter -= Time.deltaTime;
            else
                Fire();
        }
    }


    private void Fire()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            InstantiatePlayerBullet();
    }


    private void InstantiatePlayerBullet()
    {
        Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
        Instantiate(muzzleFlashEffect, firePoint.position, firePoint.rotation);
        AudioManager.instance.PlaySFX(playerShootSFX);
        shotCounter = timeBetweenShots;
    }
}
