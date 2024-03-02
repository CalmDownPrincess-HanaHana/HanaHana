using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfJump : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField]float jumpingSpeed = 5f;    
    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        InvokeRepeating("RandomJump", 1.0f, 1.0f); // 매 초마다 RandomJump 메서드를 호출
    }

    private void RandomJump()
    {
        float randomTime = Random.Range(1f, 3.0f);
        Jump();
    }
    
    private void Jump()
    {
        if (audioSource != null) {
            audioSource.Play();
        }
        Vector3 force = Vector3.up * jumpingSpeed;
        rigid.AddForce(force, ForceMode2D.Impulse);
    }
}
