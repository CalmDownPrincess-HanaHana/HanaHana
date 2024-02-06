using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장애물 사운드 스크립트
/// ObstacleController와 비슷한 작동방식
/// </summary>
public class ObstacleSound : MonoBehaviour
{
    //콜라이더로 감지하는가?
    [SerializeField]
    private bool isCol = false;
    //오디오넣기
    private AudioSource audio;
    //오디오 작동시키기 위한 조건변수
    [SerializeField]
    private bool isMoving = false;
    //오디오 작동시키기까지 기다리는 시간
    [SerializeField]
    private float waitingTime = 0f;
    //초기세팅 사운드 값
    float settingSoundValue;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어한테 닿으면 조건변수 킴
        if (isCol && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SetIsmoving(true));
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //트리거 닿으면 조건변수 킴
        if ((!isCol) && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SetIsmoving(true));
        }
    }

    private void Update()
    {
        //플레이어가 사운드 값 바꿨을 시 반영되도록
        if (settingSoundValue != PlayerPrefs.GetFloat("effectValue"))
        {
            settingSoundValue = PlayerPrefs.GetFloat("effectValue");
            audio.volume = settingSoundValue;
        }
        //오디오 들어있고 조건변수 켜지면
        if (audio != null && isMoving)
        {
            //오디오가 반복재생 체크 안 되어있으면
            if (!audio.loop)
            {
                //재생
                audio.Play();
                //조건변수 꺼줘서 또 무한재생 안되도록
                isMoving = false;
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //오디오소스 컴포넌트 가져오기
        audio = GetComponent<AudioSource>();
        //초기세팅 효과음값 가져오기
        settingSoundValue = PlayerPrefs.GetFloat("effectValue");
        //초기세팅값적용
        audio.volume = settingSoundValue;
    }

    //보이면 활성화 
    private void OnBecameVisible()
    {
        if (audio != null)
        {
            audio.enabled = true;
        }
    }
    //안보이면 비활성화
    private void OnBecameInvisible()
    {
        if (audio != null)
        {
            audio.enabled = false;
        }
    }

    IEnumerator SetIsmoving(bool n)
    {
        yield return new WaitForSeconds(waitingTime);
        this.isMoving = n;
    }
}
