using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerTouchTest : MonoBehaviour
{
    float rightButtonEnd= Screen.width * 0.4167f;
    float leftButtonEnd= Screen.width * 0.2083f;
    float jumpButtonEnd= Screen.width;
    bool isJumping = false;
    Rigidbody2D rigid;
    [SerializeField] private float maxSpeed=5f;
    [SerializeField] private float jumpPower=5f;
    // Start is called before the first frame update
    void Start()
    {
        rigid=GetComponent<Rigidbody2D>();  
        Debug.Log("leftButton�� 0~" + leftButtonEnd);
        Debug.Log("rightButton��"+leftButtonEnd+"~" + rightButtonEnd);
        Debug.Log("jumpButton��"+rightButtonEnd+"~" + jumpButtonEnd);
        
    }

    // Update is called once per frame
    void Update()
    {

        // ���� �߻� ���� ��� ��ġ ���� ��������
        Touch[] touches = Input.touches;
        // �Ʒ��� �� ��ġ�� ���� ó��
        //1. �극��ũ
        if (Input.touchCount== 1|| Input.touchCount == 0) //�հ����� 1���ų� �������
        {
            if (Input.touchCount == 1)//�հ����� �ִ� ����
            {
                Touch touch=Input.GetTouch(0);
                //�¿�Ű ������ ���� �� ��
                if (!(touch.position.x >= 0 && touch.position.x < rightButtonEnd))
                {
                    //�� �� �극��ũ �ɾ��ֱ�
                    rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0000001f, rigid.velocity.y);
                }
            }
            else if (Input.touchCount == 0)
            {
                //�� �� �극��ũ �ɾ��ֱ�
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0000001f, rigid.velocity.y);
            }
        } 
        //2. �¿� �� ����Ű
        foreach (Touch touch in touches)
        {
            if(touch.position.x>=0&& touch.position.x < leftButtonEnd)
            {
                rigid.velocity = new Vector2(maxSpeed * -1, rigid.velocity.y);
            }
            if (touch.position.x >= leftButtonEnd && touch.position.x < rightButtonEnd)
            {
                rigid.velocity = new Vector2(maxSpeed * 1, rigid.velocity.y);
            }
            if (touch.position.x >= Screen.width*0.5f && touch.position.x < jumpButtonEnd)
            {
                //�׸��� �������϶� �� �����������ϰ� �ؾ���
                if (!isJumping)
                {
                    isJumping = true;
                    rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                }
            }
        }
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = false;
        }
    }
}
