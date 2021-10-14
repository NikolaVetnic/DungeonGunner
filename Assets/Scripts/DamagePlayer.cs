using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    public int damage = 5;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            PlayerHealthController.instance.DamagePlayer(damage);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
            PlayerHealthController.instance.DamagePlayer(damage);
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        OnCollisionEnter2D(other);
    }
}
