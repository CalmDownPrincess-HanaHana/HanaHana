using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ParentObstacleController : MonoBehaviour
{
    //obtype은 enum이라 미리 틀 만들기 힘듬. 그냥 필요할때 obtype 사용해서 옵스타클 종류 나누기
    private bool isMoving = false; //이건 test용. 누르면 바로 움직이긴 함. 
    [SerializeField] private bool isMovingFromStart = false;
    [SerializeField] private bool isCol = false;
    [SerializeField] private bool isLoop = false;
    [SerializeField] private float waitingTime = 0f;
    [SerializeField] private Define.Tags colTag = Define.Tags.Player;

    protected GameObject player;

    IEnumerator WaitforGivenTime()
    {
        yield return new WaitForSeconds(waitingTime);//★waiting time설정한 만큼 기다리고
        StartCoroutine(Activate());
    }

    public virtual IEnumerator Activate() //오버라이딩 당할 예정. 근데 만약 오버라이딩 안하면 기본 기능 호출된다. 만약 코루틴 사용할거면 이거 오버라이드 하기.
    {
        isMoving = true;
        yield return null;
    } //여기는 본격적으로 동작하는 코루틴 작성

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCol && collision.transform.CompareTag(colTag.ToString()))
        {
            StartCoroutine(WaitforGivenTime());
            isCol = isLoop;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCol && collision.gameObject.CompareTag(colTag.ToString()))
        {
            StartCoroutine(WaitforGivenTime());//★
            if (this.gameObject.GetComponent<BoxCollider2D>() && !isLoop)//★BoxCollider는 트리거로만 씀. 이거 기억 나시나요? 트리거로만 쓰자고했던거. 이거 까먹을 것 같으니 여기 제대로 써두고 그 이후는 ObstacleController 수정 절대 하지말고 냅둡시다
            {
                Destroy(this.gameObject.GetComponent<BoxCollider2D>());//그래서 트리거 끝나면 destroy..
            }
        }
    }

    // Start is called before the first frame update
    public void Awake()
    {

            if (isMovingFromStart)
            {
                StartCoroutine(WaitforGivenTime());
            }
            player = GameObject.FindWithTag("Player");
        
    }



}
