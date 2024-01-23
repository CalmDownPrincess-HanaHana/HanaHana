using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    bool isWalk = true;

    [SerializeField]
    bool isJump = false;

    [SerializeField]
    float walkingSpeed = 5f;

    [SerializeField]
    float jumpingSpeed = 5f;


    private bool moveForward = true;
    private float moveDirection; //일단 오른쪽으로 출발한다는 전제.
    private Rigidbody2D rigid;
    private Animator anim;
    private Transform enemyTransform;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
        }
        moveDirection = moveForward ? 1f : -1f;
        enemyTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isWalk)
        {
            MoveSide();
        }
    }
    /// <summary>
    /// 좌우로 왔다갔다 움직임
    /// </summary>
    private void MoveSide()
    {
        // 이동 방향 설정
        moveDirection = moveForward ? 1f : -1f;
        if (anim != null)
        {
            anim.SetBool("isWalk", true);
        }
        rigid.velocity = new Vector2(moveDirection * walkingSpeed * 0.5f, rigid.velocity.y);


    }
    //원래는 raycast 사용하려고 했는데 힘들었다. 그냥 빈 object 넣어서 platform 처리시킴. 이렇게 해도 되는지 의문이지만... 뭐 아무튼 trigger처리해놔서 큰 문제는 없었다...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            moveForward = !moveForward;

            Vector3 enemyScale = enemyTransform.localScale;
            enemyScale.x = (enemyScale.x == -1) ? 1 : -1;
            enemyTransform.localScale = enemyScale;
        }
        if (collision.gameObject.CompareTag("Player") && isJump)
        {
            Jump();
        }
    }
    //이거는 사과맵 중간에 점프하는 난쟁이. 스크립트 분리를 하는게 맞는 것 같다. 그리고 보니까 뛰는 속도가 더 빨라야 하나? 싶기도. 
    //아무튼 플레이하는 거 보니까 원하는대로 작동 안하는 느낌이 있엇음.
    private void Jump()
    {
        Vector3 force = Vector3.up * jumpingSpeed;
        rigid.AddForce(force, ForceMode2D.Impulse);
    }
}

// 지형체크
/*
Vector2 frontVec = new Vector2(rigid.position.x + moveDirection*0.3f, rigid.position.y + 0.5f);


Debug.DrawRay(frontVec, Vector2.down, Color.green);

RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 0.5f);
Debug.Log(rayHit.collider);

if (rayHit.collider.CompareTag("Platform"))
{
    moveRight = !moveRight;
}
*/