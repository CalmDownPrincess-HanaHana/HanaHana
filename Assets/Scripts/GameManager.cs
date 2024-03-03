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
    public static GameManager instance;

    [SerializeField] private Vector3 respawnPoint = new Vector3(-9.16f, -0.48f, 0f);
    private int death_count = 0;
    public bool isGameover = false;
    public TextMeshProUGUI death_text;
    public GameObject gameoverUI;
    public GameObject player;
    public GameObject SaveLoad;
    public GameObject exitPanel;

    void Awake()
    {
        if (instance == null)
        {
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

        if (SceneManager.GetActiveScene().name == Define.Scene.SnowWhite.ToString())
        {
            if (SaveLoad.GetComponent<SaveLoad>().LoadRespawn("respawn") != Vector3.zero)
            {
                player.transform.position = SaveLoad.GetComponent<SaveLoad>().LoadRespawn("respawn");
            }
        }
        else if (SceneManager.GetActiveScene().name == Define.Scene.MerMaid.ToString())
        {
            if (SaveLoad.GetComponent<SaveLoad>().LoadRespawn("mermaid_respawn") != Vector3.zero)
            {
                player.transform.position = SaveLoad.GetComponent<SaveLoad>().LoadRespawn("mermaid_respawn");
            }
        }

        player.GetComponent<Player>().ChangeSprites();
    }

    void Update()
    {
        if (isGameover && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            StartCoroutine(OnPlayerRestart());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            exitPanel.SetActive(true);
        }
    }

    public void OnPlayerDead()
    {
        if (!isGameover)
        {
            //현재 상태를 게임오버 상태로 변경
            isGameover = true;
            //죽은 횟수를 증가

            if (SceneManager.GetActiveScene().name == Define.Scene.SnowWhite.ToString()|| SceneManager.GetActiveScene().name == "SnowBoss4" || SceneManager.GetActiveScene().name == "SnowBoss3" || SceneManager.GetActiveScene().name == "SnowBoss2" || SceneManager.GetActiveScene().name == "SnowBoss1")
            {
                death_count = SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("death") + 1;

                SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("death", death_count);
                Debug.Log("snowwhite!" + SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("death").ToString());
                Debug.Log("smermaid!" + SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("mermaid_death").ToString());
                death_text.text = "Death : " + death_count++;
            }
            else if (SceneManager.GetActiveScene().name == Define.Scene.MerMaid.ToString()|| SceneManager.GetActiveScene().name == Define.Scene.MerMaidBoss.ToString())
            {
                death_count = SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("mermaid_death") + 1;

                SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("mermaid_death", death_count);
                Debug.Log("mermaid!" + SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("mermaid_death").ToString());
                Debug.Log("msnowwhite!" + SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("death").ToString());
                death_text.text = "Death : " + death_count++;
            }


            StartCoroutine(OnPlayerFinish());
        }
    }

    IEnumerator OnPlayerFinish()
    {
        yield return new WaitForSeconds(0.5f);
        gameoverUI.SetActive(true);
    }

    IEnumerator OnPlayerRestart()
    {
        yield return new WaitForSeconds(0.5f);
        if (SceneManager.GetActiveScene().name == "SnowBoss4" || SceneManager.GetActiveScene().name == "SnowBoss3" || SceneManager.GetActiveScene().name == "SnowBoss2" || SceneManager.GetActiveScene().name == "SnowBoss1")
        {
            SceneManager.LoadScene("SnowWhite");
        }
        else if (SceneManager.GetActiveScene().name == Define.Scene.MerMaidBoss.ToString())
        {
            SceneManager.LoadScene(Define.Scene.MerMaid.ToString());
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
