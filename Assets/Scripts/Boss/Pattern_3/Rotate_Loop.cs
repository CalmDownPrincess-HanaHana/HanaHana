using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트가 회전하도록 하는 스크립트
/// 360도 회전하는데 rotateDuration만큼의 시간이 걸린다.
/// </summary>
public class Rotate_Loop : MonoBehaviour
{
    [SerializeField] float _rotateDuration = 10.0f;
    [SerializeField] int _direction = 1;
   
    void Start()
    {
        transform.DORotate(new Vector3(0, 0, _direction * 360), _rotateDuration, RotateMode.FastBeyond360)
                         .SetEase(Ease.Linear)
                         .SetLoops(-1);
    }
}
