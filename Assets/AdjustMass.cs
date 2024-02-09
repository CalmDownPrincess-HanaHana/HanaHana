using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMass : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRigidbody = collision.gameObject.transform.GetComponent<Rigidbody2D>();
            playerRigidbody.gravityScale = 10000f;
            Debug.Log("닿았");
        }
    }
}
