using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맨처음 fakeitem 먹었을 때, tutorial 진행을 위한 스크립트.
/// playerprefs 저장: 한곳에서 모아놓으면 안 되나?
/// 저 요상한 deathcount는 뭐꼬. 없어도 될 것 같은데?
/// </summary>
public class TutorialFakeItem : MonoBehaviour
{
    public GameObject SaveLoad;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("tutorial") == 1)
        {
            SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 2);
        }
        PlayerPrefs.SetString("RealItem", "TutorialFakeItem");
    }
    
    // trigger 처리된 박스 부딪히면 missile 발동.
    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.GetComponent<Missile>().enabled = true;
    }
}