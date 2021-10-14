using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRB;
    public float moveSpeed;

    [Header("CHASING ENEMY")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    [Header("RUN AWAY ENEMY")]
    public bool shouldRunAway;
    public float runAwayRange;

    [Header("WANDERING ENEMY")]
    public bool shouldWander;
    public float wanderLength;
    private float wanderCounter;
    public float pauseLength;
    private float pauseCounter;
    private Vector3 wanderDirection;

    [Header("PATROLING ENEMY")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    [Header("ANIMATION")]
    public Animator anim;

    [Header("HEALTH")]
    public int health = 150;

    public GameObject enemyHitEffect;
    public GameObject[] deathSplatters;

    [Header("SHOOTING")]
    public bool shouldShoot;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
    public float shootRange;

    [Header("SPRITE RENDERER")]
    public SpriteRenderer theBody;

    [Header("DROPPING ITEMS")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    [Header("SFX")]
    public int enemyDeathSFX = 1;
    public int enemyDamageSFX = 2;
    public int enemyShootSFX = 12;


    private void Start()
    {
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }
    }


    private void Update()
    {
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            SetVelocity();

            FaceThePlayer();

            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= shootRange)
                Shoot();
        }
        else if (!PlayerController.instance.gameObject.activeInHierarchy)
        {
            Stop();
        }

        anim.SetBool("isMoving", moveDirection != Vector3.zero);
    }


    private void SetVelocity()
    {
        moveDirection = Vector3.zero;

        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
        {
            moveDirection = PlayerController.instance.transform.position - transform.position;
        }
        else
        {
            if (shouldWander)
            {
                if (wanderCounter > 0)
                {
                    wanderCounter -= Time.deltaTime;

                    moveDirection = wanderDirection;

                    if (wanderCounter <= 0)
                    {
                        pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                    }
                }

                if (pauseCounter > 0)
                {
                    pauseCounter -= Time.deltaTime;

                    if (pauseCounter <= 0)
                    {
                        wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                        wanderDirection = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                    }
                }
            }

            if (shouldPatrol)
            {
                moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                    currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
            }
        }

        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runAwayRange && shouldRunAway)
        {
            moveDirection = transform.position - PlayerController.instance.transform.position;
        }

        moveDirection.Normalize();

        theRB.velocity = moveDirection * moveSpeed;
    }


    private void Stop()
    {
        moveDirection = Vector3.zero;
        theRB.velocity = Vector3.zero;
    }


    private void FaceThePlayer()
    {
        Vector3 playerPosition = PlayerController.instance.transform.position;

        if (playerPosition.x < transform.position.x)
        {
            transform.localScale = Vector3.one;
            firePoint.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            firePoint.localScale = new Vector3(-1f, -1f, 1f);
        }
    }


    private void Shoot()
    {
        fireCounter -= Time.deltaTime;

        if (fireCounter <= 0)
        {
            fireCounter = fireRate;
            AudioManager.instance.PlaySFX(enemyShootSFX);
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }


    public void DamageEnemy(int damage)
    {
        health -= damage;

        AudioManager.instance.PlaySFX(enemyDamageSFX);

        Instantiate(enemyHitEffect, transform.position, transform.rotation);

        anim.SetTrigger("hit");

        if (health <= 0)
            KillEnemy();
    }


    private void KillEnemy()
    {
        Instantiate(
            deathSplatters[Random.Range(0, deathSplatters.Length)],
            transform.position,
            Quaternion.Euler(0f, 0f, Random.Range(0, 4) * 90f));

        AudioManager.instance.PlaySFX(enemyDeathSFX);

        Destroy(gameObject);

        if (shouldDropItem)
        {
            if (Random.Range(0f, 100f) > itemDropPercent)
                return;

            Instantiate(itemsToDrop[Random.Range(0, itemsToDrop.Length)], transform.position, transform.rotation);
        }
    }
}
