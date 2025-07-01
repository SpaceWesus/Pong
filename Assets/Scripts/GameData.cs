using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance = null;
    private static bool instanceCreated = false;

    public bool[] unlockedLevels = new bool[10];

    private void Awake()
    {
        if (!GameData.instanceCreated)
        {
            instance = this;
            GameData.instanceCreated = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}