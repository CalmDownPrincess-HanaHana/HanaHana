using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float max_speed;
    [SerializeField]
    private float jump_power;
    Rigidbody2D rigid;
    SpriteRenderer sprite_renderer;
    Animator anim;

    public bool isJumpButton=false;
    public bool isLeftButton = false;
    public bool isRightButton = false;
    public bool isButtonPressed = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        max_speed = 5;
        jump_power = 15;


    }

    // Update is called once per frame
    void Update()//단발적 입력: 업데이트함수
    {
        //점프
        if ((Input.GetButtonDown("Jump")&&!anim.GetBool("isJump")))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
            anim.SetBool("isJump", true);
        }
      

        //브레이크
        if (Input.GetButtonUp("Horizontal"))
        {
            //normalized: 벡터크기를 1로 만든 상태. 방향구할 때 씀
            //방향에 속력을 0으로 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0000001f, rigid.velocity.y);
        }

        //방향전환
        if (Input.GetButton("Horizontal"))
            sprite_renderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //애니메이션
        if (rigid.velocity.normalized.x == 0)
        {
            anim.SetBool("isWalk", false);
        }
        else
        {
            anim.SetBool("isWalk", true);
        }
    }
    private void FixedUpdate()//물리 update
    {
        //키 컨트롤로 움직이기
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        
        if (rigid.velocity.x> max_speed)//오른쪽
        {
            rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < max_speed*(-1))//왼쪽
        {
            rigid.velocity = new Vector2(max_speed*(-1), rigid.velocity.y);
        }

        //버튼 이동
        if (isButtonPressed)
        {
            // 버튼을 계속 누르고 있을 때 호출할 메소드를 여기에 작성.
            if (isJumpButton)
            {
                //점프
                if (!anim.GetBool("isJump"))
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
                    anim.SetBool("isJump", true);
                }
            }
            if (isLeftButton)
            {
                rigid.AddForce(Vector2.right * -1, ForceMode2D.Impulse);

                if (rigid.velocity.x < max_speed * (-1))
                {
                    rigid.velocity = new Vector2(max_speed * (-1), rigid.velocity.y);
                }
            }
            if (isRightButton)
            {
                rigid.AddForce(Vector2.right * 1, ForceMode2D.Impulse);

                if (rigid.velocity.x > max_speed)
                {
                    rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
                }
            }
        }

    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "Enemy")
        { 
            onDamaged(collision.transform.position);
            //寃뚯엫 留ㅻ땲???寃뚯엫?ㅻ쾭 泥섎━ ?ㅽ뻾
            GameManager.instance.OnPlayerDead();
        }

        if(collision.gameObject.tag=="Flag")
        {
            //由ъ뒪???꾩튂瑜??대떦 Flag ?꾩튂濡??ъ꽕??
            Vector3 flagPosition=collision.gameObject.transform.position;
            PlayerRespawn playerRespawn = GetComponent<PlayerRespawn>();
            playerRespawn.SetRespawnPoint(flagPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Item")
        {
            //진짜 아이템 먹었을 때 animation 바꿈
            anim.SetBool("isItemGet", true);
            collision.gameObject.SetActive(false);
        }
    }

    public void onDamaged(Vector2 targetPos)
    {
        //레이어 바꾸기
        gameObject.layer = 7;

        //투명하게 바꾸기
        sprite_renderer.color = new Color(1, 1, 1, 0.4f);

        //리액션
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

    }

    //화면밖으로 나감: 죽음
    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }


    //버튼을 눌렀는지 뗐는지
    public void jumpButtonDown()
    {
        isJumpButton = true;
    }
    public void jumpButtonUp()
    {
        isJumpButton = false;
    }
    public void leftButtonDown()
    {
        isLeftButton = true;
    }
    public void leftButtonUp()
    {
        isLeftButton = false;
    }
    public void rightButtonDown()
    {
        isRightButton = true;
    }
    public void rightButtonUp()
    {
        isRightButton = false;
    }
    
    //버튼 범위에서 나갔으면 false
    public void jumpButtonExit()
    {
        isJumpButton= false;
    }
    public void leftButtonExit()
    {
        isLeftButton = false;
    }
    public void rightButtonExit()
    {
        isRightButton = false;
    }
    //버튼 범위 들어오면 true
    public void jumpButtonEnter()
    {
            isJumpButton = true;
    }
    public void leftButtonEnter()
    {
            isLeftButton = true;
    }
    public void rightButtonEnter()
    {
            isRightButton = true;
    }
    //아래 3개 메소드 : 버튼을 꾹 누르고 있는지 체크
    //버튼을 누르고 있는 동안 처리하는 동작.
    public void OnPointerDown()
    {
        isButtonPressed = true;
    }

    //버튼 떼면 false 전환
    public void OnPointerUp()
    {
        isButtonPressed = false;
    }
    //버튼 범위 나갈 때 
    public void OnPointerExit()
    {
        isButtonPressed = false;        
    }
    public void OnPointerEnter()
    {
        isButtonPressed = true;
    }
}

