using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            // "Player" 태그를 가진 오브젝트와 충돌했을 때의 로직

            // 충돌한 오브젝트의 속도를 2f만큼 감소시킵니다.
            Rigidbody2D otherRigidbody = other.GetComponent<Rigidbody2D>();
            if (otherRigidbody != null)
            {
                // 현재 속도를 가져옵니다.
                Vector2 currentVelocity = otherRigidbody.velocity;

                // 속도를 y축 방향으로 2f만큼 감소시킵니다.
                currentVelocity.y -= 2f;

                // 새로운 속도를 적용합니다.
                otherRigidbody.velocity = currentVelocity;
            }

            // 충돌한 오브젝트의 위치를 현재 오브젝트(Turtle)와 동일하게 설정합니다.
            Vector3 newPosition = other.transform.position;
            newPosition.y -= 0.5f;
            this.transform.position = newPosition;

            // 충돌한 오브젝트를 부모로 설정합니다.
            this.transform.SetParent(other.transform);
        }
        else if(other.CompareTag("Alert"))
        {
            // "Alert" 태그를 가진 오브젝트와 충돌했을 때의 로직

            // 현재 오브젝트를 파괴합니다.
            Destroy(gameObject);
        }
    }
}
