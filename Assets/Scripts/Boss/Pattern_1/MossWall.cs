using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MossWall : MonoBehaviour
{
    public float slidingSpeed = 0.5f; // 미끄러질 속도 조절
    public AudioSource audioSource;

    [SerializeField] float jumpingSpeed = 12f;
    private bool isSliding = false;
    private bool hasAppliedForce = false;
    private bool autoExit = false;
    private GameObject playerOnWall;
    private Player playerScript;
    private Rigidbody2D playerRigidbody;

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
            StartCoroutine(SlidePlayerDown(collision.gameObject.transform));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            autoExit = true;
            ExitMossWall();
        }
    }

    private IEnumerator SlidePlayerDown(Transform playerTransform)
    {
        hasAppliedForce = false;
        isSliding = true;

        // X축으로 고정
        playerRigidbody = playerTransform.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

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
            ExitMossWall();
            }
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
        if (playerRigidbody != null && !hasAppliedForce)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (!autoExit)
            {
                if (playerOnWall.GetComponent<SpriteRenderer>().flipX)
                {
                    // 왼쪽으로 점프
                    Vector3 force = new Vector3(-100, jumpingSpeed, 0);
                    playerRigidbody.AddForce(force, ForceMode2D.Impulse);
                }
                else
                {
                    // 오른쪽으로 점프
                    Vector3 force = new Vector3(100, jumpingSpeed, 0);
                    playerRigidbody.AddForce(force, ForceMode2D.Impulse);
                }
            }
            autoExit = false;
            hasAppliedForce = true; // Set the flag to true after applying force
        }
    }
}
