using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class RotateController : ParentObstacleController
{
    public enum ObType
    {
        Rotate,
        Rolling,
        Swing,
        RotateDeltaAxis,
        RotateLoop,
        RotateAround
    }
    public enum ObDirection
    {
        Left,
        Right,
        Diagonal_Right_Up,
        Diagonal_Left_Up
    }
    [SerializeField] private ObDirection obDirection;
    [SerializeField] private ObType obType;
    [SerializeField] private float rotateSpeed=10000f;
    [SerializeField] private float rollingSpeed=11f;
    [SerializeField] private float gravityScale=4f;
    /// <summary>
    /// Swing용 변수
    /// </summary>
    /// <returns></returns>

    [SerializeField] private float angle = 90f; // 최대 회전 각도 (양쪽으로 45도)
    private float lerpTime = 0;
    [SerializeField]
    private float speed = 45f; // 회전 속도 (도/초), 예: 초당 45도 회전
    /// <summary>
    /// RotateDeltaAxis
    /// </summary>
    /// <returns></returns>
    /// 
    [SerializeField] private float rotateDelta;
    [SerializeField] private Vector3 point;
    [SerializeField] private Collider2D collisionCollider;
    /// <summary>
    /// RotateLoop
    /// </summary>
    /// <returns></returns>
    [SerializeField] private float _rotateDuration = 10.0f;
    [SerializeField] private int _direction = 1;

    /// <summary>
    /// RotateAround
    /// </summary>
    /// <returns></returns>
    /// 
    [SerializeField] private Vector3 _targetVector3 = Vector3.zero;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private float _moveDirection = 1;
    public override IEnumerator Activate()
    {
        switch (obType)
        {
            case ObType.Swing:
                StartCoroutine(Swing());
                break;
            case ObType.Rotate:
                StartCoroutine(Rotate());
                break;
            case ObType.Rolling:
                StartCoroutine(Rolling(obDirection,rollingSpeed,gravityScale));
                break;
            case ObType.RotateDeltaAxis:
                StartCoroutine(RotateDeltaAxis());
                break;
            case ObType.RotateLoop:
                StartCoroutine(RotateLoop());
                break;
            case ObType.RotateAround:
                StartCoroutine(RotateAround());
                break;
        }
        yield return base.Activate(); // 부모 클래스의 Activate 메서드 실행 
                                      //사실 ismoving과 별개로 움직이기 때문에 이걸 굳이 부모 activate를 실행하지 않아도 되지만 후에 test할때를 위해 그냥 실행하겠음. 
    }

    private void Awake()
    {
        base.Start();
    }

    IEnumerator RotateLoop()
    {
        transform.DORotate(new Vector3(0, 0, _direction * 360), _rotateDuration, RotateMode.FastBeyond360)
                         .SetEase(Ease.Linear)
                         .SetLoops(-1);
        yield return null;
    }
    IEnumerator Rotate()
    {
        if (obDirection == ObDirection.Left)
        {
            while (true)
            {
                this.gameObject.transform.Rotate(0, 0, -Time.deltaTime * rotateSpeed, Space.Self);
                yield return null;
            }
        }
        if (obDirection == ObDirection.Right)
        {
            while (true)
            {
                this.gameObject.transform.Rotate(0, 0, +Time.deltaTime * rotateSpeed, Space.Self);
                yield return null;
            }
        }


    }
    IEnumerator Rolling(ObDirection obDirection, float rollingSpeed, float gravityScale)
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.gravityScale = gravityScale;
        }

        switch (obDirection)
        {
            case ObDirection.Left:
                rigid.velocity = new Vector3(-rollingSpeed, 0, 0);
                break;
            case ObDirection.Right:
                rigid.velocity = new Vector3(rollingSpeed, 0, 0);
                break;
            case ObDirection.Diagonal_Left_Up:
                rigid.AddForce(new Vector2(-1, 1) * rollingSpeed, ForceMode2D.Impulse);
                break;
            case ObDirection.Diagonal_Right_Up:
                rigid.AddForce(new Vector2(1, 1) * rollingSpeed, ForceMode2D.Impulse);
                break;
        }
        yield return null;
    }
    IEnumerator Swing()
    {
        while (true)
        {
            lerpTime += Time.deltaTime * speed;
            float currentAngle = Mathf.Lerp(-angle, angle, GetLerpTParam());

            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            yield return null;
        }
        
    }
    /*Swing메소드*/
    float GetLerpTParam()
    {
        return (Mathf.Sin(lerpTime * Mathf.Deg2Rad) + 1) * 0.5f;
    }

    /// <summary>
    /// ex) a_Tree2
    /// 트리거가 감지되면 현재위치에서 point를 더한 위치를 기준으로 rotateDelta만큼 회전
    /// </summary>
    /// <returns></returns>
    IEnumerator RotateDeltaAxis()
    {
        collisionCollider.enabled = true;
        transform.RotateAround(transform.position + point, Vector3.forward, rotateDelta);
        yield return null;
    }
    
    IEnumerator RotateAround()
    {
        while (true)
        {
            transform.RotateAround(_targetVector3, Vector3.back, _moveSpeed * Time.fixedDeltaTime * _moveDirection);
            yield return null;
        }
    }
}
