using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

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
            //StartCoroutine(ChangeAlpha());
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
            //StartCoroutine(ChangeAlpha());
        }
        else
        {
            _targetSprite.sprite = _newSprite;
        }
    }

    IEnumerator ChangeAlpha()
    {
        if (_targetSprite.color.a == 1)
        {
            float alpha = 1;
            while (_targetSprite.color.a == 0)
            {
                yield return new WaitForSeconds(0.05f);
                alpha -= 0.1f;
                _targetSprite.color = new Color(1, 1, 1, alpha);
            }
        }
        else
        {
            float alpha = 0;
            while (_targetSprite.color.a == 1)
            {
                yield return new WaitForSeconds(0.05f);
                alpha += 0.1f;
                _targetSprite.color = new Color(1, 1, 1, alpha);
            }
        }
    }
}
