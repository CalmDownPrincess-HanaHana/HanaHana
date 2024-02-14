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
    Camera camera;
    MovingController movingController;
    public float zoomSpeed=0.07f;
    float movingSpeed;
    private GameObject Tutorial;
    private TutorialText TutorialTextScript;
    private void Awake()
    {
        camera = GetComponent<Camera>();
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
                    Debug.Log("인덱스범위 초과. 관련 컴포넌트 비활성화");
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
        while (!(camera.orthographicSize >=1.9f&&camera.orthographicSize <= 2.0f))
        {
            camera.orthographicSize -= zoomSpeed;
            yield return null;
        }
        while (!(camera.orthographicSize >= 4.9f && camera.orthographicSize <= 5.0f))
        {
            camera.orthographicSize += zoomSpeed;
            yield return null;
        }
        camera.orthographicSize = 5f;
        if (movingController != null)
        {
            movingController.speed = movingSpeed;
        }
    }
}
