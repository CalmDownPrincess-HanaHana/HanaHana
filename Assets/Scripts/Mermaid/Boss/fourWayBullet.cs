using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fourWayBullet : MonoBehaviour
{
    /// <summary>
    /// 해당 프리팹을 4방면으로 쏘는 스크립트
    /// </summary>
    /// 
    [SerializeField] private GameObject objPrefab; // 발사할 프리팹
    [SerializeField] private float force = 10f; // 발사되는 힘
    [SerializeField] private float spewDuration = 1f; // 발사 후 파괴될 때까지의 지연 시간

    private void Start() {
        StartCoroutine(SpewOnce()); // sec초마다 폭탄 생성하는 코루틴 시작
    }
    IEnumerator SpewOnce()
    {
        Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

        foreach (Vector3 direction in directions)
        {
            GameObject objInstance = Instantiate(objPrefab, transform.position, Quaternion.identity);

            Rigidbody2D objRigidbody = objInstance.GetComponent<Rigidbody2D>();

            if (objRigidbody != null)
            {
                objRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
            }
            yield return null; // 다음 스프라이트를 생성하기 전에 한 프레임을 기다립니다.
        }
    }


}
