using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to hold data for each level
public class Level : MonoBehaviour
{
    //position where the main orthographic camera should be placed
    public Vector3 InitialCameraLocation;
    //size of the camera
    public float InitialCameraScale;
    //where the player starts and where it will reset the player to when they lose
    public Vector3 PlayerStartingPosition;
    //time the level must be completed in to achieve 3 stars (seconds)
    public float threeStarTime;
    //time the level must be completed in to achieve 2 stars (seconds)
    public float twoStarTime;
}
