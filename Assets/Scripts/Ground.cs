using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private Player player_script;
    private Rigidbody2D player_rigid;
    private void Awake()
    {
        player_script = GameObject.FindWithTag("Player").GetComponent<Player>();
        player_rigid=GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();  
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� �ݶ��̴��� �����ɴϴ�.
        Collider2D otherCollider = collision.collider;

        // �Ӹ��� ��Ҵ°�?
        if (!(otherCollider is CapsuleCollider2D) && otherCollider is CircleCollider2D && collision.gameObject.CompareTag("Player"))
        {
            //���� ����
            player_script.ignore_jump= true;
        }
        //�ٸ��� ������� ignore_jump =false
        if (otherCollider is CapsuleCollider2D && collision.gameObject.CompareTag("Player"))
        {
            //���� Ǯ���ֱ�
            player_script.ignore_jump = false;
        }
    }
}
