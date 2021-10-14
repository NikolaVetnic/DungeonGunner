using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public GameObject buyMessage;

    private bool inBuyZone = false;

    public bool isHealthRestore;
    public bool isHealthUpgrade;
    public bool isWeapon;

    public int itemCost;

    public int healthUpgradeAmount;

    public int shopBuySFX;
    public int shopNotEnoughSFX;

    public Gun[] potentialGuns;
    private Gun theGun;
    public SpriteRenderer gunSprite;
    public Text infoText;


    private void Start()
    {
        if (isWeapon)
        {
            theGun = potentialGuns[Random.Range(0, potentialGuns.Length)];

            gunSprite.sprite = theGun.gunShopSprite;
            infoText.text = theGun.gunName + "\n - " + theGun.itemCost + " Gold - ";
            itemCost = theGun.itemCost;
        }
    }


    private void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (LevelManager.instance.currentCoins >= itemCost)
                {
                    LevelManager.instance.SpendCoins(itemCost);

                    if (isHealthUpgrade)
                        PlayerHealthController.instance
                            .IncreaseMaxHealth(healthUpgradeAmount);

                    if (isHealthRestore)
                        PlayerHealthController.instance
                            .HealPlayer(PlayerHealthController.instance.maxHealth);

                    if (isWeapon)
                        BuyWeapon();

                    gameObject.SetActive(false);
                    inBuyZone = false;

                    AudioManager.instance.PlaySFX(shopBuySFX);
                }
                else
                {
                    AudioManager.instance.PlaySFX(shopNotEnoughSFX);
                }
            }
        }
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            buyMessage.SetActive(true);
            inBuyZone = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            buyMessage.SetActive(false);
            inBuyZone = false;
        }
    }
}
