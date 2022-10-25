using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelScreen : MonoBehaviour
{
    [SerializeField] private Text levelNameDisplay;
    [SerializeField] private Text timeDisplay;

    public void Open(int stars, float time, string levelName)
    {
        levelNameDisplay.text = levelName;
        timeDisplay.text = "" + (Math.Truncate(time * 10) / 10);
        gameObject.SetActive(true);

    }
}
