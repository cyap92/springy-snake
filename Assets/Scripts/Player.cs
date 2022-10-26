using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rigidBody;

    //is the player moving, don't allow player to interact if player is moving
    public bool InMotion = false;

    private bool dragging;
    //starting mouse position of a drag
    private Vector2 startPos;
    //ending mouse position of a drag
    private Vector2 endPos;
    private SpriteRenderer spriteRenderer;

    //multiplier for the force applied to the player on launch
    [SerializeField] private float ForceMultiplyer = 2;
    //minimum velocity before clamped to zero and force the player to stop
    [SerializeField] private float VelocityMin = .001f;
    //maximum magnitude of the launch vector
    [SerializeField] private float maxPower = 1000;

    [SerializeField] private GameObject aimingArrow;
    [SerializeField] private Sprite snakeCoiledSprite;
    [SerializeField] private Sprite snakeFlyingSprite;

    [SerializeField] private Vector3 aimingArrowStartingScale;
    //scale factor for the aiming arrow in relation to the magnitude of the launch vector
    [SerializeField] private float aimingArrowMagnitudeScaleFactor;

    private Vector2 launchPowerVector;

    private void Awake()
    {
        cam = Camera.main;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody component missing on gameobject " + gameObject.name + "!");
        }
        aimingArrow.SetActive(false);
    }

    //click on object to start jump calculations
    void OnMouseDown()
    {
        if (!InMotion && GameManager.Instance.Playing)
        {
            startPos = cam.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
            aimingArrow.gameObject.SetActive(true);
        }
    }

    //release and jump
    void OnMouseUp()
    {
        if (!InMotion && GameManager.Instance.Playing)
        {
            endPos = cam.ScreenToWorldPoint(Input.mousePosition);
            dragging = false;
            rigidBody.AddForce(launchPowerVector * ForceMultiplyer, ForceMode2D.Impulse);
            aimingArrow.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //calculate the launch vector and adjust the aiming arrow visual
        if (dragging)
        {
            endPos = cam.ScreenToWorldPoint(Input.mousePosition);
            launchPowerVector = Vector3.ClampMagnitude(startPos - endPos,maxPower);
            //Debug.Log("Magnitude: "+launchPowerVector.magnitude);

            var angle = Mathf.Atan2(launchPowerVector.y, launchPowerVector.x) * Mathf.Rad2Deg - 90;
            aimingArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            aimingArrow.transform.localScale = new Vector3(aimingArrowStartingScale.x + launchPowerVector.magnitude * aimingArrowMagnitudeScaleFactor, aimingArrowStartingScale.y + launchPowerVector.magnitude * aimingArrowMagnitudeScaleFactor, aimingArrowStartingScale.z + launchPowerVector.magnitude * aimingArrowMagnitudeScaleFactor);
            aimingArrow.GetComponent<SpriteRenderer>().color = new Color(1, 1-launchPowerVector.magnitude / maxPower, 1-launchPowerVector.magnitude / maxPower);
        }
        //player has come to rest and they can interact again
        if (InMotion && rigidBody.velocity.magnitude <=VelocityMin)
        {
            rigidBody.velocity = Vector3.zero;
            InMotion = false;
            spriteRenderer.sprite = snakeCoiledSprite;
            transform.rotation = Quaternion.identity;
        }
        //player is in motion
        else if (!InMotion && rigidBody.velocity.magnitude > VelocityMin)
        {
            spriteRenderer.sprite = snakeFlyingSprite;
            if (rigidBody.velocity.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            InMotion = true;
        }
    }

    //reset player to starting location
    private void Reset()
    {
        transform.rotation = Quaternion.identity;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0f;
        transform.position = GameManager.Instance.CurrentLevel.PlayerStartingPosition;
    }

    //detect if player leaves playable area and then reset
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        if (collision.tag == "Boundary")
        {
            Reset();
        }
    }

    //detect if player reached goal area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Goal")
        {
            GameManager.Instance.CompleteLevel();
        }
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

