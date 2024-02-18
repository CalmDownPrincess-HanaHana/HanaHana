using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;
/// <summary>
/// 플레이어
/// 수정: 유진이 총책임자
/// 수정하려면 유진에게 검토받기!!!
/// 수정이 예민한 스크립트입니다
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// 휴대폰 터치와 관련된 변수입니다. 
    /// 터치 범위 계산하는 변수
    /// </summary>
    float rightButtonEnd = Screen.width * 0.4167f;
    float leftButtonEnd = Screen.width * 0.2083f;
    float jumpButtonEnd = Screen.width;
    public float RightButtonEnd
    {
        get { return RightButtonEnd; }
    }
    public float LeftButtonEnd
    {
        get { return LeftButtonEnd; }
    }
    public float JumpButtonEnd
    {
        get { return JumpButtonEnd; }
    }
    //플레이어가 내는 소리에 대한 배열
    public AudioSource[] audioSources = null;
    //경사인지 감지
    private bool is_Slope = false;
    //무적모드 변수
    [SerializeField] private bool invincibility = false;
    //캡슐화
    public bool Invincibility
    {
        get { return invincibility; }
        set { invincibility = value; }
    }
    //점프중인지 체크하는 변수
    public bool is_jump = false;
    //점프 허용 범위
    private const float JUMP_CRITERIA = 0.4f;
    /// <summary>
    /// 점프력, 속력
    /// </summary>
    [SerializeField]
    private float jump_power;
    private float max_speed;
    //리지드바디
    Rigidbody2D rigid;
    //스프라이트렌더러
    SpriteRenderer sprite_renderer;
    //애니메이터
    Animator anim;
    //플레이어 스테이트 선언
    public Define.PlayerState player_state;
    /// <summary>
    /// 키보드입력: 좌우키 중 어떤 키 입력하는지 받는 변수
    /// </summary>
    private float horizontal = 0;
    //보더에 맞았으면 죽어야되니까 그거 감지하는 변수
    private bool isBorder = false;
    //점프무시
    private bool ignoreJump = false;
    //초기세팅 사운드 값
    float settingSoundValue;
    
    /// <summary>
    /// 유진 외 정의한 변수
    /// </summary>
    //아이스감지변수인듯 (현민)
    private bool isIce = false;
    //세이브 관련 변수인듯(소연)
    public GameObject SaveLoad;
    [HideInInspector]
    public int isMoss = 0;
    //서현 변수: 입력무시용 변수
    public bool movable = true;
    public bool isWater=false;

    //캡슐화
    public float Horizontal
    {
        get
        {
            return horizontal;
        }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //스피드 고정하려고 여기서 정의
        max_speed = 5;
        player_state = new PlayerState();
        movable = true;
        //초기세팅 효과음값 가져오기
        settingSoundValue = PlayerPrefs.GetFloat("effectValue", 1f);
        //초기세팅값적용
        foreach (AudioSource audio in audioSources)
        {
            audio.volume = settingSoundValue;
        }
    }

    // Update is called once per frame
    void Update()//단발적 입력: 업데이트함수
    {

        //플레이어가 사운드 값 바꿨을 시 반영되도록
        if (settingSoundValue != PlayerPrefs.GetFloat("effectValue", 1f))
        {
            settingSoundValue = PlayerPrefs.GetFloat("effectValue", 1f);
            foreach (AudioSource audio in audioSources)
            {
                audio.volume = settingSoundValue;
            }
        }

        if (!movable) return;
        /*
         보스맵 4
         */
        SnowBoss4();
        //MerMaid
        Swim();
    }

    private void FixedUpdate()//물리 update
    {
        if (!movable) return;

        /*
         레이캐스트
         */
        RayCastProcess();


        //터치로 움직이기
        TouchProcess();

        //키 컨트롤로 움직이기
        PcProcess();

        //점프상태 처리
        JumpStateProcess();

        //중력스케일 처리
        GravityProcess();

        
    }

    private void GravityProcess()
    {
        //떨어질 때 빨리 떨어지게
        if (rigid.velocity.normalized.y <= -JUMP_CRITERIA)//낙하하면 훅 떨어지게
        {
            rigid.gravityScale = 3;
        }

        //Idle이면 중력스케일 복구
        if (player_state == PlayerState.Idle)
        {
            rigid.gravityScale = 2;
        }
    }

    private void JumpStateProcess()
    {
        //점프"상태"설정 (player_state)
        //이 속력일 때만 점프 상태로 전환 가능함.(땅에는 붙어있다고 치는 속력)
        if (rigid.velocity.normalized.y <= -JUMP_CRITERIA || rigid.velocity.normalized.y >= JUMP_CRITERIA)
        {
            //보스맵4아닐때
            if (SceneManager.GetActiveScene().name != Define.Scene.SnowBoss4.ToString())
            {
                //애니메이션 켜주고
                anim.SetBool("isJump", true);
                anim.SetBool("isWalk", false);
                //플레이어상태 바꿔주기
                if (player_state != PlayerState.Damaged && !is_Slope)
                {
                    //점프로
                    player_state = PlayerState.Jump;
                }
            }
        }
    }

    private void RayCastProcess()
    {
        //점프중인지 감지하는 레이캐스트. 파란색
        Debug.DrawRay(rigid.position + new Vector2(-0.3f, 0), Vector3.down, new Color(0, 0, 1));
        RaycastHit2D jumpLeftRayHit = Physics2D.Raycast(rigid.position + new Vector2(-0.3f, 0), Vector2.down, 0.6f, LayerMask.GetMask("Platform"));
        Debug.DrawRay(rigid.position + new Vector2(0.3f, 0), Vector3.down, new Color(0, 0, 1));
        RaycastHit2D jumpRightRayHit = Physics2D.Raycast(rigid.position + new Vector2(0.3f, 0), Vector2.down, 0.6f, LayerMask.GetMask("Platform"));

        if (jumpLeftRayHit.collider != null || jumpRightRayHit.collider != null)
        {
            is_jump = false;
        }
        else
        {
            //점프면 is_jump=true
            if (player_state == PlayerState.Jump)
            {
                is_jump = true;
            }
        }


        //타고올라가기 방지 레이캐스트(낭떠러지에서 이동키 누르면 쫙 올라가던 문제해결)
        //빨간색
        Debug.DrawRay(rigid.position + new Vector2(0, -0.45f), Vector3.right, new Color(1, 0, 0));
        Debug.DrawRay(rigid.position + new Vector2(0, -0.45f), Vector3.left, new Color(1, 0, 0));
        RaycastHit2D platformRightRayHit = Physics2D.Raycast(rigid.position + new Vector2(0, -0.45f), Vector2.right, 0.5f, LayerMask.GetMask("Platform"));
        RaycastHit2D platformLeftRayHit = Physics2D.Raycast(rigid.position + new Vector2(0, -0.45f), Vector2.left, 0.5f, LayerMask.GetMask("Platform"));
        //맞았다는 뜻(근데 slope은 체크기준이 아님.)
        if (platformRightRayHit.collider != null || platformLeftRayHit.collider != null)
        {
            //평지가 아니면 && Slope이 아니면
            if (!is_Slope && !(rigid.velocity.normalized.y > -JUMP_CRITERIA && rigid.velocity.normalized.y < JUMP_CRITERIA))
            {
                //박스콜라이더로 켜주면 못 타고 올라감
                this.GetComponent<CapsuleCollider2D>().enabled = false;
                this.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else
        {
            this.GetComponent<CapsuleCollider2D>().enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void PcProcess()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        //현민코드
        if (isIce)
        {
            if (Mathf.Abs(rigid.velocity.x) <= max_speed)
            {
                rigid.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);
            }

            if ((player_state != PlayerState.Jump) && (Input.GetButton("Jump")))
            {
                if (audioSources != null)
                {
                    audioSources[0].Play();
                }

                rigid.velocity = new Vector2(rigid.velocity.x, jump_power * 1.08f);
            }
        }
        else
        {
            //좌우키 누른대로 이동
            if (horizontal != 0)
            {
                rigid.velocity = new Vector2(max_speed * horizontal, rigid.velocity.y);
            }
        }

        //점프키누르면 점프력 주기
        //이 속력일때만 점프가능함(땅에는 붙어있다고 치는 속력)
        if (rigid.velocity.normalized.y > -JUMP_CRITERIA && rigid.velocity.normalized.y < JUMP_CRITERIA)//y방향성이 없을 때 눌러야 함.
        {
            //점프중 아니고 && 점프무시변수 안 켜진 상태고 && 플레이어 스테이트가 점프가 아니고&&스페이스바 눌렸고&&jump가 true고 && 보스맵4가 아니면
            if (!is_jump && (!ignoreJump) && (player_state != PlayerState.Jump) && (Input.GetButton("Jump")) && SceneManager.GetActiveScene().name != Define.Scene.SnowBoss4.ToString())//점프 bool값이 false 이고, 천장에 붙은 상태면 점프 안 되고(!ignoreJump), state가 Jump가 아니어야하고, 점프 버튼이 눌려야하고, SnowBoss4씬이 아니어야 함(SnowBoss4씬은 무한점프이므로.) 
            {

                if (audioSources != null)
                {
                    //소리재생
                    audioSources[0].Play();
                }
                //점프를 한 순간 is_jump=true. 이단점프 방지용 변수
                is_jump = true;
                //점프력 주기
                rigid.velocity = new Vector2(rigid.velocity.x, jump_power);

            }
        }
        //경사면 점프ok
        if (Input.GetButton("Jump") && is_Slope && !is_jump)
        {
            if (audioSources != null)
            {
                audioSources[0].Play();
            }
            is_jump = true;//점프를 한 순간 is_jump=true. 이단점프 방지용 변수
            rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
            is_Slope = false;//점프를 한 순간 경사가 아니니까
        }
        //브레이크
        if (!isIce && Input.GetButtonUp("Horizontal"))
        {
            //normalized: 벡터크기를 1로 만든 상태. 방향구할 때 씀
            //방향에 속력을 0으로 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0000001f, rigid.velocity.y);
        }

        if (Input.GetButton("Horizontal") && player_state != PlayerState.Damaged)
        {
            //방향전환
            sprite_renderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
    }

    private void TouchProcess()
    {
        // 현재 발생 중인 모든 터치 정보 가져오기
        Touch[] touches = Input.touches;
        // 아래는 각 터치에 대한 처리
        //1. 브레이크
        if (!isIce) //손가락이 1개거나 없어야함
        {
            if (Input.touchCount == 1)//손가락이 있는 경우면
            {
                //터치정보 갖고옴
                Touch touch = Input.GetTouch(0);
                //좌우키 범위에 들어가면 안 됨
                if (!(touch.position.x >= 0 && touch.position.x < rightButtonEnd))
                {
                    //이 때 브레이크 걸어주기
                    rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0000001f, rigid.velocity.y);
                }
            }
            else if (Input.touchCount == 0)
            {
                //이 때 브레이크 걸어주기
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0000001f, rigid.velocity.y);
            }
        }
        //2. 좌우 및 점프키
        foreach (Touch touch in touches)
        {
            //왼쪽키 범위
            if (touch.position.x >= 0 && touch.position.x < leftButtonEnd)
            {
                rigid.velocity = new Vector2(max_speed * -1, rigid.velocity.y);
                if (player_state != PlayerState.Damaged)
                {
                    //방향전환
                    sprite_renderer.flipX = true;
                }
            }
            //오른쪽키 범위
            if (touch.position.x >= leftButtonEnd && touch.position.x < rightButtonEnd)
            {
                rigid.velocity = new Vector2(max_speed * 1, rigid.velocity.y);
                if (player_state != PlayerState.Damaged)
                {
                    //방향전환
                    sprite_renderer.flipX = false;
                }
            }
            //점프키범위
            if (touch.position.x >= Screen.width * 0.5f && touch.position.x < jumpButtonEnd)
            {
                isMoss = 1;
                //그리고 점프중일때 또 점프하지못하게 해야함
                //점프키누르면
                //노말라이즈드로 한 거 이제보니 항상 -1,1,0만 되니까 JUMP_CRITERIA 변수가 의미없어지네..?
                if (rigid.velocity.normalized.y > -JUMP_CRITERIA && rigid.velocity.normalized.y < JUMP_CRITERIA)//y방향성이 없을 때 눌러야 함.
                {
                    //점프중 아니어야하고, 점프무시가 켜져있는 상태가 아니어야하고, 점프상태가 아니어야하고, jump변수 true고, 씬이 보스4가 아닐 때 
                    if (!is_jump && (!ignoreJump) && (player_state != PlayerState.Jump) && SceneManager.GetActiveScene().name != Define.Scene.SnowBoss4.ToString())//점프 bool값이 false 이고, 천장에 붙은 상태면 점프 안 되고(!ignoreJump), state가 Jump가 아니어야하고, 점프 버튼이 눌려야하고, SnowBoss4씬이 아니어야 함(SnowBoss4씬은 무한점프이므로.) 
                    {
                        if (audioSources != null)
                        {
                            //점프소리재생
                            audioSources[0].Play();
                        }
                        //점프를 한 순간 is_jump=true. 이단점프 방지용 변수
                        is_jump = true;
                        //점프력 가하기
                        rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
                    }
                }
                //경사면 점프ok
                if (is_Slope && !is_jump)
                {
                    if (audioSources != null)
                    {
                        audioSources[0].Play();
                    }
                    is_jump = true;//점프를 한 순간 is_jump=true. 이단점프 방지용 변수
                    rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
                    is_Slope = false;//점프를 한 순간 경사가 아니니까
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //경사면
        if (collision.gameObject.CompareTag("Slope"))
        {
            //경사체크 켜주고
            is_Slope = true;
            //여기서는 중력 세게 받게
            rigid.gravityScale = 3;
            //플레이어 상태를 Walk로. Jump상태가 되지 않도록 Walk상태로 바꿔줌 
            player_state = PlayerState.Walk;
        }
        else
        {
            //경사 아니면 경사 아니라고 대입해주기
            is_Slope = false;
        }
        //땅이면
        if (collision.gameObject.CompareTag("Platform"))
        {
            rigid.gravityScale = 2;//땅에 착지하면 중력스케일 원상복구
        }
        //적이면
        if (collision.gameObject.CompareTag("Enemy"))
        {
           
            //주금
            Die(collision.transform.position);
        }
    }

    //콜라이더 닿는동안
    private void OnCollisionStay2D(Collision2D collision)
    {
        //땅에 닿는동안
        if (collision.gameObject.CompareTag("Platform"))
        {
            //왜 저 속력했는지 기억은 안 나는데 그 때 엄청 디버깅했으니 어떤 이유가 있을거임
            if (rigid.velocity.normalized.y < 0.005f && rigid.velocity.normalized.y > -0.005f)
            {
                //보스맵4 아니면
                if (SceneManager.GetActiveScene().name != Define.Scene.SnowBoss4.ToString())
                {
                    //x에 방향성이 있으면 걷기상태로 전환
                    if (rigid.velocity.normalized.x != 0)
                    {
                        anim.SetBool("isJump", false);
                        anim.SetBool("isWalk", true);
                        if (player_state != PlayerState.Damaged)
                        {
                            player_state = PlayerState.Walk;
                        }
                    }
                    else//x에 방향성이 없으면 idle (위 if문이 true가 아니면 else)
                    {

                        anim.SetBool("isJump", false);
                        anim.SetBool("isWalk", false);
                        if (player_state != PlayerState.Damaged)
                        {
                            player_state = PlayerState.Idle;
                        }
                    }
                }
            }
        }
        //적에닿는동안
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //주금
            Die(collision.transform.position);
        }
        //서현코드
        else if (collision.gameObject.CompareTag("BurnEnemy"))
        {
            StartCoroutine(BurnAndDie());
            Die(collision.transform.position);
        }
        //보스여도 주금
        else if (collision.gameObject.CompareTag("Boss"))
        {
            Die(collision.transform.position);
        }
    }

    //트리거 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //소연코드
        if (collision.gameObject.CompareTag("Flag"))
        {
            //리스폰 위치를 해당 Flag 위치로 재설정
            Vector3 flagPosition = collision.gameObject.transform.position;
            SaveLoad.GetComponent<SaveLoad>().SaveRespawn("respawn", flagPosition);
        }
        else if (collision.gameObject.CompareTag("Item"))//없어도되는코드..?
        {
            //진짜 아이템 먹었을 때 animation 바꿈
            anim.SetBool("isItemGet", true);
            collision.gameObject.SetActive(false);
        }
        //보더면
        else if (collision.gameObject.CompareTag("Border"))
        {
            //무적 아니면
            if (!Invincibility)
            {
                if (audioSources != null)
                {
                    //퍽 소리
                    audioSources[1].Play();
                }
                //플레이어상태전환
                player_state = PlayerState.Damaged;
                //통과하는 레이어로 바꿈
                this.gameObject.layer = 7;
                //보더 감지 트루
                isBorder = true;
                //죽음처리 함수 불러옴(UI)
                GameManager.instance.OnPlayerDead();
            }
        }
    }

    //트리거에 닿는동안
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isWater = true;
        }
        //현민코드
        if (!isIce && collision.gameObject.CompareTag("Ice"))
        {
            isIce = true;
        }
    }
    //트리거 나가면
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isWater = false;
        }
        //현민코드
        if (collision.gameObject.CompareTag("Ice"))
        {
            isIce = false;
        }
    }

    //주금 처리
    //타겟포지션 받는 이유: 그 적의 반대방향으로 얘가 통 튀어야하니까
    public void Die(Vector2 targetPos)
    {
        //무적아니면
        if (!invincibility)
        {
            //주금 처리 함수
            onDamaged(targetPos);
            //게임 매니저의 게임오버 처리 실행
            GameManager.instance.OnPlayerDead();
        }
    }
    //주그면
    //타겟포지션: 에네미의 반대방향으로 통 튀려고
    public void onDamaged(Vector2 targetPos)
    {

        if (audioSources != null)
        {
            //퍽소리
            audioSources[1].Play();
        }

        //맞은 상태
        player_state = PlayerState.Damaged;
        //레이어변경
        this.gameObject.layer = 7;
        //리액션
        if(SceneManager.GetActiveScene().name == Define.Scene.SnowBoss4.ToString()||isWater)
        {
            //터져죽기
            anim.SetBool("isBrokenDie", true);
        }
        else
        {

            //튀어올라죽기
            //투명하게 바꾸기
            sprite_renderer.color = new Color(1, 1, 1, 0.4f);
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            if (!isBorder)//보더면 통 튀면 안 되니까..
            {
                rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
            }
        }
        
    }

    //화면밖으로 나감: 죽음
    private void OnBecameInvisible()
    {
        //무적아니면
        if (!invincibility)
        {
            if (audioSources != null)
            {
                //퍽
                audioSources[1].Play();
            }
            //데미지드 상태로 전환
            player_state = PlayerState.Damaged;
            //통과되는 레이어로 바꿈
            this.gameObject.layer = 7;
            //주금처리함수 불러옴(UI)
            GameManager.instance.OnPlayerDead();
            //이 게임 오브젝트 꺼줌
            this.gameObject.SetActive(false);
        }
        
    }

    //서현 코드
    IEnumerator BurnAndDie()
    {
        player_state = PlayerState.Damaged;
        this.gameObject.layer = 7;
        anim.SetBool("isExplosion", true);
        anim.SetBool("isJump", false);
        yield return new WaitForSeconds(1.0f);
    }

    private void SnowBoss4()
    {
        //보스맵4에서는 날아가는 애니메이션으로 
        if (!anim.GetBool("isFly") && SceneManager.GetActiveScene().name == Define.Scene.SnowBoss4.ToString())
        {
            //애니메이션: 계속 날아가는거로.
            anim.SetBool("isFly", true);
        }
        
        //무한점프
        if (SceneManager.GetActiveScene().name == Define.Scene.SnowBoss4.ToString())
        {
            InfiniteJump();   
        }

    }
    private void InfiniteJump()
    {
        //pc
        if (Input.GetButton("Jump") && player_state != PlayerState.Damaged)
        {
            player_state = PlayerState.Fly;
            rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
        }
        Touch[] touches = Input.touches;
        //모바일
        foreach (Touch touch in touches)
        {
            //점프키범위
            if (touch.position.x >= Screen.width * 0.5f && touch.position.x < jumpButtonEnd)
            {
                player_state = PlayerState.Fly;
                rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
            }
        }
    }
    private void Swim()
    {
        if (SceneManager.GetActiveScene().name == Define.Scene.MerMaid.ToString()||SceneManager.GetActiveScene().name=="HyunminMermaid3"|| SceneManager.GetActiveScene().name == "MerMaid_YUJIN"||SceneManager.GetActiveScene().name == "MerMaid_Soyeono" || SceneManager.GetActiveScene().name == "MerMaidBoss" || SceneManager.GetActiveScene().name == "YujinBoss" || SceneManager.GetActiveScene().name == "MerMaidBoss_Soyeon")
        {
            if (isWater)
            {
                jump_power = 6;
                InfiniteJump();
            }
            else
            {
                jump_power = 16;
            }
        }
    }
    //서현
    /// <summary>
    /// 진짜 아이템 먹으면 애니메이션 컨트롤러 바꿔주기
    /// </summary>
    public void ChangeSprites()
    {       
        //스킨선택따라 갈아입기
        string changeClothes = PlayerPrefs.GetString("SnowWhiteCloth");
        switch (changeClothes)
        {
            case "true":
                //애니메이터 바꿔주는 코드 넣어주기.
                anim.runtimeAnimatorController =
                                    (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Anim/SnowSkinPlayer",
                                        typeof(RuntimeAnimatorController)));
                break;
            case "false":
                //기본 스킨
                anim.runtimeAnimatorController =
                    (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Anim/Player_Default",
                        typeof(RuntimeAnimatorController)));
                break;
            //선택을 안 해서 값이 없을경우 기본스킨
            default:
                anim.runtimeAnimatorController =
                    (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Anim/Player_Default",
                        typeof(RuntimeAnimatorController)));
                break;
        }
        //아이템얻었을경우
        string realItem = PlayerPrefs.GetString("RealItem");
        switch (realItem)
        {
            case "SnowWhite":
                //기본스킨일경우 배낭스킨
                anim.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Anim/Player_RealItem_SnowWhite",
                    typeof(RuntimeAnimatorController)));
                if (changeClothes == "false")
                {
                    anim.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Anim/Player_RealItem_SnowWhite",
                        typeof(RuntimeAnimatorController)));
                }
                //백설스킨일경우 배낭스킨
                if (changeClothes == "true")
                {
                    anim.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Anim/SnowSkinPlayer_RealItem",
                        typeof(RuntimeAnimatorController)));
                }
                break;
        }
    }
}

