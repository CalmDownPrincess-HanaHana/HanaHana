using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

/// <summary>
/// 백설공주4패턴 스크립트
/// </summary>
public class SnowBoss4 : MonoBehaviour
{
    //얘가 내는 소리들 
    public AudioSource[] audioSources;
    //클리어했을 때 UI (11.19시점 베타버전 플레이했네요 짝짝~으로 되어있음)
    public GameObject clearUI;
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

        //랜덤패턴시작
        RandomPattern();
    }

    // Update is called once per frame
    void Update()
    {
        //클리어유아이 떴으면 스테이지 씬으로 돌려보내기(베타용임.)
        //엔딩씬으로 가도록 추가개발필요
        if (clearUI.active == true)
        {
            Invoke("gotoStageScene", 3f);
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
            if (!once)
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
        //보스 죽으면 플레이어 무적으로 만들어줘야함
        player_script.Invincibility = true;
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
            if (launchers[2].active || launchers[3].active)
            {
                launchers[2].SetActive(false);
                launchers[3].SetActive(false);
            }
            //쿠광쾅콰광(소리+ 화면흔들림+ 폭발애니메이션: 이미 구현한 코더분들거 쌔벼오기)
            StartCoroutine(AfterDead());

            //거울쨍그랑(쨍그랑 애니메이션 후->거울 deactive-> 원형 프리팹 이용해 거울 파편 퍼져나가기)   
            Invoke("mirrorDeactive", 11f);

            //시연용 UI띄우기(와 짝짝~)
            Invoke("showClearUI", 20f);
            //페이드인페이드아웃(이미 구현 쌔벼오기) white ver. -> 씬이동(잠잠해짐 씬으로 이동)
        }

    }
    //거울 삭제
    private void mirrorDeactive()
    {
        //쿠광광소리끄고
        audioSources[0].Stop();
        foreach (GameObject obj in afterKilledFlames)
        {
            //불도 꺼주고
            obj.SetActive(false);
        }
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
    }

    private void showClearUI()
    {
        clearUI.SetActive(true);
        //짝짞~
        audioSources[2].Play();
    }

    //죽고나서 일어나는 일들
    IEnumerator AfterDead()
    {
        //코루틴 다 꺼주고
        StopAllCoroutines();
        foreach (GameObject obj in afterKilledFlames)
        {
            //불꽃 켜주고
            obj.SetActive(true);
        }

        // 카메라 shaking
        Camera.main.transform.DOShakePosition(10, 3);

        // 보스 애니메이션 변경(절규)
        anim.SetBool("isHideEye", true);

        // 불 스프라이트는 자동 재생

        //폭발사운드
        audioSources[0].loop = true;
        audioSources[0].Play();


        // 다음 씬 로드 : 보스 애니메이션 끝나고 이동
        yield return new WaitForSeconds(10);
    }

}