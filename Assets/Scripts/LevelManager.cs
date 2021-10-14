using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public int currentCoins;

    public Transform startPoint;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        AudioManager.instance.levelMusic[AudioManager.instance.currentLevelMusic].Play();

        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;
        PlayerController.instance.SwitchGun();

        GetCoins(CharacterTracker.instance.currentCoins);
        Time.timeScale = 1f;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseUnpause();
    }


    public IEnumerator LevelEnd()
    {
        //AudioManager.instance

        AudioManager.instance.PlayWin();
        UIController.instance.StartFadeToBlack();

        PlayerController.instance.canMove = false;
        yield return new WaitForSeconds(waitToLoad);

        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;
        CharacterTracker.instance.currentCoins = currentCoins;

        SceneManager.LoadScene(nextLevel);
    }


    public void PauseUnpause()
    {
        if (!isPaused)
        {
            isPaused = true;
            UIController.instance.pauseMenu.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            isPaused = false;
            UIController.instance.pauseMenu.SetActive(false);

            Time.timeScale = 1f;
        }
    }


    public void GetCoins(int amount)
    {
        currentCoins += amount;
        UIController.instance.goldText.text = LevelManager.instance.currentCoins.ToString();
    }


    public void SpendCoins(int amount)
    {
        currentCoins = currentCoins - amount < 0 ? 0 : currentCoins - amount;
        UIController.instance.goldText.text = LevelManager.instance.currentCoins.ToString();
    }
}
