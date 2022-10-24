using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] LevelPair[] Levels;
    [SerializeField] GameObject playerPrefab;
    
    private Level currentLevel;
    public Level CurrentLevel { get { return currentLevel; } }

    private Player player;
    public Player Player { get { return player; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        string currrentLevelId = "0";
        InitLevel(currrentLevelId);
    }

    private void InitLevel(string id)
    {
        GameObject currentLevelGameObject;
        GameObject playerGameObject;
        foreach (LevelPair level in Levels)
        {
            if (level.id == id)
            {
                currentLevelGameObject = Instantiate(level.LevelPrefab);
                currentLevelGameObject.transform.position = Vector3.zero;
                currentLevel = currentLevelGameObject.GetComponent<Level>();

                playerGameObject = Instantiate(playerPrefab);
                playerGameObject.transform.position = currentLevel.PlayerStartingPosition;
                player = playerGameObject.GetComponent<Player>();
                break;
            }
        }

    }
}

[Serializable]
public struct LevelPair
{
    public string id;
    public GameObject LevelPrefab;
}
