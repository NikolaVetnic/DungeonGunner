using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public float speed = 7.5f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;

    public int damageToGive = 50;


    void Update()
    {
        theRB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
         * Default function build into Unity, happens when a trigger colli-
         * der enters another collider, when two colliders overlap the fun-
         * ction is called
         */

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
        else if (other.tag == "Boss")
        {
            other.GetComponent<BossController>().DamageBoss(damageToGive);
            Instantiate(impactEffect, transform.position, transform.rotation);
            Instantiate(other.GetComponent<BossController>().bossHitEffect, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
