using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//얘도 주인공 따라다니는데 follower랑은 살짝 다름.  follower는 player가 간 길을 그대로 따라가고 얘는 말그대로 player가 목표
public class Missile : MonoBehaviour
{
    public float waitingTime = 0f;
    private bool hasStarted = false;
    public float speed = 5f;
    public float inertiaStrength = 1.5f; // ���� ���� ����
    public Transform player;
    private Rigidbody2D rigid;

    [SerializeField]
    private Vector3 targetPos = new Vector3(0, 0, 0);

    //이거는 유진님이 만든거라 잘 모르겠습니다....
    [SerializeField]
    private bool has_rigid = false; //리지드바디 있으면 관성받게 하려는 체크박스
    [SerializeField]
    private bool is_prefab = false; //프리팹이면 생성될때마다 플레이어 받아오는 연결 끊어지므로 받아옴
    [SerializeField]
    private bool follow_once = false; //처음 위치를 기억하고 그 방향으로 일직선으로 발사됨
    private void Awake()
    {
        StartCoroutine(Wait());
        if (is_prefab)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        if (has_rigid)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        if (has_rigid)
        {
            inertiaMissle();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitingTime);

        hasStarted = true;
    }

    void Update()
    {
        if (!follow_once && hasStarted)
        {
            if (player.position != null)
            {
                Vector3 direction = (player.position + targetPos) - transform.position;
                direction.Normalize();
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    private void inertiaMissle()
    {
        if (player == null)
            return;

        Vector2 toPlayer = player.position - transform.position;
        Vector2 desiredDirection = toPlayer.normalized;

        // ���� �ӵ��� ������ �����Ͽ� �̵� ���� ����
        Vector2 newVelocity = rigid.velocity + desiredDirection * (speed * Time.fixedDeltaTime);
        newVelocity *= inertiaStrength;

        rigid.velocity = newVelocity;
    }

}
