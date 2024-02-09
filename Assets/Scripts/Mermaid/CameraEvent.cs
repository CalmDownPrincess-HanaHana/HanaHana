using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CameraEvent : MonoBehaviour
{
    private Camera mainCamera;
    private CameraController mainCameraController;
    private bool isStartEvent = false;
    private LineRenderer lineRenderer; // lineRenderer 변수 추가

    [SerializeField] private float moveSpeed = 1f;
    private Vector3[] path;
    private int currentTargetIndex = 0;

    void Start()
    {
        // 메인 카메라 찾기
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        mainCameraController = Camera.main.GetComponent<CameraController>();
         // 라인 렌더러의 경로를 가져옴
        path = new Vector3[lineRenderer.positionCount];
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            // 선의 꼭짓점 가져옴
            path[i] = lineRenderer.GetPosition(i);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌한 경우
            mainCameraController.ChangeCameraState(Define.CameraState.None);
            isStartEvent = true;
             // 시작점으로 카메라 이동
            currentTargetIndex = 0;
            mainCamera.transform.position = path[currentTargetIndex];
        }
    }
    void Update()
    {
         if (isStartEvent)
        {
            // 타겟 꼭짓점과의 거리 측정
            float distanceToTarget = Vector3.Distance(mainCamera.transform.position, path[currentTargetIndex]);

            // 타겟 꼭짓점에 도달했을 때
            if (distanceToTarget <= 0.1f)
            {
                // 마지막 꼭짓점에 도달하면 종료
                if (currentTargetIndex == path.Length - 1)
                {
                    isStartEvent = false;
                    return;
                }

                // 다음 꼭짓점으로 이동
                currentTargetIndex++;
            }

            // 다음 꼭짓점으로 카메라 이동
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, path[currentTargetIndex], moveSpeed * Time.deltaTime);
        }
    }   

}
