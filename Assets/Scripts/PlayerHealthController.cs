using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{

    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public GameObject playerHitEffect;

    public float invincibilityDuration = 1.0f;
    private float invincibilityCount;

    private Color vulnerableColor;
    private Color invincibleColor;

    public int playerDeathSFX = 9;
    public int playerHurtSFX = 11;

    public float waitAfterDeath = 2f;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;

        vulnerableColor = new Color(
                PlayerController.instance.bodySR.color.r,
                PlayerController.instance.bodySR.color.g,
                PlayerController.instance.bodySR.color.b, 1.00f);

        invincibleColor = new Color(
                PlayerController.instance.bodySR.color.r,
                PlayerController.instance.bodySR.color.g,
                PlayerController.instance.bodySR.color.b, 0.35f);

        UIController.instance.healthSlider.maxValue = maxHealth;
        UpdateCurrentHealth();
    }


    private void Update()
    {
        if (invincibilityCount > 0)
            CountDownInvincibility();
    }


    private void CountDownInvincibility()
    {
        invincibilityCount -= Time.deltaTime;

        if (invincibilityCount <= 0)
            PlayerController.instance.bodySR.color = vulnerableColor;
    }


    public void DamagePlayer(int damage)
    {
        if (invincibilityCount > 0)
            return;

        AudioManager.instance.PlaySFX(playerHurtSFX);

        currentHealth = currentHealth - damage < 0 ? 0 : currentHealth - damage;
        UpdateCurrentHealth();
        PlayerController.instance.anim.SetTrigger("hit");

        ActivateInvincibility();

        Instantiate(playerHitEffect, PlayerController.instance.transform.position, PlayerController.instance.transform.rotation);

        if (currentHealth <= 0)
            KillPlayer();
    }


    private void KillPlayer()
    {
        PlayerController.instance.gameObject.SetActive(false);
        AudioManager.instance.PlaySFX(playerDeathSFX);
        AudioManager.instance.PlayGameOver();

        StartCoroutine(GameOver());
    }


    public IEnumerator GameOver()
    {
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(waitAfterDeath);
        UIController.instance.deathScreen.SetActive(true);
    }


    public void ActivateInvincibility(float length)
    {
        invincibilityCount = length;
        PlayerController.instance.bodySR.color = invincibleColor;
    }


    public void ActivateInvincibility()
    {
        ActivateInvincibility(invincibilityDuration);
    }


    public bool IsInvincible()
    {
        return invincibilityCount <= 0;
    }


    private void UpdateCurrentHealth()
    {
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth + " / " + maxHealth;
    }


    public void HealPlayer(int healAmount)
    {
        currentHealth = currentHealth + healAmount > maxHealth ? maxHealth : currentHealth + healAmount;
        UpdateCurrentHealth();
    }


    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UpdateCurrentHealth();
    }
}
