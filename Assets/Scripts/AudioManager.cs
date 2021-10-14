using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [HideInInspector] public static AudioManager instance;

    public AudioSource[] levelMusic;
    public int currentLevelMusic;

    public AudioSource gameOverMusic;
    public AudioSource winMusic;

    /* 
     * Used : 0, 1, 2, 7, 8, 9, 11, 12, 17 ............................
     */

    public AudioSource[] sfx;


    private void Awake()
    {
        instance = this;
    }


    public void PlayGameOver()
    {
        foreach (AudioSource track in levelMusic)
            track.Stop();
        gameOverMusic.Play();
    }


    public void PlayWin()
    {
        foreach (AudioSource track in levelMusic)
            track.Stop();
        winMusic.Play();
    }


    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();                                                  // needed in order to play the sfx from the beginning each time
        sfx[sfxToPlay].Play();
    }
}
