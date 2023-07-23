using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    private bool moveRight = false;
    private float moveDirection; //일단 왼쪽으로 출발한다는 전제.
    private Rigidbody2D rigid;
    private Animator anim;
   
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveSide();
    }
 /// <summary>
    /// 좌우로 왔다갔다 움직임
    /// </summary>
    private void MoveSide()
    {
        // 이동 방향 설정
        anim.SetBool("isWalk", true);
        float moveDirection = moveRight ? 1f : -1f;
        rigid.velocity = new Vector2(moveDirection*speed*0.5f, rigid.velocity.y);


    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            moveRight = !moveRight;
            this.GetComponent<SpriteRenderer>().flipX = (moveRight);
        }
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