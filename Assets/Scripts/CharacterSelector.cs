using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{

    public bool shouldUnlock;
    private bool canSelect;

    public SpriteRenderer charSprite;
    public Text charName;
    public GameObject message;

    public int idxToSpawn;

    public int swapCharactersSFX;


    private void Start()
    {
        if (shouldUnlock)
        {
            if (PlayerPrefs.HasKey(PlayerSpawnManager.instance.playersToSpawn[idxToSpawn].charName))
            {
                if (PlayerPrefs.GetInt(PlayerSpawnManager.instance.playersToSpawn[idxToSpawn].charName) == 1)
                    gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }


    private void Update()
    {
        if (canSelect && Input.GetKeyDown(KeyCode.E))
            SwapCharacters();
    }


    private void SwapCharacters()
    {
        AudioManager.instance.PlaySFX(swapCharactersSFX);

        Vector3 currPos = PlayerController.instance.transform.position;
        int currIdx = PlayerController.instance.playerToSpawnIdx;

        Destroy(PlayerController.instance.gameObject);

        PlayerController newPlayer = Instantiate(
            PlayerSpawnManager.instance.playersToSpawn[idxToSpawn], currPos,
            PlayerSpawnManager.instance.playersToSpawn[idxToSpawn].transform.rotation);
        PlayerController.instance = newPlayer;

        CameraController.instance.target = newPlayer.transform;

        charSprite.sprite = PlayerSpawnManager.instance.playersToSpawn[currIdx].bodySR.sprite;
        charName.text = PlayerSpawnManager.instance.playersToSpawn[currIdx].charName;
        idxToSpawn = currIdx;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            canSelect = true;
            message.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            canSelect = false;
            message.SetActive(false);
        }
    }
}
