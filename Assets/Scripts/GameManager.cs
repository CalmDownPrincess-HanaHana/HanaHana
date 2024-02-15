using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            isGameover = true;
            death_count = SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("death") + 1;
            SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("death", death_count);
            death_text.text = "Death : " + death_count++;
            StartCoroutine(OnPlayerFinish());
        }
    }

    IEnumerator OnPlayerFinish() {
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
