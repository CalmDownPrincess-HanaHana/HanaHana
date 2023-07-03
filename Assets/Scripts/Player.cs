using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] 
    private Sprite itemPlayer; //������ ���� ��
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
        max_speed = 3;
        jump_power = 8;
    }

    // Update is called once per frame
    void Update()//?��발적 ?��?��: ?��?��?��?��?��?��
    {
        //?��?��
        if ((Input.GetButtonDown("Jump")&&!anim.GetBool("isJump")))
        {
            rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }
        //브레?��?��
        if (Input.GetButtonUp("Horizontal"))
        {
            //normalized: 벡터?��기�?? 1�? 만든 ?��?��. 방향구할 ?�� ???
            //방향?�� ?��?��?�� 0?���? 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0000001f, rigid.velocity.y);
        }

        //방향?��?��
        if (Input.GetButton("Horizontal"))
            sprite_renderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //?��?��메이?��
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
        //?�� 컨트롤로 ???직이�?
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
      
        if (rigid.velocity.x> max_speed)//?��른쪽
        {
            rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < max_speed*(-1))//?���?
        {
            rigid.velocity = new Vector2(max_speed*(-1), rigid.velocity.y);
        }

        //버튼 ?��?��
        if (isButtonPressed)
        {
            // 버튼?�� 계속 ?��르고 ?��?�� ?�� ?��출할 메소?���? ?��기에 ?��?��.
            if (isJumpButton)
            {
                //?��?��
                if (!anim.GetBool("isJump"))
                {
                    rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
                    anim.SetBool("isJump", true);
                }
            }
            if (isLeftButton)
            {
                rigid.AddForce(Vector2.right * -1, ForceMode2D.Impulse);

                if (rigid.velocity.x < max_speed * (-1))//?���?
                {
                    rigid.velocity = new Vector2(max_speed * (-1), rigid.velocity.y);
                }
            }
            if (isRightButton)
            {
                rigid.AddForce(Vector2.right * 1, ForceMode2D.Impulse);

                if (rigid.velocity.x > max_speed)//?��른쪽
                {
                    rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
                }
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJump", false);
        }

        if(collision.gameObject.tag == "Enemy")
        { 
            onDamaged(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Item")
        {
            //������ ���� �� �ִϸ��̼��� ���� �־���ҵ�
            anim.SetBool("isItemGet", true);
            //changeSprite(itemPlayer);
            collision.gameObject.SetActive(false);
        }
    }
    
    void changeSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            sprite_renderer.sprite = sprite;
        }
    }

    public void onDamaged(Vector2 targetPos)
    {
        //?��?��?�� 바꾸�?
        gameObject.layer = 7;

        //?��명하�? 바꾸�?
        sprite_renderer.color = new Color(1, 1, 1, 0.4f);

        //리액?��
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

    }

    //?��면밖?���? ?���?: 죽음
    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }


    //버튼?�� ?��????���? ?��?���?
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
    
    //버튼 범위?��?�� ?��갔으�? false
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
    //버튼 범위 ?��?��?���? true
    public void jumpButtonEnter()
    {
        if(isButtonPressed)
            isJumpButton = true;
    }
    public void leftButtonEnter()
    {
        if (isButtonPressed)
            isLeftButton = true;
    }
    public void rightButtonEnter()
    {
        if (isButtonPressed)
            isRightButton = true;
    }
    //?��?�� 3�? 메소?�� : 버튼?�� �? ?��르고 ?��?���? 체크
    //버튼?�� ?��르고 ?��?�� ?��?�� 처리?��?�� ?��?��.
    public void OnPointerDown()
    {
        isButtonPressed = true;
    }

    //버튼 ?���? false ?��?��
    public void OnPointerUp()
    {
        isButtonPressed = false;
    }
    //버튼 범위 ?���? ?�� 
    public void OnPointerExit()
    {
        isButtonPressed = false;        
    }
    public void OnPointerEnter()
    {
        isButtonPressed = true;
    }
}

