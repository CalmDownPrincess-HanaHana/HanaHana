using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomInOut : MonoBehaviour
{
    public enum zoomCriteria
    { 
        checkPoints
    }

    public Vector3[] points;
    [SerializeField] zoomCriteria zoomCriter;
    int pointIndex=0;
    Camera _camera;
    MovingController movingController;
    public float zoomSpeed=0.07f;
    float movingSpeed;
    private GameObject Tutorial;
    private TutorialText TutorialTextScript;
    public GameObject SaveLoad;
    private void Awake()
    {
        int tutorial_flag = SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("tutorial");
        if (tutorial_flag != 0)
        {
            Destroy(GetComponent<MovingController>());
        }
        _camera = GetComponent<Camera>();
        movingController = GetComponent<MovingController>();
        if (movingController != null)
        {
            movingSpeed = movingController.speed;
            movingController.enabled = false;
        }
        Tutorial=GameObject.Find("Tutorial");
        TutorialTextScript=Tutorial.GetComponent<TutorialText>();

    }
    // Update is called once per frame
    void Update()
    {
        switch (zoomCriter)
        {
            case zoomCriteria.checkPoints:
                float distanceToTarget=100f;//임의의 수 초기화용
                //타겟 꼭짓점과의 거리재기
                try
                {
                    distanceToTarget = Vector3.Distance(transform.position, points[pointIndex]);
                }
                catch(Exception e)
                {
                    Debug.Log(e.ToString()+"인덱스범위 초과. 관련 컴포넌트 비활성화");
                    Invoke("DeactiveComponents", 1.5f);
                    TutorialTextScript.isSummaryOver=true;
                }
                //도달했으면
                if (distanceToTarget <= 1.0f)
                {
                        
                        pointIndex++;
           
                        if (movingController != null)
                        {
                            movingController.speed = 0f;
                        }
                        StartCoroutine(ZoomInAndOut());
                }
                break;
        }
    }

    void DeactiveComponents()
    {
        if (movingController != null)
        {
            movingController.enabled = false;
        }
        GetComponent<CameraController>().enabled = true;
        GetComponent<CameraZoomInOut>().enabled = false;
        
    }

    IEnumerator ZoomInAndOut()
    {
        while (!(_camera.orthographicSize >=1.9f&&_camera.orthographicSize <= 2.0f))
        {
            _camera.orthographicSize -= zoomSpeed;
            yield return null;
        }
        while (!(_camera.orthographicSize >= 4.9f && _camera.orthographicSize <= 5.0f))
        {
            _camera.orthographicSize += zoomSpeed;
            yield return null;
        }
        _camera.orthographicSize = 5f;
        if (movingController != null)
        {
            movingController.speed = movingSpeed;
        }
    }
}
