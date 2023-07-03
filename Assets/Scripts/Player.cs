using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] 
    private Sprite itemPlayer; //아이템 얻은 후
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
    void Update()//?떒諛쒖쟻 ?엯?젰: ?뾽?뜲?씠?듃?븿?닔
    {
        //?젏?봽
        if ((Input.GetButtonDown("Jump")&&!anim.GetBool("isJump")))
        {
            rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }
        //釉뚮젅?씠?겕
        if (Input.GetButtonUp("Horizontal"))
        {
            //normalized: 踰≫꽣?겕湲곕?? 1濡? 留뚮뱺 ?긽?깭. 諛⑺뼢援ы븷 ?븣 ???
            //諛⑺뼢?뿉 ?냽?젰?쓣 0?쑝濡? 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0000001f, rigid.velocity.y);
        }

        //諛⑺뼢?쟾?솚
        if (Input.GetButton("Horizontal"))
            sprite_renderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //?븷?땲硫붿씠?뀡
        if (rigid.velocity.normalized.x == 0)
        {
            anim.SetBool("isWalk", false);
        }
        else
        {
            anim.SetBool("isWalk", true);
        }
    }
    private void FixedUpdate()//臾쇰━ update
    {
        //?궎 而⑦듃濡ㅻ줈 ???吏곸씠湲?
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
      
        if (rigid.velocity.x> max_speed)//?삤瑜몄そ
        {
            rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < max_speed*(-1))//?쇊履?
        {
            rigid.velocity = new Vector2(max_speed*(-1), rigid.velocity.y);
        }

        //踰꾪듉 ?씠?룞
        if (isButtonPressed)
        {
            // 踰꾪듉?쓣 怨꾩냽 ?늻瑜닿퀬 ?엳?쓣 ?븣 ?샇異쒗븷 硫붿냼?뱶瑜? ?뿬湲곗뿉 ?옉?꽦.
            if (isJumpButton)
            {
                //?젏?봽
                if (!anim.GetBool("isJump"))
                {
                    rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
                    anim.SetBool("isJump", true);
                }
            }
            if (isLeftButton)
            {
                rigid.AddForce(Vector2.right * -1, ForceMode2D.Impulse);

                if (rigid.velocity.x < max_speed * (-1))//?쇊履?
                {
                    rigid.velocity = new Vector2(max_speed * (-1), rigid.velocity.y);
                }
            }
            if (isRightButton)
            {
                rigid.AddForce(Vector2.right * 1, ForceMode2D.Impulse);

                if (rigid.velocity.x > max_speed)//?삤瑜몄そ
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
            //아이템 얻은 후 애니메이션이 따로 있어야할듯
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
        //?젅?씠?뼱 諛붽씀湲?
        gameObject.layer = 7;

        //?닾紐낇븯寃? 諛붽씀湲?
        sprite_renderer.color = new Color(1, 1, 1, 0.4f);

        //由ъ븸?뀡
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

    }

    //?솕硫대컰?쑝濡? ?굹媛?: 二쎌쓬
    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }


    //踰꾪듉?쓣 ?닃????뒗吏? ?뿉?뒗吏?
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
    
    //踰꾪듉 踰붿쐞?뿉?꽌 ?굹媛붿쑝硫? false
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
    //踰꾪듉 踰붿쐞 ?뱾?뼱?삤硫? true
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
    //?븘?옒 3媛? 硫붿냼?뱶 : 踰꾪듉?쓣 袁? ?늻瑜닿퀬 ?엳?뒗吏? 泥댄겕
    //踰꾪듉?쓣 ?늻瑜닿퀬 ?엳?뒗 ?룞?븞 泥섎━?븯?뒗 ?룞?옉.
    public void OnPointerDown()
    {
        isButtonPressed = true;
    }

    //踰꾪듉 ?뼹硫? false ?쟾?솚
    public void OnPointerUp()
    {
        isButtonPressed = false;
    }
    //踰꾪듉 踰붿쐞 ?굹媛? ?븣 
    public void OnPointerExit()
    {
        isButtonPressed = false;        
    }
    public void OnPointerEnter()
    {
        isButtonPressed = true;
    }
}

