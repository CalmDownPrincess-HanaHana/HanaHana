using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerFreeze : MonoBehaviour
{
    [SerializeField] private bool _isCol = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCol || !other.CompareTag("Player")) return;

        other.GetComponent<Player>().movable = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!_isCol || !other.gameObject.CompareTag("Player")) return;
        
        other.gameObject.GetComponent<Player>().movable = false;
    }
}
