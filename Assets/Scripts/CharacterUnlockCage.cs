using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{

    private bool canUnlock;

    public GameObject message;

    public CharacterSelector[] characterSelectors;
    private CharacterSelector charToUnlock;

    public SpriteRenderer cagedSR;


    private void Start()
    {
        CharacterSelector[] lockedCharacters = characterSelectors
            .Where(cs => PlayerPrefs.GetInt(PlayerSpawnManager.instance.playersToSpawn[cs.idxToSpawn].charName) == 0)
            .Where(cs => cs.shouldUnlock)
            .ToArray();

        if (lockedCharacters.Count() == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        charToUnlock = lockedCharacters[Random.Range(0, lockedCharacters.Length)];
        cagedSR.sprite = PlayerSpawnManager.instance.playersToSpawn[charToUnlock.idxToSpawn].bodySR.sprite;
    }


    private void Update()
    {
        if (canUnlock && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt(PlayerSpawnManager.instance.playersToSpawn[charToUnlock.idxToSpawn].charName, 1);

            Instantiate(charToUnlock, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
