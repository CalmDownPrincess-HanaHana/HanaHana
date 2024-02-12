using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 번개
/// </summary>
public class Lightning : MonoBehaviour
{
    // 아이템을 먹어야 켜짐-> 아이템에 대한 정보 필요
    [SerializeField] private GameObject need_item;
    //번개 정보들(SnowWhite에서는 번개랑 불 오브젝트 가져옴)
    [SerializeField] private GameObject[] lightnings;
    [SerializeField] private bool isCol;
    private Player player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isCol) return;
        //플레이어한테 닿았고 아이템 얻었으면
        if ((other.gameObject.CompareTag("Player")))
        {
            if (need_item.GetComponent<Falling_Umbrella>() != null)
            {
                if (need_item.GetComponent<Falling_Umbrella>().Item_get)
                {
                    foreach (GameObject gameObject in lightnings)
                    {
                        gameObject.SetActive(true);//번개 켜주기
                    }
                    Invoke("PlayerDie", 3f);
                }
            }
            else
            {
                foreach (GameObject gameObject in lightnings)
                {
                    gameObject.SetActive(true);//번개 켜주기
                }
                
            }
        }
    }
    //트리거면 아래의 코드
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCol) return;
        //플레이어한테 닿았고 아이템 얻었으면
        if ((other.gameObject.CompareTag("Player")))
        {
            if (need_item.GetComponent<Falling_Umbrella>() != null)
            {
                if (need_item.GetComponent<Falling_Umbrella>().Item_get)
                {
                    foreach (GameObject gameObject in lightnings)
                    {
                        gameObject.SetActive(true);//번개 켜주기
                    }
                    Invoke("PlayerDie", 1.5f);
                }
            }
            else
            {
                foreach (GameObject gameObject in lightnings)
                {
                    gameObject.SetActive(true);//번개 켜주기
                }
                
            }
        }
    }

    private void PlayerDie()
    {
        player.Die(player.transform.position);       
    }
}
