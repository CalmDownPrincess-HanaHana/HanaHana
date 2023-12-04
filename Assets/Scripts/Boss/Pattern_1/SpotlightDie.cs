using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightDie : MonoBehaviour
{
    public GameObject Spotlight;
    public GameObject Blackout;
    [SerializeField] private int n = 3;
    [SerializeField] private Player player;
    [SerializeField] private float blinkSec = 3.0f;
    private bool isCanDie = false;

    void Start()
    {
        List<Transform> randomChildren = GetRandomChildren(Spotlight.transform, n);
        controlSpotlight(randomChildren, blinkSec);
    }

    //랜덤한 child N개를 뽑는다.
    #region GenerateRandomList
    List<Transform> GetRandomChildren(Transform parentTransform, int n)
    {//n개의 child를 선택해 반환한다.
        List<Transform> randomChildrenList = new List<Transform>();

        if (parentTransform.childCount < n)
        {
            Debug.LogWarning("Not enough children under the specified parent.");
            return randomChildrenList;
        }

        int previousIndex = -1; // 이전에 선택한 index 초기화

        for (int i = 0; i < n; i++)
        {
            int randomIndex = GetUniqueRandomIndex(parentTransform.childCount, previousIndex);

            Transform selectedChild = parentTransform.GetChild(randomIndex);

            // 중복된 index를 방지하기 위해 이전 index 업데이트
            previousIndex = randomIndex;
            // 리스트에 추가
            randomChildrenList.Add(selectedChild);
        }

        return randomChildrenList;
    }

    //n개의 child를 선택해 반환한다.
    int GetUniqueRandomIndex(int maxIndex, int previousIndex)
    {
        int randomIndex = Random.Range(0, maxIndex);

        // 이전 index와 중복되지 않도록 확인
        while (randomIndex == previousIndex)
        {
            randomIndex = Random.Range(0, maxIndex);
        }

        return randomIndex;
    }
    #endregion

    //각 child에 대해 기믹을 실행한다.
    #region ControlSpotlight
    void controlSpotlight(List<Transform> randomChildren, float blinkSec)
    {
        StartCoroutine(SequentialBlink(randomChildren, blinkSec));
    }

    IEnumerator SequentialBlink(List<Transform> children, float blinkSec)
    {
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
            AlertBlink alertBlink = child.GetComponent<AlertBlink>();

            if (alertBlink != null)
            {
                yield return StartCoroutine(BlinkChild(child, alertBlink, blinkSec));
            }
        }
    }

    IEnumerator BlinkChild(Transform child, AlertBlink alertBlink, float duration)
    {
        alertBlink.enabled = true;
        yield return new WaitForSeconds(duration);
        alertBlink.enabled = false;

        isCanDie = true;
        Blackout.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        isCanDie = false;
        Blackout.SetActive(false);
        child.gameObject.SetActive(false);
    }
    #endregion

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isCanDie && other.transform.CompareTag("Player"))
        {
            player.GetComponent<Player>().Die(transform.position);
            isCanDie = false;
        }
    }
}