using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomAppear : MonoBehaviour
{
    [SerializeField] private float sec = 2f;

    private List<Transform> children = new List<Transform>(); // 자식 오브젝트를 저장할 리스트

    private void Start()
    {
        // 모든 자식 오브젝트를 리스트에 추가
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        StartCoroutine(SpawnBombRepeatedly(sec)); // sec초마다 폭탄 생성하는 코루틴 시작
    }

    private IEnumerator SpawnBombRepeatedly(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); // 일정 시간 대기 후에 폭탄 생성

            ActivateRandomChild(); // 자식 오브젝트 활성화
            yield return new WaitForSeconds(0.1f); // 0.1초 대기 후에 다시 자식 오브젝트 활성화
            ActivateRandomChild(); // 자식 오브젝트 활성화
        }
    }

    private void ActivateRandomChild()
    {
        Transform randomChild = GetRandomChild(); // 랜덤한 자식 오브젝트 가져오기
        if (randomChild != null)
        {
            randomChild.gameObject.SetActive(true); // 자식 오브젝트 활성화
            children.Remove(randomChild); // 리스트에서 활성화된 자식 오브젝트 제거
        }
    }

    private Transform GetRandomChild()
    {
        if (children.Count == 0)
        {
            Debug.LogWarning("No more child objects to activate!");
            return null;
        }

        // 랜덤한 자식 오브젝트 반환
        return children[Random.Range(0, children.Count)];
    }
}
