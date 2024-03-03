using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectBlink : MonoBehaviour
{
    //자식이 일정한 시간 동안 활성화 됐다, 안 됐다 하기.
    [SerializeField] private float interval = 10f; //깜빡이는 간격
    [SerializeField] private float startAfter = 0f; //깜빡이는 시작
    private List<Transform> children = new List<Transform>(); // 자식 오브젝트를 저장할 리스트
    private bool isOnce = false;

    void Start()
    {
        // 모든 자식 오브젝트를 리스트에 추가
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (!isOnce) {
            isOnce = true;
            StartCoroutine(enableBlink(interval, startAfter)); // sec초마다 폭탄 생성하는 코루틴 시작
        }
        
    }

    private IEnumerator enableBlink(float interval,float startAfter) 
    {
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false); // 자식 오브젝트 활성화
        }
        yield return new WaitForSeconds(startAfter);
        while (true)
        {
            // 생김
            foreach (Transform child in children)
            {
                child.gameObject.SetActive(true); // 자식 오브젝트 활성화
            }
            yield return new WaitForSeconds(interval);

            //사라짐
            foreach (Transform child in children) {
                child.gameObject.SetActive(false); // 자식 오브젝트 활성화
            }
            yield return new WaitForSeconds(interval);
        }

    }
}

