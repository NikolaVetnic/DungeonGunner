using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    /*
     * When we make a static var like this, one thing to know - althou-
     * gh we made it public it will not be visible in Unity, because it
     * is static; static variable is something that will be set for all
     * versions of the PlayerController scripts in Unity.
     * 
     * The reason for this is that we can now access the instance of t-
     * he player object - the only instance allowed! - by simply acces-
     * sing a static field of the class, i.e. we can do:
     * 
     * PlayerController.instance;
     */


    public static PlayerController instance;

    public string charName;
    public int playerToSpawnIdx;

    [HideInInspector] public bool canMove = true;

    private float activeMoveSpeed;
    public float moveSpeed;

    [Header("DASH")]
    public float dashSpeed = 8f;
    public float dashLength = .5f;
    public float dashCooldown = 1f;
    public float dashInvincibility = .5f;
    private float dashCounter;
    private float dashCoolCounter;

    private Vector2 moveInput;

    public Rigidbody2D theRB;

    [Header("GUNS")]
    public Transform gunHand;

    public List<Gun> availableGuns = new List<Gun>();
    public int currentGun;

    [Header("CROWD CONTROL ATTACK")]
    public List<EnemyController> enemiesTooCloseToThePlayer = new List<EnemyController>();
    public int crowdControlDamage = 50;
    public int crowdControlPrice = 1;
    public GameObject crowdControlEffect;
    public float crowdControlCooldown = 1.0f;
    private float crowdControlCount;
    public float knockbackLength;
    public float knockbackDuration;

    public Animator anim;

    public SpriteRenderer bodySR;

    public int playerDashSFX = 8;
    public int playerCrowdControlSFX = 8;


    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        activeMoveSpeed = moveSpeed;

        SwitchGun();
    }


    private void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            SetVelocity();
            Dash();

            if (Input.GetKeyDown(KeyCode.Tab))
                if (availableGuns.Count > 0)
                {
                    currentGun = currentGun + 1 >= availableGuns.Count ? 0 : currentGun + 1;
                    SwitchGun();
                }
                else
                {
                    Debug.LogError("Player has no guns!");
                }

            anim.SetBool("isMoving", moveInput != Vector2.zero);

            if (crowdControlCount > 0)
                crowdControlCount -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R) && crowdControlCount <= 0)
                FireCrowdControl();
        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }

        if (!LevelManager.instance.isPaused)
            FaceTheCursor();
    }


    #region CROWD CONTROL SHOT

    private void FireCrowdControl()
    {
        crowdControlCount = crowdControlCooldown;

        Instantiate(crowdControlEffect, transform.position, transform.rotation);
        PlayerHealthController.instance.DamagePlayer(crowdControlPrice);
        AudioManager.instance.PlaySFX(playerCrowdControlSFX);

        if (enemiesTooCloseToThePlayer.Count > 0)
            for (int i = enemiesTooCloseToThePlayer.Count - 1; i > -1; i--)
            {
                //Vector3 knockbackDir = enemiesTooCloseToThePlayer[i].transform.position - PlayerController.instance.transform.position;
                //knockbackDir = knockbackDir.normalized;
                StartCoroutine(knockBackCoroutine(
                    enemiesTooCloseToThePlayer[i].gameObject,
                    (enemiesTooCloseToThePlayer[i].transform.position - PlayerController.instance.transform.position).normalized,
                    knockbackLength,
                    knockbackDuration));

                enemiesTooCloseToThePlayer[i].DamageEnemy(crowdControlDamage);
            }
    }


    IEnumerator knockBackCoroutine(GameObject target, Vector3 direction, float length, float duration)
    {
        float timeleft = duration;

        while (target != null && timeleft > 0)
        {
            if (timeleft > Time.deltaTime)
                target.transform.Translate(direction * Time.deltaTime / duration * length);
            else
                target.transform.Translate(direction * timeleft / duration * length);

            timeleft -= Time.deltaTime;

            yield return null;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy") && !enemiesTooCloseToThePlayer.Contains(other.gameObject.GetComponent<EnemyController>()))
            enemiesTooCloseToThePlayer.Add(other.gameObject.GetComponent<EnemyController>());
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
            enemiesTooCloseToThePlayer.Remove(other.gameObject.GetComponent<EnemyController>());
    }

    #endregion


    private void SetVelocity()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        theRB.velocity = moveInput * activeMoveSpeed;
    }


    private void FaceTheCursor()
    {
        /*
         * When we make a static var like this, one thing to know - althou-
         * Unity contacted me saying: "After further investigation this tu-
         * rns out to be by design. Unity intentionally doesn't let you ch-
         * ange the value of a property you are animating. What is happeni-
         * ng here is you're changing the local scale value of your object,
         * but since you're in play mode and the object is animated, it wi-
         * ll be overwritten back to the animated value. As a workaround it
         * can be suggested that you to either implement it as an actual s-
         * tate machine state, or disable animator and gain back control on
         * its object values."
         * 
         * I.e. YOU CANNOT USE SCALE IN ANIMATION IF YOU ARE USING IT TO T-
         * URN THE PLAYER AROUND GOD FUCKING DAMN IT!
         */

        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

        if (mousePosition.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunHand.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunHand.localScale = Vector3.one;
        }

        // rotate gun hand
        Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        gunHand.rotation = Quaternion.Euler(0, 0, angle);
    }


    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;

                AudioManager.instance.PlaySFX(playerDashSFX);

                PlayerHealthController.instance.ActivateInvincibility(dashInvincibility);

                anim.SetTrigger("dash");
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }


    public bool IsDashing()
    {
        return dashCounter > 0;
    }


    public void SwitchGun()
    {
        foreach (Gun gun in availableGuns)
            gun.gameObject.SetActive(false);

        availableGuns[currentGun].gameObject.SetActive(true);
        UIController.instance.gunImage.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunName.text = availableGuns[currentGun].gunName;
    }
}
