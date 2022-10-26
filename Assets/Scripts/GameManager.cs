using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Camera Camera;
    [SerializeField] LevelPair[] Levels;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject LevelSelect;
    [SerializeField] GameObject TitleScreen;
    [SerializeField] GameObject PauseScreen;
    [SerializeField] GameObject PauseButton;
    [SerializeField] EndLevelScreen EndLevelScreen;
    [SerializeField] Text TimeDisplay;

    private float levelTime;

    private string currentLevelName;

    private bool playing;
    public bool Playing { get { return playing; } }

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
        TitleScreen.gameObject.SetActive(true);
        EndLevelScreen.gameObject.SetActive(false);
        TimeDisplay.gameObject.SetActive(false);
        LevelSelect.gameObject.SetActive(false);
        PauseButton.SetActive(false);
        PauseScreen.SetActive(false);
    }

    public void InitLevel(string id)
    {
        GameObject currentLevelGameObject;
        GameObject playerGameObject;
        foreach (LevelPair level in Levels)
        {
            if (level.id == id)
            {
                currentLevelGameObject = Instantiate(level.LevelPrefab);
                currentLevel = currentLevelGameObject.GetComponent<Level>();              
                currentLevelGameObject.transform.position = Vector3.zero;

                Camera.orthographicSize = currentLevel.InitialCameraScale;
                Camera.transform.position = currentLevel.InitialCameraLocation;

                playerGameObject = Instantiate(playerPrefab);
                playerGameObject.transform.position = currentLevel.PlayerStartingPosition;
                player = playerGameObject.GetComponent<Player>();

                currentLevelName = id;
                LevelSelect.SetActive(false);
                TimeDisplay.gameObject.SetActive(true);
                PauseButton.SetActive(true);
                playing = true;
                levelTime = 0;
                Pause();
                return;
            }
        }
        Debug.LogError("Level not found for id: " + id);
    }

    public void CompleteLevel()
    {
        playing = false;
        int stars = 0 ;
        if (levelTime <= currentLevel.threeStarTime)
        {
            stars = 3;
        }
        else if (levelTime <= currentLevel.twoStarTime)
        {
            stars = 2;    
        }
        else
        {
            stars = 1;
        }
        EndLevelScreen.Open(stars, levelTime,currentLevelName);
    }
 

    public void DismissTitleScreen()
    {
        TitleScreen.SetActive(false);
        LevelSelect.SetActive(true);
    }

    public void OpenLevelSelect()
    {
        LevelSelect.SetActive(true);
        EndLevelScreen.gameObject.SetActive(false);
        currentLevel.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        PauseButton.SetActive(false);
        TimeDisplay.gameObject.SetActive(false);
        PauseScreen.gameObject.SetActive(false);
        playing = false;
    }

    public void Pause()
    {
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        TimeDisplay.text = "Time: "+(Math.Truncate(levelTime*10)/10);

        levelTime += Time.deltaTime;

    }
}

[Serializable]
public struct LevelPair
{
    public string id;
    public GameObject LevelPrefab;
}
