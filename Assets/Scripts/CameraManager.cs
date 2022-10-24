using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Player player;

    void Start()
    {
        player = GameManager.Instance.Player;
        if (player == null)
        {
            Debug.LogError("missing Player reference for in camera manager");
        }
    }

    private void Update()
    {
        if (player.InMotion)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10) ;
        }
    }
}
