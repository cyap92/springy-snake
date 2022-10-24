using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public string levelId;
    public Vector3 InitialCameraLocation;
    public float InitialCameraScale;
    public Vector3 PlayerStartingPosition;
    public Collider2D GoalCollider;
}
