using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*싱글턴 사용: 프로그램 실행 시 유지할 값들
1. 리스폰 포인트
2. 죽은 횟수
3. 게임 오버 상태 및 관리
4. UI 출력을 여기서 하나...?: 굳이 얘를 스태틱으로 관리할 필요가 있나?
게임 오버 출력을 다른 데에서 하면 안 되나?
5. 배경 바꾸기: 여기서 꼭...? 오디오 소스... 꼭? */
public class GameManager : MonoBehaviour
{
    public static GameManager instance; //싱글턴을 할당할 전역변수

    [SerializeField] private Vector3 respawnPoint = new Vector3(-9.16f, -0.48f, 0f); // 플레이어가 리스폰할 체크포인트 위치
    private int death_count = 0;//죽은 횟수
    public bool isGameover = false; //게임오버 상태
    public TextMeshProUGUI death_text;//죽은 횟수를 출력할 UI 텍스트
    public GameObject gameoverUI; //게임오버 시 활성화할 UI 게임오브젝트
    public GameObject player;//플레이어
    public GameObject SaveLoad;
    public GameObject exitPanel;

    /// <summary>
    /// 게임 시작과 동시에 싱글턴을 구성
    /// </summary>



    void Awake()
    {
        //싱글턴 변수 instance가 비어 있는가?
        if (instance == null)
        {
            //instance가 비어 있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 개임 매니저가 존재합니다.");
            Destroy(gameObject);
        }

    }

    void Start()
    {
        //리스폰 위치로 플레이어 위치를 reset함.
        //보스씬들은 리스폰위치에서 태어나면x

        /*      if (SceneManager.GetActiveScene().name == Define.Scene.SnowWhite.ToString())
              {
                  if (SaveLoad.GetComponent<SaveLoad>().LoadRespawn("respawn") != Vector3.zero)
                  {
                      player.transform.position = SaveLoad.GetComponent<SaveLoad>().LoadRespawn("respawn");
                  }
              }else if(SceneManager.GetActiveScene().name == Define.Scene.Mermaid.ToString())
               {
                  if (SaveLoad.GetComponent<SaveLoad>().LoadRespawn("mermaid_respawn") != Vector3.zero)
                  {
                      player.transform.position = SaveLoad.GetComponent<SaveLoad>().LoadRespawn("mermaid_respawn");
                  }
              }
        */


        player.GetComponent<Player>().ChangeSprites();
    }

    void Update()
    {
        if (isGameover && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            //보스씬이면 스노우화이트 세이브포인트부터 시작
            if (SceneManager.GetActiveScene().name == "SnowBoss4" || SceneManager.GetActiveScene().name == "SnowBoss3"|| SceneManager.GetActiveScene().name == "SnowBoss2"|| SceneManager.GetActiveScene().name == "SnowBoss1")
            {
                SceneManager.LoadScene("SnowWhite");
            }
            //게임오버 상태에서 마우스 왼쪽 버튼을 클릭하면 현재 씬 재시작
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        //만약 뒤로가기키 눌렀을때
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f; //시간 정지
            exitPanel.SetActive(true); //exit팝업착 띄우기
        }

    }

    /// <summary>
    /// 플레이어 캐릭터 사망 시 게임오버를 실행하는 메서드
    /// </summary>
    public void OnPlayerDead()
    {
        if (!isGameover)
        {
            //현재 상태를 게임오버 상태로 변경
            isGameover = true;
            //죽은 횟수를 증가

            death_count = SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("death") + 1;

            SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("death", death_count);
            death_text.text = "Death : " + death_count++;
            //게임오버 UI를 활성화
            gameoverUI.SetActive(true);
        }
    }

    public void OnPlayerFinish()
    {
        if (!isGameover)
        {
            // 딜레이를 위한 Invoke 실행
            float delaySeconds = 2f; // 2초
            Invoke("HideFinishUI", delaySeconds); // 지정된 시간(delaySeconds) 후에 HideFinishUI 메서드를 실행합니다.

        }
    }
    //Finish처리 함수
    private void HideFinishUI()
    {
    }

    public void ExitYes()
    {
        Application.Quit();
    }

    public void ExitNo()
    {
        Time.timeScale = 1f;
        exitPanel.SetActive(false);
    }
}