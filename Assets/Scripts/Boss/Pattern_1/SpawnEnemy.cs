using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 첫번째 phase에 불덩이 랜덤 위치에 spawn 하는 함수
/// 변수 값 좀 줄여라
/// </summary>
public class SpawnEnemy : MonoBehaviour
{
    public bool enableSpawn = false;
    public GameObject Enemy; // Prefab을 받을 public 변수 입니다.
    public GameObject player;
    [SerializeField] private float second = 5f;
    [SerializeField] private float delay = 1.5f;
    [SerializeField] private bool isPanel = false;
    [SerializeField] private float randomX = 0f;
    [SerializeField] private float randomY = 0f;
    [SerializeField] private float theend = 20f;

    void Start()
    {
        InvokeRepeating("EnemySpawn", second, delay); // 3초후 부터, SpawnEnemy 함수를 1초마다 반복해서 실행 시킵니다.
        Invoke("CancelInvokeLog", theend);
        // 일정 시간 후에 enableSpawn 변수를 끄는 코드
    }

    void Update()
    {
        if (!player.activeSelf)
        {
            CancelInvoke("EnemySpawn");
        }
    }
    void EnemySpawn()
    {
        if (!isPanel)
        {
            randomX = Random.Range(9.5f, 23f); // 적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
            randomY = Random.Range(5f, 3f);
        }

        if (enableSpawn)
        {
            GameObject enemy = Instantiate(Enemy, new Vector3(randomX, randomY, 0f), Quaternion.identity);
        }
    }
    private void CancelInvokeLog()
    {

        CancelInvoke("EnemySpawn");

    }
}
