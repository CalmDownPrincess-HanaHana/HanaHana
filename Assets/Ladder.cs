using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Ladder : MonoBehaviour
{
    public float slidingSpeed = 0.5f; // 미끄러질 속도 조절
    public AudioSource audioSource;

    private bool isSliding = false;
    private bool autoExit = false;
    private GameObject playerOnWall;
    private Player playerScript;
    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;

    /*
    OnCollisionEnter2D -> SlidePlayerDown
    OnTriggerEnter2D -> ExitMossWall
    Update -> GoUp
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnWall = collision.gameObject;
            // player 스크립트 가져오기
            playerScript = playerOnWall.GetComponent<Player>();

            // player 스크립트가 null이 아닌지와 변수가 있는지 확인
            if (playerScript != null)
                --playerScript.isMoss;
            if(audioSource!=null){
                audioSource.Play(); //재생
            }
            playerTransform = collision.gameObject.transform;
            playerRigidbody = playerTransform.GetComponent<Rigidbody2D>();
            isSliding=true;
            StartCoroutine(SlidePlayerDown());
        }
    }

    private IEnumerator SlidePlayerDown()
    {
        if(isSliding){
           // X축으로 고정
        if (playerRigidbody != null&&isSliding)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }}
        // Y축으로 천천히 미끄러짐
        while (isSliding)
        {
            playerTransform.Translate(Vector3.down * slidingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void Update()
    {
        if (playerOnWall && isSliding) {
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)||(playerScript != null&& playerScript.isMoss == 1))
        {
            playerScript.isMoss = 0;
            StartCoroutine(GoUp());
            }
        }
    }

    IEnumerator GoUp(){
        StopCoroutine(SlidePlayerDown());
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        playerTransform.Translate(Vector3.up * slidingSpeed * Time.deltaTime);
        //playerRigidbody.AddForce(Vector3.up*jumpingSpeed);
        yield return new WaitForSeconds(1);
    }


        private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            autoExit = true;
            ExitMossWall();
        }
    }

    private void ExitMossWall()
    {
        if(audioSource!=null){
        audioSource.Stop(); //정지
        }
        // 이끼벽을 떠날 때 실행되는 부분
        isSliding = false;

        // Rigidbody의 제약 조건을 모두 해제
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}