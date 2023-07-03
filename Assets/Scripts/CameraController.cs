using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Define.CameraState camera_state;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 deltaPos; 
    void Awake()
    {
        Init();
    }

    void Init()
    {
        camera_state = Define.CameraState.Player;
        player = GameObject.FindWithTag("Player").transform;
        deltaPos = new Vector3(0f, 0f, -1f);
    }
    
    //TODO: 만약 플레이어가 죽게되면 이벤트로 정보받아와서 플레이어와의 연결을 끊는 함수 추가

    void LateUpdate()
    {
        // 카메라의 position은 플레이어의 position에 델타값을 더한 값
        transform.position = player.transform.position + deltaPos;
        transform.LookAt(player.transform);
    }
}
