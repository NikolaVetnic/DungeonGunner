using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public static BossController instance;

    public string bossName;

    public BossAction[] actions;
    private int currAction;
    private float actionCounter;

    public BossSequence[] sequences;
    public int currSequence;

    private float shotCounter;
    public GameObject bossHitEffect;

    private Vector2 moveDirection;
    public Rigidbody2D theRB;

    public int health;

    public GameObject deathSplatter;
    public GameObject levelExit;

    public int bossDamageSFX;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        actions = sequences[currSequence].actions;
        actionCounter = actions[currAction].actionLength;
        SetBossUI();
    }


    private void Update()
    {
        if (actionCounter > 0)
        {
            actionCounter -= Time.deltaTime;

            HandleMovement();
            HandleShooting();
        }
        else
        {
            currAction = currAction + 1 < actions.Length ? currAction + 1 : 0;
            actionCounter = actions[currAction].actionLength;
        }
    }


    private void SetBossUI()
    {
        UIController.instance.bossName.text = bossName;
        UIController.instance.bossHealthBar.maxValue = health;
        UIController.instance.bossHealthBar.value = health;
    }


    private void HandleMovement()
    {
        moveDirection = Vector2.zero;

        if (actions[currAction].shouldMove)
        {
            if (actions[currAction].shouldChasePlayer)
                moveDirection = PlayerController.instance.transform.position - transform.position;

            if (actions[currAction].shouldMoveToPoints && Vector3.Distance(transform.position, actions[currAction].pointToMoveTo.position) > 0.5f)
                moveDirection = actions[currAction].pointToMoveTo.position - transform.position;
        }

        moveDirection.Normalize();
        theRB.velocity = moveDirection * actions[currAction].moveSpeed;
    }


    private void HandleShooting()
    {
        if (actions[currAction].shouldShoot)
        {
            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                shotCounter = actions[currAction].timeBetweenShots;

                foreach (Transform shotPoint in actions[currAction].shotPoints)
                    Instantiate(actions[currAction].itemToShoot, shotPoint.position, shotPoint.rotation);
            }
        }
    }


    public void DamageBoss(int damage)
    {
        health -= damage;

        AudioManager.instance.PlaySFX(bossDamageSFX);

        GameObject largeBossHitEffect = Instantiate(bossHitEffect, transform.position, transform.rotation);
        largeBossHitEffect.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        if (health <= 0)
            KillBoss();
        else if (health <= sequences[currSequence].endSequenceHealth && currSequence < sequences.Length - 1)
            ChangeSequence();

        UIController.instance.bossHealthBar.value = health;
    }


    private void ChangeSequence()
    {
        actions = sequences[++currSequence].actions;
        currAction = 0;
        actionCounter = actions[currAction].actionLength;
    }


    private void KillBoss()
    {
        gameObject.SetActive(false);
        Instantiate(deathSplatter, transform.position, transform.rotation);

        UIController.instance.bossHealthBar.gameObject.SetActive(false);

        if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2.0f)
            levelExit.transform.position += new Vector3(4.0f, 0.0f, 0.0f);
        levelExit.SetActive(true);
    }
}


[System.Serializable]
public class BossAction
{

    [Header("ACTION")]
    public float actionLength;

    public bool shouldMove;
    public bool shouldChasePlayer;
    public bool shouldMoveToPoints;
    public Transform pointToMoveTo;
    public float moveSpeed;

    public bool shouldShoot;
    public GameObject itemToShoot;
    public float timeBetweenShots;
    public Transform[] shotPoints;
}


[System.Serializable]
public class BossSequence
{

    [Header("SEQUENCE")]
    public BossAction[] actions;

    public int endSequenceHealth;
}