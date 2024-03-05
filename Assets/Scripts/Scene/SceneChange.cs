using System;
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
    public GameObject GameManager;
    //이어하기
    public void Change()
    {
        if (SceneManager.GetActiveScene().name == Define.Scene.StageScene.ToString())
        {
            if (GameObject.Find("popup2ForPC") !=null&& GameObject.Find("popup2ForPC").activeSelf)
            {
                //머메이드에서 이어하기
                SceneManager.LoadScene(Define.Scene.MerMaid.ToString());

            }
            else if (GameObject.Find("popup") != null && GameObject.Find("popup").activeSelf)
            {
                //스노우화이트에서 이어하기

                SceneManager.LoadScene(Define.Scene.SnowWhite.ToString());
            }
        }
        //메인씬: 스노우화이트부터시작
        if (SceneManager.GetActiveScene().name == Define.Scene.MainScene.ToString())
        {
            SceneManager.LoadScene(Define.Scene.SnowWhite.ToString());
        }
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

        //스테이지씬에서 인어공주 새로시작 || MerMaidBossClear씬에서 태초로 돌아감
        if ((GameObject.Find("popup2ForPC") != null && GameObject.Find("popup2ForPC").activeSelf)||SceneManager.GetActiveScene().name==Define.Scene.MerMaidBossClear.ToString())
        {
            //인어공주의 세이브포인트, death, RealItem정보 지우기
            PlayerPrefs.DeleteKey("RealItem" + Define.Scene.MerMaid.ToString());
            PlayerPrefs.DeleteKey("mermaid_respawnX");
            PlayerPrefs.DeleteKey("mermaid_respawnY");
            PlayerPrefs.DeleteKey("mermaid_respawnZ");
            PlayerPrefs.DeleteKey("mermaid_death");
        }
        //스테이지씬에서 백설공주 새로시작 || SnowBossClear씬에서 태초로 돌아감
        if ((GameObject.Find("popup") != null && GameObject.Find("popup").activeSelf) || SceneManager.GetActiveScene().name == Define.Scene.SnowBossClear.ToString())
        {
            //백설공주의 세이브포인트, death, RealItem정보 지우기
            PlayerPrefs.DeleteKey("RealItem"+Define.Scene.SnowWhite.ToString());
            PlayerPrefs.DeleteKey("respawnX");
            PlayerPrefs.DeleteKey("respawnY");
            PlayerPrefs.DeleteKey("respawnZ");
            PlayerPrefs.DeleteKey("death");
        }
        //스테이지씬, 메인씬이면 씬 이동
        if (SceneManager.GetActiveScene().name == Define.Scene.StageScene.ToString()|| SceneManager.GetActiveScene().name == Define.Scene.MainScene.ToString())
        {
            Change();
        }

        Time.timeScale = 1f; //시간 다시 흐르게
    }

    public void back_Home()
    {
        Destroy(GameManager);
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