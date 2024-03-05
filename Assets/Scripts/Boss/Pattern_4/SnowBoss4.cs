using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 백설공주4패턴 스크립트
/// </summary>
public class SnowBoss4 : MonoBehaviour
{
    //쨍그랑 판넬
    [SerializeField] private Image zzangrang;
    //플레이어스크립트
    Player player_script;
    /// <summary>
    /// 페이드아웃 & 씬로드 변수
    /// </summary>
    //페이드아웃 bool 변수
    private bool fade_out = false;
    //타이머
    private float timer = 0;
    //페이드아웃 판넬 UI 
    [SerializeField] private Image fadeOutPanel;

    /////////////////////////////////////////////////

    //얘가 내는 소리들 
    public AudioSource[] audioSources;
    //죽고나서 터지는 불들
    [SerializeField] GameObject[] afterKilledFlames;

    //발사블록들
    public GameObject[] launchers;
    //발사블록1의 스크립트
    private Launch_FireAndBullet launcher0_script;
    //발사블록2의 스크립트
    private Launch_FireAndBullet launcher1_script;
    //발사블록 34 스크립트(미사일용 발사블록)
    private Launch_FireAndBullet guidedMissleLuncher1_script;
    private Launch_FireAndBullet guidedMissleLuncher2_script;

    //주그면 나오는 거울 깨진 조각들
    [SerializeField]
    private List<GameObject> mirrors;
    //맞으면 빨간색 반짝반짝 타이머
    private float hit_timer = 0;
    //패턴타이머
    private float pattern_timer = 0;
    //한 패턴당 10초는 고정
    private const float PATTERN_TIME = 10;
    //거울렌더러
    public SpriteRenderer mirror_renderer;
    //보스피설정
    [SerializeField]
    public float boss_hp;
    //플레이어 총알
    private GameObject P_bullet;
    //플레이어총알 스크립트
    private FireAndBullet P_bullet_script;
    //움직이는속도
    [SerializeField]
    private float move_speed;
    //애니메이션 타이머
    private float anim_timer;
    //애니메이션 켜지는 조건변수
    private bool turn_on_anim_timer = false;

    //버드 패턴에서 쓰는 변수라는 뜻에서 말머리 붙임
    private List<Vector3> B_target_positions = new List<Vector3>();
    private int B_current_target_index = 0;

    //고블린 패턴에서 쓰는 변수라는 뜻에서 말머리 붙임
    private List<Vector3> G_target_positions = new List<Vector3>();
    private int G_current_target_index = 0;
    private Animator anim;
    private Animator mirror_anim;
    private Animator naruto_anim;
    public SnowBoss4State boss_state = new SnowBoss4State();
    private Vector3 Boss_initial_position = new Vector3(23.05f, 0.27f, 0);
    private List<Vector3> Launcher_initial_position = new List<Vector3>()
    {
        new Vector3(21.9f, -0.25f),
        new Vector3(21.9f, -0.25f)
    };

    //주금
    private bool is_dead = false;
    //죽는거 한 번만 실행해야하니까 그거 위한 변수
    private bool once = false;

    [SerializeField] private GameObject startUI;
    private void Awake()
    {
        Transform child_transform = this.transform.GetChild(1);
        //미러 애니메이션 갖고오기(쨍그랑)
        mirror_anim = child_transform.GetComponent<Animator>();
        child_transform = this.transform.GetChild(2);
        //나루토 애니메이션 갖고오기(샤샥)
        naruto_anim = child_transform.GetComponent<Animator>();
        //보스 애니메이션(공주)
        anim = GetComponent<Animator>();
        //발사 박스 스크립트들 갖고오기
        launcher0_script = launchers[0].GetComponent<Launch_FireAndBullet>();
        launcher1_script = launchers[1].GetComponent<Launch_FireAndBullet>();
        //미사일 박스 스크립트들 갖고오기
        guidedMissleLuncher1_script = launchers[2].GetComponent<Launch_FireAndBullet>();
        guidedMissleLuncher2_script = launchers[3].GetComponent<Launch_FireAndBullet>();

        //플레이어불렛
        P_bullet = GameObject.FindWithTag("bullet").GetComponent<Launch_FireAndBullet>().fire;
        //플레이어불렛 스크립트
        P_bullet_script = P_bullet.GetComponent<FireAndBullet>();

        // 원하는 목표 위치들을 리스트에 추가
        B_target_positions.Add(new Vector3(23.05f, 0.27f, 0));
        B_target_positions.Add(new Vector3(19.93f, 3.08f, 0));
        B_target_positions.Add(new Vector3(17.52f, 2.07f, 0));
        B_target_positions.Add(new Vector3(18.13f, 0.93f, 0));
        B_target_positions.Add(new Vector3(15.58f, -0.69f, 0));
        B_target_positions.Add(new Vector3(16.46f, -2.62f, 0));
        B_target_positions.Add(new Vector3(18.57f, -1.87f, 0));
        B_target_positions.Add(new Vector3(20.06f, -3.58f, 0));
        B_target_positions.Add(new Vector3(23.48f, -1.96f, 0));

        // 원하는 목표 위치들을 리스트에 추가
        G_target_positions.Add(new Vector3(23.05f, 0.27f, 0));
        G_target_positions.Add(new Vector3(22.07f, 3.52f, 0));
        G_target_positions.Add(new Vector3(23.72f, 2.33f, 0));
        G_target_positions.Add(new Vector3(17.22f, -0.66f, 0));
        G_target_positions.Add(new Vector3(18.92f, -2f, 0));
        G_target_positions.Add(new Vector3(21.55f, -1.85f, 0));
        //플레이어스크립트가져오기
        player_script = GameObject.FindWithTag("Player").GetComponent<Player>();
        //점프안내판넬 활성화
        StartCoroutine(showStartText());
        //랜덤패턴시작
        RandomPattern();
    }

