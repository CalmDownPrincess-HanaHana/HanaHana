using UnityEngine;

public enum LaserDirection { Left, Right, Up, Down }

public class FourWayLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float maxDistance = 20f; // 레이저의 최대 길이
    public LayerMask obstacleMask; // 레이저가 충돌을 검사할 레이어 마스크
    public LaserDirection laserDirection = LaserDirection.Left; // 레이저의 방향

    void Update()
    {
        // 현재 게임 오브젝트의 위치를 시작점으로 설정
        Vector3 startPos = transform.position;

        // 선택한 방향으로 레이저 그리기
        Vector3 direction = Vector3.zero;
        switch (laserDirection)
        {
            case LaserDirection.Left:
                direction = -transform.right;
                break;
            case LaserDirection.Right:
                direction = transform.right;
                break;
            case LaserDirection.Up:
                direction = transform.up;
                break;
            case LaserDirection.Down:
                direction = -transform.up;
                break;
        }
        DrawLaser(startPos, direction);
    }

    void DrawLaser(Vector3 startPos, Vector3 direction)
    {
        // 레이저 발사 방향으로 레이를 쏴서 충돌 검사
        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxDistance, obstacleMask);

        // 레이저의 끝점 설정
        Vector3 endPos = hit.collider != null ? hit.point : startPos + direction * maxDistance;

        // Line Renderer로 레이저 그리기
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        // 충돌한 경우 플레이어가 죽는 함수 호출
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<Player>().Die(hit.collider.transform.position);
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<bombDie>().LaserCollision();
            }
            
        }
    }
}
