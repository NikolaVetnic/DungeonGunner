using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed;
    private Vector3 direction;

    public GameObject impactEffect;

    public int damage;


    private void Start()
    {
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
    }


    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            PlayerHealthController.instance.DamagePlayer(damage);

        Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
