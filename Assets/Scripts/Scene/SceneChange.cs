using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
///이어하기, 새로하기, 등 ui button과 scenechange를 적절히 연결
///gamemanager가 timescale 0, scenechange가 scene load 하고 timescale 1로 세팅 
/// </summary>
public class SceneChange : MonoBehaviour
{

    public GameObject SaveLoad;
    private bool isNew = false;
    //이어하기
    public void Change()
    {        
        SceneManager.LoadScene("SnowWhite");
        Time.timeScale = 1f; //시간 다시 흐르게
    }

    //새로하기
    public void new_Change()
    {
        /*if(EditorUtility.DisplayDialog("게임 세이브 정보 삭제", "정말 삭제 하시겠습니까?", "네", "아니오"))*/
        //백설공주 맵 깼을 시 다른데이터는 살려두기. 
        //홈씬에서 눌렀을 때
        if (SceneManager.GetActiveScene().name == Define.Scene.MainScene.ToString())
        {
            PlayerPrefs.DeleteAll();
        }
        //스테이지씬에서 눌렀을 때
        if (SceneManager.GetActiveScene().name == Define.Scene.StageScene.ToString()|| SceneManager.GetActiveScene().name == Define.Scene.SnowBossClear.ToString())
        {
            if (PlayerPrefs.GetString("SnowWhiteClear") == "true")
            {
                string cloth = PlayerPrefs.GetString("SnowWhiteCloth");
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetString("SnowWhiteClear", "true");
                PlayerPrefs.SetString("SnowWhiteCloth", cloth);
            }
            else if (PlayerPrefs.GetString("MermaidClear") == "true")
            {
                string cloth = PlayerPrefs.GetString("SnowWhiteCloth");
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetString("SnowWhiteClear", "true");
                PlayerPrefs.SetString("SnowWhiteCloth", cloth);
            }
            else
            {
                PlayerPrefs.DeleteAll();
            }
        }


        isNew = true;
        //SaveLoad.GetComponent<SaveLoad>().SaveBool("New", isNew);
        SceneManager.LoadScene("SnowWhite");
        Time.timeScale = 1f; //시간 다시 흐르게
    }

    public void back_Home()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Closet_Change()
    {
        SceneManager.LoadScene("ClosetScene");
    }

    public void Stage_Change()
    {
        //isNew = SaveLoad.GetComponent<SaveLoad>().LoadBool("New");
        SceneManager.LoadScene("StageScene");
    }

    public void TurnOffGame()
    {
        Application.Quit(); // 빌드된 게임에서 실행 중인 경우 종료
    }
}