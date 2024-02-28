using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePlayerSlow : MonoBehaviour
{
    private GameObject playerObject;
    private Player playerScript;

    private int colNum = 0; //부딪힌 횟수.

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<Player>();
    }

    /// <summary>
    /// 부딪히면 약 3초간 느려진다. 원본 maxspeed = 5f; isSlowed = false; 인데 1f  랑 true로 3초간 바꿔줌. 
    /// </summary>
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("SlowingObj")){
            colNum++;
            StartCoroutine(SlowDownForSeconds(3f));
        }
    }
    
    IEnumerator SlowDownForSeconds(float seconds)
    {
        playerScript.isSlowed = true; // 느려진 상태로 변경
        float slowedSpeed = 1f; // 느려진 속도

        playerScript.max_speed = slowedSpeed;

        yield return new WaitForSeconds(seconds);
        colNum--;

        // 일정 시간이 지난 후 colNum 이 0 이면, 즉 부딪힌 만큼 기다리면
        if(colNum == 0){
            playerScript.isSlowed = false; // 느려진 상태 해제
            playerScript.max_speed  = 5f;
        }
    }
}
