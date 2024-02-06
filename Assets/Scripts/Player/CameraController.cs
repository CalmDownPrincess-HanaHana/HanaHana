using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using static Define;

public class CameraController : MonoBehaviour
{
    public CameraState cameraState;
    [SerializeField] private Transform player;
    private Player player_script;
    [SerializeField] public Vector3 delta;

    public bool isReverse = false;
    private bool preIsReverse; //이전 프레임의 isReverse 저장

    private float zValue = -1f;


    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (SceneManager.GetActiveScene().name == Define.Scene.SnowBoss4.ToString() || SceneManager.GetActiveScene().name == Define.Scene.SnowBoss123.ToString() || SceneManager.GetActiveScene().name == "SnowBoss3" || SceneManager.GetActiveScene().name == "SnowBoss2" || SceneManager.GetActiveScene().name == "SnowBoss1")
        {
            return;
        }
        cameraState = Define.CameraState.Player;
        player = GameObject.FindWithTag("Player").transform;
        player_script = player.GetComponent<Player>();
        preIsReverse = isReverse;
        UpdateDelta();
    }
    public void ChangeCameraState(CameraState newState)
    {
        cameraState = newState;
    }
    private void UpdateDelta()
    {
        zValue = isReverse ? 1f : -1f;
        delta = new Vector3(3f, 1f, zValue);

        Quaternion newRotation = Quaternion.Euler(0f, isReverse ? 180f : 0f, 0f);
        transform.rotation = newRotation;
    }

    void LateUpdate()
    {
        // 플레이어가 존재하지 않다면 return
        if (player.IsUnityNull()) return;

        if (preIsReverse != isReverse) //isReverse에 변동이 있을때만
        {
            preIsReverse = isReverse;
            UpdateDelta(); // delta 값 업데이트
        }

        // 카메라가 고정되어야하는 경우에는 return;
        if (SceneManager.GetActiveScene().name == Define.Scene.SnowBoss123.ToString() || SceneManager.GetActiveScene().name == "SnowBoss3" || SceneManager.GetActiveScene().name == Define.Scene.SnowBoss4.ToString())
        {
            return;
        }

        if(cameraState == Define.CameraState.Player){ //만약 카메라 스테이트가 player 일때만 따라감
            // 카메라의 position은 플레이어의 position에 델타값을 더한 값
            transform.position = player.transform.position + delta;
        }

        // 플레이어가 피격당했다면 플레이어를 버림
        if (player_script.player_state == PlayerState.Damaged)
        {
            player = null;
        }

    }
}