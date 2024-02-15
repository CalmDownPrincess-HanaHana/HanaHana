using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맨 처음 flag 만나는 걸 감지하는 함수.
/// TutorialText UI를 끝낸다.
/// </summary>
public class TutorialFlag : MonoBehaviour
{
    public GameObject SaveLoad;
    public GameObject TutorialText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("tutorial") != 4)
            {
                TutorialText.GetComponent<TutorialText>().TutoEnd();//튜토리얼 마지막 text를 출력, 관련 UI를 끄세요.
            }
        }

    }
}
