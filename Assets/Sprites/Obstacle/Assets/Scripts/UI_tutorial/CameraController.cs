using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Define.CameraState cameraState;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 delta; 
    void Awake()
    {
        Init();
    }

    void Init()
    {
        cameraState = Define.CameraState.Player;
        player = GameObject.FindWithTag("Player").transform;
        delta = new Vector3(0f, 0f, 5f);
    }

    void LateUpdate()
    {
        // 카메라의 position은 플레이어의 position에 델타값을 더한 값
        transform.position = player.transform.position + delta;
        transform.LookAt(player.transform);
    }
}
