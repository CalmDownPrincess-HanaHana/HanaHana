using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabJump : MonoBehaviour
{
    Rigidbody2D rigid;
    public float jumpSpeed=-10f;
    private void Update()
    {
        this.rigid.velocity=new Vector2(0,this.rigid.velocity.y);
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigid.AddForce(new Vector2(this.rigid.velocity.x, jumpSpeed), ForceMode2D.Impulse);
        }
    }
}
