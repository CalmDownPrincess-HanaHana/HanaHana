using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombDie : MonoBehaviour
{
    /// <summary>
    /// -폭탄 폭파
    /// 시간이 2초 지나거나 외부와 collide 되면
    /// 스프라이트 발사
    /// 체크 해제 후 비활성화
    /// 
    /// -스프라이트 발사
    /// </summary>

    [SerializeField] private float explosionTime = 2f; // 폭파되는 시간
    [SerializeField] private GameObject objPrefab; // 발사할 프리팹
    
    private bool isExploding = false; // 폭파 중인지 여부를 나타내는 변수

    void Start()
    {
        StartCoroutine(CountDelay(explosionTime)); // 일정 시간 후에 폭파하는 코루틴 시작
    }

    void OnCollisionEnter(Collision collision)// 외부와 충돌하면 폭파
    {
        Debug.Log("asdf");
        if (!isExploding)
        {
            StartCoroutine(ExplodeAfter());
        }
    }

    private IEnumerator CountDelay(float delay)
    {
         yield return new WaitForSeconds(delay); // 지정된 딜레이 후에 폭파
         StartCoroutine(ExplodeAfter());
    }

    private IEnumerator ExplodeAfter()
    {   
        if (!isExploding)
        {
            isExploding = true; // 폭파 중임을 표시     
            //yield return new WaitForSeconds(0.1f); // 충돌 후 0.1초 대기 후에 폭파
            //alert 넣기
            GameObject objInstance = Instantiate(objPrefab, transform.position, Quaternion.identity);
            gameObject.SetActive(false); // 오브젝트 비활성화
        }
        yield return null;
    }

    private IEnumerator ShootSprites()
    {
        isExploding = true; // 폭파 중임을 표시
        // 스프라이트 프리팹을 생성합니다.
        Instantiate(objPrefab, transform.position, Quaternion.identity);
        yield return null;
    }
}
