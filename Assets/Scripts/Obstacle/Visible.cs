using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

// 대상 스프라이트를 visible하게 바꿔주거나, newSprite로 바꿔주는 스크립트
public class Visible : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _targetSprite;
    [SerializeField] private Sprite _newSprite = null;
    [SerializeField] private bool isCol;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!isCol) return;
        
        if (_newSprite.IsUnityNull())
        {
            _targetSprite.color = new Color(1, 1, 1, 1);
        }
        else
        {
            _targetSprite.sprite = _newSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isCol) return;
        
        if (_newSprite.IsUnityNull())
        {
            _targetSprite.color = new Color(1, 1, 1, 1);
        }
        else
        {
            _targetSprite.sprite = _newSprite;
        }
    }
}
