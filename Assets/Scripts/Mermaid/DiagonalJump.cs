using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalJump : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] float jumpingSpeed = 5f;
    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 대각선으로 두 번 점프하기
            Jump();
            Invoke("Jump", 0.5f); // 첫 번째 점프 후 0.5초 후에 두 번째 점프
        }
    }

    private void Jump()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        // 대각선으로 점프하기 위해 x와 y 방향으로 힘을 가함
        Vector2 force = new Vector2(jumpingSpeed, jumpingSpeed);
        rigid.AddForce(force, ForceMode2D.Impulse);
    }
}
