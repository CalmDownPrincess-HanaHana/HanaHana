using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : MonoBehaviour
{
    private float max_speed;
    private float jump_power;
    Rigidbody2D rigid;
    SpriteRenderer sprite_renderer;
    Animator anim;
    public Define.PlayerState player_state;
    private bool isJumpButton=false;
    private bool isLeftButton = false;
    private bool isRightButton = false;
    private bool isButtonPressed = false;
    private bool isBorder = false;
    public GameObject SaveLoad;
    public bool ignore_jump = false;
    private bool isClimbing=false;
    private float inputVertical;
    public float ladder_speed;
    public LayerMask whatIsLadder;
    public float distance;
    private bool jump=true;
    public bool movable = true;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        max_speed = 5;
        jump_power = 15;
        player_state=new PlayerState();
        movable = true;
    }

    // Update is called once per frame
    void Update()//단발적 입력: 업데이트함수
    {
        if(!movable) return;
        
        //낙하속도 빠르게
        if (rigid.velocity.y < -0.2f)
        {
            rigid.gravityScale = 3;

        }
        //Idle이면 중력스케일 복구
        if (player_state == PlayerState.Idle)
        {
            rigid.gravityScale = 2;
        }

        //점프
        if ((Input.GetButtonDown("Jump") && !anim.GetBool("isJump") && !ignore_jump)&&jump)
        {
            //더플점프 막기: -1.5f이하이면 못 점프하게.
            if (!(rigid.velocity.y <= -1.5f) && player_state != PlayerState.Damaged)
            {
                player_state = PlayerState.Jump;
                rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
                anim.SetBool("isJump", true);
            }
        }
        //점프 상태 설정
        if((rigid.velocity.y <= -1.5f))
        {
            player_state = PlayerState.Jump;
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
        {
            sprite_renderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        //애니메이션
        if (rigid.velocity.normalized.x == 0)
        {
            if (player_state != PlayerState.Damaged&&rigid.velocity.y==0)
            {
                 player_state = PlayerState.Idle;
            }
            
            anim.SetBool("isWalk", false);
        }
        else
        {
            if (player_state != PlayerState.Damaged&&!anim.GetBool("isJump"))
            {
                player_state = PlayerState.Walk;
            }
            anim.SetBool("isWalk", true);
        }

        // 화면 위에 손가락이 없는지 확인
        if (Input.touchCount == 0)
        {
            isButtonPressed = false;
            isJumpButton=false;
            isLeftButton=false;
            isRightButton=false;
   
        }
        // 화면 위에 손가락이 있는지 확인
        if (Input.touchCount > 0)
        {
            isButtonPressed = true;
        }
    }
    private void FixedUpdate()//물리 update
    {
        if(!movable) return;

        //키 컨트롤로 움직이기
        float h = Input.GetAxisRaw("Horizontal");
       
        rigid.velocity = new Vector2(max_speed * h, rigid.velocity.y);
        RaycastHit2D hitInfo=Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        //버튼 이동
        if (isButtonPressed)
        {
            // 버튼을 계속 누르고 있을 때 호출할 메소드를 여기에 작성.
            if (isJumpButton&&jump)
            {
                //점프 
                if (!anim.GetBool("isJump") && !ignore_jump)
                {
                    //더플점프 막기: -1.5f이하이면 못 점프하게.
                    if (!(rigid.velocity.y <= -1.5f) && player_state != PlayerState.Damaged)
                    {
                        
                        player_state = PlayerState.Jump;
                        rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
                        anim.SetBool("isJump", true);
                    }
                }
            }
            if (isLeftButton)
            {
                rigid.velocity = new Vector2(max_speed * -1, rigid.velocity.y);
            }
            if (isRightButton)
            {
                rigid.velocity = new Vector2(max_speed * 1, rigid.velocity.y);
            }
        }
        if(hitInfo.collider!=null){
            jump=false;
            if((Input.GetButtonDown("Jump")||Input.GetKeyDown(KeyCode.Space)))
            {
                isClimbing=true;
                rigid.velocity = new Vector2(rigid.velocity.x, ladder_speed);
            }else{
                if(isLeftButton||isRightButton){
                    isClimbing=false;
                    jump=true;
                }
            }
        }else{jump=true;}
        if(isClimbing==true && hitInfo.collider!=null){
            //inputVertical=(Input.GetButtonDown("Jump")||Input.GetKeyDown(KeyCode.Space))?1.0f:0.01f;
            inputVertical=Convert.ToSingle(Input.GetButtonDown("Jump")||Input.GetKeyDown(KeyCode.Space));
            rigid.velocity=new Vector2(rigid.velocity.x, inputVertical*ladder_speed);
            rigid.gravityScale=0; 
        }else{
            rigid.gravityScale=2;
        }

    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            rigid.gravityScale = 2;//땅에 착지하면 중력스케일 원상복구
            anim.SetBool("isJump", false);
        }
        if(collision.gameObject.CompareTag("Enemy"))
        { 
            onDamaged(collision.transform.position);           
            //게임 매니저의 게임오버 처리 실행
            GameManager.instance.OnPlayerDead();
        }
        /*
        else if(collision.gameObject.CompareTag("Jumping"))
        {
            onJumped(collision.transform.position);
            GameManager.instance.OnPlayerDead();
        }*/
        
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Enemy"))
        { 
            onDamaged(collision.transform.position);           
            //게임 매니저의 게임오버 처리 실행
            GameManager.instance.OnPlayerDead();
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Flag"))
        {
            //리스폰 위치를 해당 Flag 위치로 재설정
            Vector3 flagPosition = collision.gameObject.transform.position;
            SaveLoad.GetComponent<SaveLoad>().SaveRespawn("respawn",flagPosition);
            Debug.Log("flagPosition"+flagPosition);
            Debug.Log("SaveRespawn"+SaveLoad.GetComponent<SaveLoad>().LoadRespawn("respawn"));
        }
        else if (collision.gameObject.CompareTag("Item"))
        {
            //진짜 아이템 먹었을 때 animation 바꿈
            anim.SetBool("isItemGet", true);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Border"))
        {
            player_state = PlayerState.Damaged;
            this.gameObject.layer = 7;
            isBorder = true;
            GameManager.instance.OnPlayerDead();
        }
    }

    public void onDamaged(Vector2 targetPos)
    {  
        //맞은 상태
        player_state = PlayerState.Damaged;
        //레이어변경
        this.gameObject.layer = 7;  
        //투명하게 바꾸기
        sprite_renderer.color = new Color(1, 1, 1, 0.4f);
        //리액션
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        if (!isBorder)
        {
            rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
        }
    }

    /*
    public void onJumped(Vector2 targetPos)
    {
        //맞은 상태
        player_state = PlayerState.Damaged;
        //레이어변경
        this.gameObject.layer = 7;       
        //투명하게 바꾸기
        sprite_renderer.color = new Color(1, 1, 1, 0.4f);
        //리액션
        /*int dirc = targetPos.x-transform.position.x  > 0 ? 1 : -1;
        if (!isBorder)
        {
            rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
        }
        //StartCoroutine(StartRotation());

        rigid.velocity=new Vector2(rigid.velocity.x, 50f);
        rigid.gravityScale=0; 
        Destroy(this.gameObject,0.5f);
    }*/
    

    /*private IEnumerator StartRotation()
    {
        for(int i=0;i<10;i++)
        {
            transform.Rotate(Vector3.up, 30f * Time.deltaTime);
            //transform.Rotate(0, 0, -Time.deltaTime * 100f, Space.Self);
            yield return null;
        }
    }*/

    //화면밖으로 나감: 죽음
    private void OnBecameInvisible()
    {   
        player_state = PlayerState.Damaged;
        if(!gameObject){
        this.gameObject.layer= 7;
        GameManager.instance.OnPlayerDead();
        this.gameObject.SetActive(false);
        }
        
    }



    //버튼을 눌렀는지 뗐는지
    public void jumpButtonTrue()
    {
        isJumpButton = true;
    }
    public void jumpButtonFalse()
    {
        isJumpButton = false;
    }
    public void leftButtonTrue()
    {
        
        isLeftButton = true;
        sprite_renderer.flipX = true;
        
    }
    public void leftButtonFalse()
    {
        isLeftButton = false;       
    }
    public void rightButtonTrue()
    {
        sprite_renderer.flipX = false;
        isRightButton = true;
    }
    public void rightButtonFalse()
    {
        isRightButton = false;
    }

}

