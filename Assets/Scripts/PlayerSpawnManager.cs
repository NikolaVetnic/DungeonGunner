using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{

    [HideInInspector] public static PlayerSpawnManager instance;

    public PlayerController[] playersToSpawn;


    private void Awake()
    {
        instance = this;
    }
}
