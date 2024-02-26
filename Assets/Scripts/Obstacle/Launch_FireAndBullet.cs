using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

/// <summary>
/// Fire 또는 Bullet을 "발사"시키는 애
/// </summary>
public class Launch_FireAndBullet : MonoBehaviour
{
    /// <summary>
    /// 쿨타임 관련 변수
    /// </summary>
    //쿨타임
    [SerializeField]
    private float cool_time = 1f;
    //쿨타임 찼는지 확인하는 타이머
    private float time = 0;


    /// <summary>
    /// //애플 패턴(4_2)
    /// 마름모 모양으로 빈칸 생겼다가 줄어들었다가 하는 빨간색사과 패턴
    /// </summary>
    // 열에서 최대 사과 갯수
    private const int MAX_APPLE_NUM = 12;
    // 12개 사과의 발사지점 
    private List<Transform> start_points = new List<Transform>();
    // 사과의 열임. 열마다 발사갯수가 달라짐
    private int a_pattern_count = 0;
    // 1 or -1이고 ↘↗방향결정
    private int a_pattern_direction = 1;
    //발사지점
    private List<List<int>> initial_index = new List<List<int>>
    {
        //0열에서는 4개발사
        new List<int> { 4,5,10,11 },
        //1열에서는 6개발사
        new List<int> { 3,4,5,9,10,11},
        //2열에서는 8개발사
        new List<int> { 2,3,4,5,8,9,10,11},
        //3열에서는 10개발사
        new List<int> { 1,2,3,4,5,7,8,9,10,11},
        //4열에서는 12개발사
        new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }
    };

    //플레이어 정보 가져오기
    private Player player_script;
    //총알이 하나면 이 변수에 프리팹 넣기
    public GameObject fire;
    //쏘는 블록의 위치(스프라이트 없는 게임오브젝트)
    public Transform pos;
    //총알 프리팹 배열의 인덱스
    public int fires_index;
    //총알(프리팹)이 여러종류면 이 배열에 넣기
    [SerializeField]
    private GameObject[] fires = new GameObject[0];
    //보스 정보 가져오는 변수
    private SnowBoss4 boss_script;

    //캡슐화
    public float Cool_Time
    {
        get
        {
            return cool_time;
        }
        set
        {
            cool_time = value;
        }
    }


    private void Awake()
    {
        //스크립트 가져오기
        player_script = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (SceneManager.GetActiveScene().name == "SnowBoss4"|| SceneManager.GetActiveScene().name == "SnowBoss4Training")
        {
            boss_script = GameObject.FindWithTag("Boss").GetComponent<SnowBoss4>();
        }

        //애플패턴(4_2)
        if (null != fires.FirstOrDefault(fires => fires.name == "Apple"))// "name"이 "Apple"인 항목이 배열에 존재하는지 확인
        {
            //MAX APPLE NUM만큼 위치초기화 반복
            for (int i = 0; i < MAX_APPLE_NUM; i++)
            {
                // "start_point"라는 이름의 새로운 게임 오브젝트를 생성
                //그다음 trans에다가 그거 위치 넣어줌
                Transform trans = new GameObject("start_point").transform;
                if (i < MAX_APPLE_NUM / 2)//위 
                {
                    trans.position = new Vector3(22f, 2f + 0.5f * i, 0);
                }
                else//아래
                {
                    trans.position = new Vector3(22f, -2f - (0.5f * (i % (MAX_APPLE_NUM / 2))), 0);
                }
                //start_points리스트에 그 위치 넣어줌
                start_points.Add(trans);
            }
        }
    }

    private void Update()
    {
        //타이머에 시간 더해주기
        time += Time.deltaTime;

        if (time >= cool_time)
        {
            if (fires.Length >= 1)//배열에 뭔가 넣었을 때(배열은 총알 본인에 대한 프리팹을 담음. 발사블럭에 총알의 종류를 여러개 하고싶으면 이 배열에 담아주면 됨
            {
                if (fires_index == -1)
                {
                    return;
                }
                //안죽었을때
                if (player_script.player_state != Define.PlayerState.Damaged)
                {
                    //그게 분열되는 애일 때(4_1패턴)
                    if (fires[fires_index].name == "Bullet13")//프리팹 네임 대조함
                    {
                        cool_time = 1.5f;//얘의 쿨타임은 1초(1초마다 보스에서 나오는 톱니바퀴 새로생성)
                        //그 프리팹을 클론해줌. 
                        GameObject clone_obj = Instantiate(fires[fires_index], pos.position, transform.rotation) as GameObject;
                        // 클론된 객체에 대해 코루틴 시작
                        StartCoroutine(SplitCoroutine(clone_obj));
                    }
                    //애플 패턴일 때(4_2패턴)
                    else if (fires[fires_index].name == "Apple")
                    {
                        //얘의 발사쿨타임은 0.1초
                        cool_time = 0.1f;

                        int i = 0;
                        int j = 0;
                        //스타트지점의 위치값 차례로 가져옴
                        foreach (Transform trans in start_points)
                        {
                            //발사위치 맞으면 발사(잘 모르겠으면 initial_index 변수 정의 보고오기
                            //(빨간사과 열마다 인덱스가 달라짐)
                            if (i == initial_index[a_pattern_count][j])
                            {
                                //패턴탄알발사
                                Instantiate(fires[fires_index], trans.position, Quaternion.identity);
                                j++;
                            }
                            i++;
                        }
                        if (a_pattern_direction == 1)//↗모양 발사
                        {
                            a_pattern_count++;
                        }
                        else//↘모양 발사
                        {
                            a_pattern_count--;
                        }
                        if (a_pattern_count == 4 || a_pattern_count == 0)//4열일때는 12개고 0열일때는 4개발사인데 이 때 ↗거나 ↘로 방향 꺾여야하니까 그거처리
                        {
                            a_pattern_direction *= -1; // 방향을 반대로 변경
                        }
                    }
                    //불꽃놀이 패턴일 때 (4_3)
                    else if (fires[fires_index].name == "fireworks")
                    {
                        //발사쿨타임 1초
                        cool_time = 1f;
                        //랜덤위치에서 불꽃놀이
                        float x = Random.Range(20f, 24f);
                        float y = Random.Range(2f, -2f);
                        //그냥 아무값이나 넣어준거
                        Transform transf = this.transform;
                        //위치초기화: 랜덤값 넣은 위치로
                        transf.position = new Vector3(x, y, 0);
                        //발사
                        Instantiate(fires[fires_index], transf.position, transform.rotation);
                    }
                    //원형애플패턴
                    else if (fires[fires_index].name == "GreenApple")
                    {
                        //4_2 의 circle apple 패턴
                        if (boss_script.boss_state == SnowBoss4State.pattern4_2)
                        {
                            //쿨타임1초
                            cool_time = 1f;
                            //이 클래스에 원형으로 퍼져나가게 하는 거 정의해놔서 그거 불러오려고 객체생성
                            CirclePattern circle_pattern = new CirclePattern();
                            //그냥 아무값이나 넣어준거
                            Transform transf = this.transform;
                            //x: 18~24
                            //y: 4~-4
                            //랜덤위치
                            float x = Random.Range(20f, 24f);
                            float y = Random.Range(2f, -2f);
                            transf.position = new Vector3(x, y, 0);
                            //퍼져나갈때소리
                            AudioSource audio = GetComponent<AudioSource>();
                            if (audio != null)
                            {
                                audio.Play();
                            }
                            //원형발사
                            circle_pattern.CircleLaunch(fires[fires_index], transf);
                        }
                        //4_4에서의 원형애플
                        else if (boss_script.boss_state == SnowBoss4State.pattern4_4)
                        {
                            //쿨타임 2초
                            cool_time = 2f;
                            //원형발사 함수 갖다쓰려고 객체생성
                            CirclePattern circle_pattern = new CirclePattern();
                            AudioSource audio = GetComponent<AudioSource>();
                            if (audio != null)
                            {
                                audio.Play();//발사할 때 소리 플레이
                            }
                            //원형 발사
                            circle_pattern.CircleLaunch(fires[fires_index], boss_script.gameObject.GetComponent<Transform>());
                        }
                        //4_5패턴이면
                        else if (boss_script.boss_state == SnowBoss4State.pattern4_5)
                        {
                            //쿨타임1초
                            cool_time = 1f;
                            //함수갖다쓰려고 객체생성
                            CirclePattern circle_pattern = new CirclePattern();
                            AudioSource audio = GetComponent<AudioSource>();
                            if (audio != null)
                            {
                                audio.Play();//발사할 때 소리재생
                            }
                            //C모양으로 발사하는 함수 불러옴
                            circle_pattern.CLaunch(fires[fires_index], boss_script.gameObject.GetComponent<Transform>(), 4);
                        }
                    }
                    //GuidedMissle 프리팹일때는(불사조가 쏘는거)
                    else if (fires[fires_index].name == "GuidedMissle")
                    {
                        //쿨타임1초
                        cool_time = 1f;
                        //발사
                        Instantiate(fires[fires_index], pos.position + new Vector3(-1f, 0, 0), transform.rotation);
                    }
                    //바람개비 프리팹일때는(파란색 이쁜 꽃같이 생긴거)
                    else if (fires[fires_index].name == "Pinwheel")
                    {
                        //쿨탐10초
                        cool_time = 10;
                        //발사
                        Instantiate(fires[fires_index], pos.position + new Vector3(1f, 0, 0), transform.rotation);
                    }
                    else//특별한 경우 아니면 그냥 1초 쿨탐 & 발사
                    {   //파란새가 이 경우에 들어감
                        cool_time = 1f;
                        Instantiate(fires[fires_index], pos.position, transform.rotation);
                    }
                }

            }
            else//배열에 안 넣었을 때(총알 하나일 때. SnowWhite에서 쓰는 fire의 경우)
            {
                Instantiate(fire, pos.position, transform.rotation);//그냥 그 위치에서 생성해주고 끝
            }
            time = 0f;//쿨타임 초기화
        }

    }

    /// <summary>
    ///톱니바퀴 3단 나루토분신 코드
    /// </summary>
    /// <param name="clone_obj"></param>
    /// <returns></returns>
    IEnumerator SplitCoroutine(GameObject clone_obj)
    {
        // n 초 대기(n초 기다린 후에 분열될거임)
        yield return new WaitForSeconds(0.5f);

        // 대기 후 split(분열)
        GameObject cclon_obj1 = Instantiate(clone_obj, clone_obj.transform.position + new Vector3(0, 2f, 0), transform.rotation);
        GameObject cclon_obj2 = Instantiate(clone_obj, clone_obj.transform.position + new Vector3(0, -2f, 0), transform.rotation);
        float 초기위치 = clone_obj.transform.position.y;
        float move_time = 0;

        Rigidbody2D cclon_obj1_rigid=cclon_obj1.GetComponent<Rigidbody2D>();
        Rigidbody2D cclon_obj2_rigid = cclon_obj2.GetComponent<Rigidbody2D>();
        //n초동안 각각 위/아래로 퍼짐
        while (move_time <= 0.5f)
        {
            move_time += Time.deltaTime;//타이머 시간 더해주는 중
            // 체크 후에 접근
            if (cclon_obj1 != null && cclon_obj2 != null)
            {
                cclon_obj1_rigid.velocity = new Vector2(0, 5);
                cclon_obj2_rigid.velocity = new Vector2(0, -5);
            }
            yield return null; // 한 프레임을 기다림
        }
        cclon_obj1_rigid.velocity = new Vector2(0, 0);
        cclon_obj2_rigid.velocity = new Vector2(0, 0);
    }
}
