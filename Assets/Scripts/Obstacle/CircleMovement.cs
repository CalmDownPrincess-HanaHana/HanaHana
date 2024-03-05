using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*성맵에서 빙글빙글 도는 바닥 4개, 플레이어 물리기 바닥의 물리를 이어받음*/
public class CircleMovement : MonoBehaviour
{
    [SerializeField]
    Transform rotationCenter;

    [SerializeField]
    float rotationRadius = 2f, angularSpeed = 2f;

    [SerializeField]
    private float degree;

    [SerializeField]
    private bool isOpposite = false;

    float posX, posY = 0f;

    void Awake()
    {
        degree*=Mathf.Deg2Rad;
    }
    void FixedUpdate()
    {
        posX = rotationCenter.position.x + Mathf.Cos(degree) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(degree) * rotationRadius;

        int direction = (isOpposite)? -1 : 1;

        transform.position = new Vector2(posX, posY);
        degree = degree + Time.deltaTime * angularSpeed*direction;

        if (degree >= 360f*Mathf.Deg2Rad)
        {
            degree = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