    // Update is called once per frame
    void Update()
    {
        
        //카메라흔들리는것때문에 죽게되는 경우를 위한 코드
        //무적이더라도 화면안에 플레이어가 없으면 죽음
        if (!(player_script.gameObject.transform.position.y <= 5.5 && player_script.gameObject.transform.position.y >= -5.5))
        {
            player_script.Invincibility = false;
            player_script.Die(player_script.transform.position);
        }

        //안죽었으면 패턴진행(스포당하면 안되니까)
        if (!is_dead)
        {
            //맞으면 빨간색 처리됐던걸 원상복구
            hit_timer += Time.deltaTime;
            if (hit_timer >= 0.15f)
                //0.15초 지나면 거울 원상복구
                mirror_renderer.color = new Color(1, 1, 1, 0.7f);
            //애니메이션타이머(나루토 샤샥)
            if (turn_on_anim_timer)
                anim_timer += Time.deltaTime;

            //패턴
            pattern_timer += Time.deltaTime;
            if (pattern_timer >= PATTERN_TIME)
            {
                //클론된 오브젝트들 삭제(싹 청소)
                DeleteCloneObjects();
                //세팅
                WhenPatternChangeSetting();
                //랜덤패턴시작
                RandomPattern();
                //나루토애니메이션(샤샥)
                naruto_anim.SetBool("pattern_change", true);
                //타이머 셋
                pattern_timer = 0;
                //애니메이션 타이머 재생
                turn_on_anim_timer = true;
            }

            //샤샥 하고 1초 지났으면
            if (anim_timer >= 1f && naruto_anim.GetBool("pattern_change"))
            {
                //애니메이션꺼주기
                naruto_anim.SetBool("pattern_change", false);
                anim_timer = 0;
                turn_on_anim_timer = false;
            }
            //4_1패턴이면 폴리곤 무브로 움직임
            if (boss_state == SnowBoss4State.pattern4_1 || boss_state == SnowBoss4State.pattern4_4)
            {
                PolygonMove();
            }
            //4_5패턴이면 지그재그 움직임
            else if (boss_state == SnowBoss4State.pattern4_5)
            {
                ZigZagMove();
            }
        }

        //깼을 때
        if (boss_hp <= 0)
        {
            //여기로 갈거임
            Vector3 targetPosition = Boss_initial_position;
            // 현재 위치와 목표 위치 사이의 거리 계산
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // 목표 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * move_speed);

            //죽는거 한 번만 실행해야하니까 once변수 씀
            if (!once )
            {
                Dead();
                once = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //총알(플레이어가 쏘는거) 맞으면
        if (collision.gameObject.CompareTag("bullet"))
        {
            //빨갛게 처리
            mirror_renderer.color = new Color(1, 0.54f, 0.54f, 0.77f);
            //총알 데미지만큼 피 깎기
            boss_hp -= P_bullet_script.bullet_damage;
            //타이머 초기화
            hit_timer = 0f;
        }
        //무적이어도 보스에게 닿으면 플레이어 주금
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player_script.Invincibility)
            {
                if (this.boss_hp <= 0)
                {
                    player_script.Invincibility = false;
                }
            }
        }
    }
    //패턴1
    private void Pattern1()
    {
        //스테이트바꿔주고
        boss_state = SnowBoss4State.pattern4_1;
        //패턴 1에서 재생하는 프리팹 세팅
        launcher0_script.fires_index = 0;
        launcher1_script.fires_index = 0;
    }
    //패턴2
    private void Pattern2()
    {
        //스테이트바꿔주고
        boss_state = SnowBoss4State.pattern4_2;
        //원형 사과
        launcher0_script.fires_index = 1;

        //다이아 사과
        launcher1_script.fires_index = 1;
    }

