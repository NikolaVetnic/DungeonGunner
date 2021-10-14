using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string levelToLoad;

    public GameObject deletePanel;

    public PlayerController[] charactersToDelete;

    public int buttonClickSFX;


    public void StartGame()
    {
        AudioManager.instance.PlaySFX(buttonClickSFX);
        Invoke("LoadCharacterSelect", AudioManager.instance.sfx[buttonClickSFX].clip.length);
    }

    private void LoadCharacterSelect()  { SceneManager.LoadScene(levelToLoad);  }


    public void ExitGame()
    {
        AudioManager.instance.PlaySFX(buttonClickSFX);
        Invoke("CloseGame", AudioManager.instance.sfx[buttonClickSFX].clip.length);
    }

    private void CloseGame()            { Application.Quit();                   }


    public void DeleteSave()
    {
        deletePanel.SetActive(true);
        AudioManager.instance.PlaySFX(buttonClickSFX);
    }


    public void ConfirmDelete()
    {
        deletePanel.SetActive(false);

        foreach (PlayerController playerController in charactersToDelete)
            PlayerPrefs.DeleteKey(playerController.charName);

        AudioManager.instance.PlaySFX(buttonClickSFX);
    }


    public void CancelDelete()
    {
        deletePanel.SetActive(false);
        AudioManager.instance.PlaySFX(buttonClickSFX);
    }
}
