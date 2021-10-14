using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{

    public float moveSpeed = 3f;
    private Vector3 moveDirection;

    public float deceleration = 5f;

    public float lifeTime = 3f;

    public SpriteRenderer theSR;
    public float fadeSpeed = 2.5f;


    private void Start()
    {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }


    private void Update()
    {
        transform.position += moveDirection * Time.deltaTime;
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b,
                Mathf.MoveTowards(theSR.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (theSR.color.a == 0f)
                Destroy(gameObject);
        }
    }
}
