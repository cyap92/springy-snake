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
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float ForceMultiplyer = 2;
    [SerializeField] private float VelocityMin = .001f;
    [SerializeField] private float maxPower = 1000;
    [SerializeField] private GameObject aimingArrow;

    [SerializeField] private Sprite snakeCoiledSprite;
    [SerializeField] private Sprite snakeFlyingSprite;

    [SerializeField] private Vector3 aimingArrowStartingScale;
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

        if (InMotion && rigidBody.velocity.magnitude <=VelocityMin)
        {
            rigidBody.velocity = Vector3.zero;
            InMotion = false;
            spriteRenderer.sprite = snakeCoiledSprite;
            transform.rotation = Quaternion.identity;
        }
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
    private void Reset()
    {
        transform.rotation = Quaternion.identity;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0f;
        transform.position = GameManager.Instance.CurrentLevel.PlayerStartingPosition;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == "Boundary")
        {
            Reset();
        }
    }

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

