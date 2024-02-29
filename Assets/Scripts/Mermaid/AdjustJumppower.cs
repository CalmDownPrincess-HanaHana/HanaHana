using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustJumppower : MonoBehaviour
{
    [SerializeField] private float jumpPower = 100f;
    private Rigidbody2D playerRigidbody;
    private Player playerScript;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.jump_power = jumpPower;
            }
        }
    }
}
