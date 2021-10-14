using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera;
    public Camera bigMapCamera;
    private bool bigMapActive;

    public bool isBossRoom;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        if (isBossRoom)
            target = PlayerController.instance.transform;
    }


    private void Update()
    {
        if (target != null)
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.M) && !isBossRoom)
            if (!bigMapActive)
                SetBigMapActive(true);
            else
                SetBigMapActive(false);
    }


    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }


    public void SetBigMapActive(bool isActive)
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = isActive;
            UIController.instance.bigMapInstructions.SetActive(isActive);

            mainCamera.enabled = !isActive;
            UIController.instance.miniMap.SetActive(!isActive);
            bigMapCamera.enabled = isActive;

            PlayerController.instance.canMove = !isActive;
            Time.timeScale = isActive ? 0f : 1f;
        }
    }
}
