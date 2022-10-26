using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Camera Camera;
    //Level Prefabs
    [SerializeField] LevelPair[] Levels;
    //prefab of the player gameobject
    [SerializeField] GameObject playerPrefab;
    //level select ui
    [SerializeField] GameObject LevelSelect;
    //title screen ui
    [SerializeField] GameObject TitleScreen;
    //pause screen pop up
    [SerializeField] GameObject PauseScreen;
    //in game pause button 
    [SerializeField] GameObject PauseButton;
    //level complete pop up
    [SerializeField] EndLevelScreen EndLevelScreen;
    //in game time display
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

    //initialize game objects for level
    //id must match id in "Levels"
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

    //On level completed, player reached goal
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
        Destroy(currentLevel.gameObject);
        Destroy(player.gameObject);
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
        //display level time rounded to tenths of a second
        TimeDisplay.text = "Time: "+(Math.Truncate(levelTime*10)/10);

        levelTime += Time.deltaTime;

    }
}

//struct to hold level prefabs and level id for instantiation
[Serializable]
public struct LevelPair
{
    public string id;
    public GameObject LevelPrefab;
}
