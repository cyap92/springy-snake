using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rigidBody;

    public bool InMotion = false;

    private bool dragging;
    private Vector2 startPos;
    private Vector2 endPos;

    [SerializeField] private float ForceMultiplyer = 2;

    private Vector2 launchPowerVector;

    private void Start()
    {
        cam = Camera.main;
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody component missing on gameobject " + gameObject.name+"!");
        }
    }

    //click on object to start jump calculations
    void OnMouseDown()
    {
        if (!InMotion)
        {
            startPos = cam.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
        }

    }

    //release and jump
    void OnMouseUp()
    {
        if (!InMotion)
        {
            endPos = cam.ScreenToWorldPoint(Input.mousePosition);
            dragging = false;
            rigidBody.AddForce(launchPowerVector * ForceMultiplyer, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if (dragging)
        {
            endPos = cam.ScreenToWorldPoint(Input.mousePosition);
            launchPowerVector = startPos - endPos;
            //Debug.Log("Power Vector: "+(startPos- endPos));
        }

        if (InMotion && rigidBody.velocity.magnitude <=0)
        {
            InMotion = false;
        }
        else if (!InMotion && rigidBody.velocity.magnitude > 0)
        {
            InMotion = true;
        }
    }
    private void Reset()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0f;
        transform.position = GameManager.Instance.CurrentLevel.PlayerStartingPosition;
    }
 

    //debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, (endPos - startPos));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, (startPos - endPos ));
    }
}

