using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    public float yyy=-1.5f;
    public float xxx =-1.5f;
    private AdjustJumppower adjustjumppower;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Alert"))
        {
            // "Alert" 태그를 가진 오브젝트와 충돌했을 때의 로직
            adjustjumppower = this.GetComponent<AdjustJumppower>();
            adjustjumppower.ResetJumpPower();
        }

        else if(other.CompareTag("Player"))
        {
            // "Player" 태그를 가진 오브젝트와 충돌했을 때의 로직

            // 충돌한 오브젝트의 위치를 현재 오브젝트(Turtle)와 동일하게 설정합니다.
            Vector3 newPosition = other.transform.position;
            newPosition.y += yyy;
            newPosition.x += xxx;
            this.transform.position = newPosition;

            // 충돌한 오브젝트를 부모로 설정합니다.
            this.transform.SetParent(other.transform);
        }
    }
}
