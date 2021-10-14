using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    public float speed;
    private Vector3 direction;

    public GameObject impactEffect;

    public int damage;


    private void Start()
    {
        direction = transform.right;
    }


    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (!BossController.instance.gameObject.activeInHierarchy)
            Destroy(gameObject);
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
