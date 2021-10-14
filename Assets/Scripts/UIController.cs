using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    public Slider healthSlider;
    public Text healthText;

    public Text goldText;

    public Image gunImage;
    public Text gunName;

    public Text bossName;
    public Slider bossHealthBar;

    public GameObject deathScreen;

    public Image fadeScreen;
    public float fadeSpeed;
    private bool fadeToBlack;
    private bool fadeOutBlack;

    public string newGameScene;
    public string mainMenuScene;

    public GameObject pauseMenu;

    public GameObject miniMap;
    public GameObject bigMapInstructions;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;
    }


    private void Update()
    {
        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
                fadeOutBlack = false;
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 1, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
                fadeToBlack = false;
        }
    }


    public void MakeFadeScreenTransparent()
    {
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 0f);
    }


    public void MakeFadeScreenNonTransparent()
    {
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1f);
    }


    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }


    public void NewGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(newGameScene);
        Destroy(PlayerController.instance.gameObject);
    }


    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);
        Destroy(PlayerController.instance.gameObject);
    }


    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}
