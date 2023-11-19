using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Rotate_DeltaAxis : MonoBehaviour
{
    [SerializeField] private Tags compareTag;
    [SerializeField] private float rotateDelta;
    [SerializeField] private Vector3 point;
    [SerializeField] private Collider2D collisionCollider;
    
    /// <summary>
    /// 트리거가 감지되면 현재위치에서 point를 더한 위치를 기준으로 rotateDelta만큼 회전
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(compareTag.ToString())) 
            return;

        collisionCollider.enabled = true;
        transform.RotateAround(transform.position + point, Vector3.forward, rotateDelta);
    }
}
