using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowingWind : MonoBehaviour
{
    [SerializeField]
    private float windPower = 0.5f;

    [SerializeField]
    private float forceAngle = 0f; // 0f 면 오른쪽. 위로 힘주고 싶으면 90을 넣으면 됨 

    private Rigidbody2D objRigidbody;
    private void OnTriggerStay2D(Collider2D col)
    {
        Vector2 forceDirection = Quaternion.Euler(0, 0, forceAngle) * Vector2.right;
        if(col.gameObject.GetComponent<Rigidbody2D>() != null){
            objRigidbody = col.gameObject.GetComponent<Rigidbody2D>();
        }
        objRigidbody.velocity = Vector2.zero;
        objRigidbody.AddForce(forceDirection * windPower * 100); // 범위 내 obj 위로 올려보내기.
    }
}
