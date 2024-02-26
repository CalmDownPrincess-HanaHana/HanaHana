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
    private bool isNew = false;
    //이어하기
    public void Change()
    {
        
        if (SceneManager.GetActiveScene().name == Define.Scene.StageScene.ToString())
        {
            if (GameObject.Find("popup2ForPC") !=null&& GameObject.Find("popup2ForPC").active == true)
            {
                //머메이드에서 이어하기
                SceneManager.LoadScene(Define.Scene.MerMaid.ToString());

            }
            else if (GameObject.Find("popup") != null && GameObject.Find("popup").active == true)
            {
                //스노우화이트에서 이어하기

                SceneManager.LoadScene(Define.Scene.SnowWhite.ToString());
                

            }
        }
        else
        {
            SceneManager.LoadScene("SnowWhite");
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
        //스테이지씬에서 눌렀을 때
        if (SceneManager.GetActiveScene().name == Define.Scene.StageScene.ToString()|| SceneManager.GetActiveScene().name == Define.Scene.SnowBossClear.ToString())
        {

            //스노우화이트씬에서 새로하기 시: Mermaid세이브포인트 / death 살리기
            float mermaidRespawnX; float mermaidRespawnY; float mermaidRespawnZ;
            //Mermaid씬에서 새로하기 시: SnowWhite세이브포인트/ death 살리기
            float snowRespawnX; float snowRespawnY; float snowRespawnZ;
            if (PlayerPrefs.GetString("SnowWhiteClear") == "true")
            {
             
                //Mermaid클리어정보살리기
                string mermaidClear=PlayerPrefs.GetString("MermaidClear");
                //입은 스킨 살리기
                string cloth = PlayerPrefs.GetString("Cloth");
                //초기화
                PlayerPrefs.DeleteAll();
                //입은 스킨 살리기
                PlayerPrefs.SetString("Cloth", cloth);
                //SnowWhite클리어정보 살리기
                PlayerPrefs.SetString("SnowWhiteClear", "true");
                //Mermaid클리어정보 살리기
                if (mermaidClear== "true")
                {
                    PlayerPrefs.SetString("MermaidClear" ,"true");
                }
                //패턴연습소 살리기
                PlayerPrefs.SetString("SnowWhiteTrainingStage", "true");
                //Mermaid의 세이브포인트 위치 살리기
                //Mermaid의 death 살리기
            }
            else if (PlayerPrefs.GetString("MermaidClear") == "true")
            {
                
                //입은스킨정보살리기
                string cloth = PlayerPrefs.GetString("Cloth");
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetString("Cloth", cloth);
                //클리어데이터살리기
                PlayerPrefs.SetString("MermaidClear", "true");
                PlayerPrefs.SetString("SnowWhiteClear", "true");
                //패턴연습소살리기
                PlayerPrefs.SetString("SnowWhiteTrainingStage", "true");
                //SnowWhite의 세이브포인트 위치 살리기
                //SnowWhite의 death 살리기
            }
            else
            {
                PlayerPrefs.DeleteAll();
            }


        }


        isNew = true;
        //SaveLoad.GetComponent<SaveLoad>().SaveBool("New", isNew);
        Change();
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