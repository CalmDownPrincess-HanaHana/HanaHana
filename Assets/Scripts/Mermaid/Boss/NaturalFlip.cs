using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalFlip : MonoBehaviour
{
    private Vector3 previousPosition;
    SpriteRenderer renderer;
    void Start()
    {
        previousPosition = transform.position;
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 현재 위치와 이전 위치의 차이를 구하여 이동 방향을 추정
        Vector3 currentPosition = transform.position;
        Vector3 direction = currentPosition - previousPosition;
        previousPosition = currentPosition;

        // 이동 방향이 0인 경우 (정지 상태) 처리
        if (direction != Vector3.zero)
        {
            // 방향을 정규화하여 단위 벡터로 만듦
            direction.Normalize();
            if (direction.x < 0)
            {
                renderer.flipX = true;
            }
            if (direction.x > 0)
            {
                renderer.flipX = false;
            }
        }
    }
}
