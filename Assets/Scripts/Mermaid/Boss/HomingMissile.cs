using UnityEngine;

public class HomingMissile : MonoBehaviour
{/// <summary> - 유진
/// 챗지피티씀 그래서 자세한 동작법은 모르니 속성만 주석을 달겠습니다
/// 관성줄이는 법
/// -z회전 고정
/// -rigidbody의 항력 크게하기
/// -질량 낮추기
/// </summary>
    public float speed = 17.5f;            // 미사일 기본 속도
    public float rotationSpeed = 0.5f;    // 회전 속도
    public float maxSpeed = 20f;        // 최대 속도
    public float homingRange = 20.6f;     // 유도 범위
    public Transform player;            // 플레이어의 Transform

    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Homing();
    }

    void Homing()
    {
        if (player == null)
            return;

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 일정 범위 내에서만 유도
        if (distanceToPlayer < homingRange)
        {
            // 플레이어 방향으로 회전
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 플레이어 방향으로 이동
            Vector2 moveDirection = direction.normalized;

            // 최대 속도로 제한
            Vector2 newVelocity = rigid.velocity + moveDirection * (speed * Time.deltaTime);
            if (newVelocity.magnitude > maxSpeed)
            {
                newVelocity = newVelocity.normalized * maxSpeed;
            }

            rigid.velocity = newVelocity;
        }
    }
}