    //패턴3
    private void Pattern3()
    {
        //스테이트바꿔주고
        boss_state = SnowBoss4State.pattern4_3;
        //바람개비
        launcher0_script.fires_index = 2;
        //불꽃놀이
        launcher1_script.fires_index = 2;
    }
    //패턴4
    private void Pattern4()
    {
        //스테이트바꿔주고
        boss_state = SnowBoss4State.pattern4_4;
        //원형애플
        launcher0_script.fires_index = 1;
        launcher1_script.fires_index = -1;//아무것도 안 한다는 뜻.
        //조준탄 박스 켜주기
        launchers[2].SetActive(true);
        launchers[3].SetActive(true);

        guidedMissleLuncher1_script.fires_index = 0;
        guidedMissleLuncher2_script.fires_index = 0;
    }

    //패턴5
    private void Pattern5()
    {
        //스테이트 대입
        boss_state = SnowBoss4State.pattern4_5;

        //원형애플(C모양)
        launcher0_script.fires_index = 1;
        //고블린 던지기
        launcher1_script.fires_index = 3;

    }
    //랜덤패턴
    private void RandomPattern()
    {
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0:
                Pattern1();
                break;
            case 1:
                Pattern2();
                break;
            case 2:
                Pattern3();
                break;
            case 3:
                Pattern4();
                break;
            case 4:
                Pattern5();
                break;
        }
    }

    //패턴바뀌면 세팅
    private void WhenPatternChangeSetting()
    {
        //훅 소리 재생
        GetComponent<AudioSource>().Play();
        //런처박스들 쿨타임초기화
        this.launcher0_script.Cool_Time = 0;
        this.launcher1_script.Cool_Time = 0;
        //원위치로 나루토함
        this.transform.position = Boss_initial_position;
        //미사일박스 꺼줌
        launchers[2].SetActive(false);
        launchers[3].SetActive(false);
        //런처박스들도 위치초기화
        launchers[0].transform.position = Launcher_initial_position[0];
        launchers[1].transform.position = Launcher_initial_position[1];
    }

    //지그재그로 움직임
    private void ZigZagMove()
    {
        //지그재그 위치점 찍어놨던 배열에서 하나씩 꺼냄
        Vector3 targetPosition = G_target_positions[G_current_target_index];
        // 현재 위치와 목표 위치 사이의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        // 일정 거리 내에 있으면 다음 목표 위치로 변경
        if (distanceToTarget <= 0.1f) // 예시로 0.1f를 사용, 원하는 값으로 조정 가능
        {
            G_current_target_index = (G_current_target_index + 1) % G_target_positions.Count;
            targetPosition = G_target_positions[G_current_target_index];
        }

        // 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * move_speed);

    }

    //폴리곤모양 이동
    private void PolygonMove()
    {

        Vector3 targetPosition = B_target_positions[B_current_target_index];
        // 현재 위치와 목표 위치 사이의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        // 일정 거리 내에 있으면 다음 목표 위치로 변경
        if (distanceToTarget <= 0.1f) // 예시로 0.1f를 사용, 원하는 값으로 조정 가능
        {
            B_current_target_index = (B_current_target_index + 1) % B_target_positions.Count;
            targetPosition = B_target_positions[B_current_target_index];
        }

        // 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * move_speed);

    }

    //짠탄제거
    public void DeleteCloneObjects()
    {
        // 씬 내의 모든 게임 오브젝트 가져오기
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            //클론된 애면
            if (IsClone(obj))
            {
                Destroy(obj); // 클론 오브젝트 삭제
            }
        }
    }

    //클론된애인지 판별
    private bool IsClone(GameObject obj)
    {
        // 이름에 "(Clone)" 문자열이 포함되어 있는지 검사
        return obj.name.Contains("(Clone)");
    }

    //주금
    private void Dead()
    {
        Player player_script = GameObject.FindWithTag("Player").GetComponent<Player>();
        
        //플레이어가 데미지드 아닌 상태여야함
        if (player_script.player_state != PlayerState.Damaged)
        {
            //짠탄제거
            DeleteCloneObjects();
            //주금처리
            is_dead = true;
            //발사 박스 비활성화
            launchers[0].SetActive(false);
            launchers[1].SetActive(false);
            //미사일박스도 활성화되어있으면 꺼줌
            if (launchers[2].activeSelf || launchers[3].activeSelf)
            {
                launchers[2].SetActive(false);
                launchers[3].SetActive(false);
            }
            //쿠광쾅콰광(소리+ 화면흔들림+ 폭발애니메이션: 이미 구현한 코더분들거 쌔벼오기)
            StartCoroutine(AfterDead());
            //거울쨍그랑(쨍그랑 애니메이션 후->거울 deactive-> 원형 프리팹 이용해 거울 파편 퍼져나가기)   
            Invoke("mirrorDeactive", 11f);
                   
        }

    }
    //거울 삭제
    private void mirrorDeactive()
    {
        //무적해제
        player_script.Invincibility = false;
        //무적해제했을 때 화면안에 플레이어가 없으면 죽음
        if(!(player_script.gameObject.transform.position.y<=5.5&& player_script.gameObject.transform.position.y >=-5.5))
        {
            player_script.Die(player_script.transform.position);
        }

        //쿠광광소리끄고
        audioSources[0].Stop();
        //비명소리끄고
        audioSources[2].Stop();
        foreach (GameObject obj in afterKilledFlames)
        {
            //불도 꺼주고
            obj.SetActive(false);
        }
        //쨍그랑 판넬 키기
        zzangrang.gameObject.SetActive(true);
        //쨍그랑 애니메이션 켜주고
        mirror_anim.SetBool("isDead", true);
        //쨍그랑 틀어주고
        audioSources[1].Play();
        //절규 애니메이션 꺼주고
        anim.SetBool("isHideEye", false);
        //거울 치워주고
        mirror_anim.gameObject.SetActive(false);
        //원형모양으로 거울조각 날아가고
        CirclePattern circle_pattern = new CirclePattern();
        foreach (GameObject obj in mirrors)
        {
            obj.SetActive(true);
        }
        circle_pattern.CircleLaunch(mirrors, this.transform, 10);
        //쨍그랑
        audioSources[1].Play();
        StartCoroutine(FadeOutAndSceneLoad());
        
    }

    //죽고나서 일어나는 일들
    IEnumerator AfterDead()
    {
        yield return null;
        //코루틴 다 꺼주고
        StopAllCoroutines();
        //문구띄워주기
        StartCoroutine(ShowLastText());
        foreach (GameObject obj in afterKilledFlames)
        {
            //불꽃 켜주고
            obj.SetActive(true);
        }

        //카메라를 흔들어주기때문에 카메라 안에 안 담기면 죽는 불상사 막기위함.
        player_script.Invincibility = true;
        
        // 카메라 shaking
        Camera.main.transform.DOShakePosition(10, 3);

        // 보스 애니메이션 변경(절규)
        anim.SetBool("isHideEye", true);

        // 불 스프라이트는 자동 재생

        //폭발사운드
        audioSources[0].loop = true;
        audioSources[0].Play();
        //절규사운드
        audioSources[2].loop = true;
        audioSources[2].Play();
        
    }

    IEnumerator FadeOutAndSceneLoad()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        AudioSource bgmAudio= gameManager.GetComponent<AudioSource>();
        yield return new WaitForSeconds(5f);
        if (player_script.player_state != PlayerState.Damaged)
        {
            //페이드아웃 켜지고
            fade_out = true;
            //페이드아웃 판넬의 부모오브젝트(캔버스) 가져옴
            Transform parent_trans = fadeOutPanel.gameObject.transform.parent;
            //켜주기
            parent_trans.gameObject.SetActive(true);
            //페이드아웃 조건변수 켜지면
            if (fade_out)
            {
                player_script.Invincibility = true;//페이드아웃일 때는 무적.
                while (true)
                {
                    //타이머 키고
                    timer += Time.deltaTime;
                    //색 까매지게
                    fadeOutPanel.color += new Color(0, 0, 0, 0.01f);
                    //브금 줄어들게
                    bgmAudio.volume -= 0.01f;
                    if (fadeOutPanel.color.a >= 0.7f)
                    {
                        player_script.gameObject.SetActive(false);
                    }
                    if (timer >= 5f)
                    {
                        
                        //n초 되면 씬로드
                        if(SceneManager.GetActiveScene().name == Define.Scene.SnowBoss4.ToString())
                        {
                            SceneManager.LoadScene(Define.Scene.SnowBossClear.ToString());
                        }
                        break;
                    }
                    yield return null;
                }
            }
        }
    }
    //무적
    IEnumerator PlayerInvincibility(float time)
    {
        SpriteRenderer playerSprite = player_script.gameObject.GetComponent<SpriteRenderer>();
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0.5f);
        player_script.Invincibility = true;
        yield return new WaitForSeconds(time);
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        player_script.Invincibility = false;
    }
   IEnumerator ShowLastText()
    {
        startUI.SetActive(true);
        startUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keep jumping!";
        yield return new WaitForSeconds(2f);
        startUI.SetActive(false);
        yield return null;
    }
    IEnumerator showStartText()
    {
        startUI.SetActive(true);
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < 2f)
        {
            yield return null;
        }

        startUI.SetActive(false);
        Time.timeScale = 1;
        yield return null;
    }

}
