using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    public string levelToLoad;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
            StartCoroutine(LevelManager.instance.LevelEnd());
    }
}
