using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrowdControl : MonoBehaviour
{

    public GameObject impactEffect;

    public int damageToGive = 50;


    private void Awake()
    {
        Debug.Log("CROWD CONTROL ACTIVATED");
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("SOMETHING IN AREA");
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Enemy")
    //    {
    //        Debug.Log("ENEMY HIT");
    //        other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
    //        Instantiate(impactEffect, transform.position, transform.rotation);
    //    }
    //}
}
