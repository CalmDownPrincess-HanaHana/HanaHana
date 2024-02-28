using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircleScaler : MonoBehaviour
{
    private Transform playerTransform;
    private Player _playerScript;
    public Vector3 initScale = new Vector3(5, 5, 0);
    
    public Vector3 maxScale = new Vector3(12, 8f, 0);
    public Vector3 minScale = new Vector3(3f, 3f, 0);
    public float maxDuration = 4f;
    public float minDuration = 3f;

    public float maxPositionY = 1f;

    protected Vector3 maxPosition;
    private Vector3 minPosition = new Vector3(-7, -3, 0);
    private bool isPatternEnd = true;
    private bool isGameOver = false;

    private float xRatio;
    private float yRatio;

    [SerializeField] private Camera _mainCamera;
    private Vector3 currentPos, currentScale = new Vector3(0,0,0);

    private void Start()
    {
        maxPosition = new Vector3(7,maxPositionY,0);
        // 화면 크기에 맞게 min, max 값 조정
        float xScreenHalfSize = _mainCamera.orthographicSize * _mainCamera.aspect;
        float yScreenHalfSize = _mainCamera.orthographicSize;

        // 18.0f => 유니티 기준 화면 크기
        xRatio = (xScreenHalfSize * 2.0f) / 18.0f;
        yRatio = (yScreenHalfSize * 2.0f) / 10.0f;

        initScale = new Vector3(initScale.x * xRatio, initScale.y * yRatio, initScale.z);
        maxScale = new Vector3(maxScale.x * xRatio, maxScale.y * yRatio, maxScale.z);
        minScale = new Vector3(minScale.x * xRatio, minScale.y * yRatio, minScale.z);
        maxPosition = new Vector3(maxPosition.x * xRatio, maxPosition.y * yRatio, maxPosition.z);
        minPosition = new Vector3(minPosition.x * xRatio, minPosition.y * yRatio, minPosition.z);
        
        // 패턴 초기 위치 플레이어 위치로 고정
        playerTransform = GameObject.FindWithTag("Player").transform;
        _playerScript = playerTransform.gameObject.GetComponent<Player>();
        transform.position = playerTransform.position;
        transform.localScale = initScale;

        StartCoroutine(CheckGameOverAfterInvincOver());
    }

    private void Update()
    {
        if (!isPatternEnd) return;

        isPatternEnd = false;
        StartCoroutine(WaitAndScale());
    }

    private void LateUpdate()
    {
        if (isGameOver&&!_playerScript.Invincibility)
        {
            _playerScript.Die(transform.position);
        }
    }

    IEnumerator WaitAndScale()
    {
        float duration = Random.Range(minDuration, maxDuration);
        MoveAndScale(duration);
        yield return new WaitForSeconds(duration);
        isPatternEnd = true;
    }

    private void MoveAndScale(float duration)
    {
        currentPos.x = GetPosX();
        
        if (currentPos.x > -3f && currentPos.x < 3f)
        {
            currentPos.y = Random.Range(minPosition.y, maxPosition.y - 3f);
            currentScale.y = Random.Range(5f, maxScale.y);
        }
        else
        {
            currentPos.y = Random.Range(minPosition.y, maxPosition.y);
            currentScale.y = Random.Range(minScale.y, maxScale.y);
        }
        currentScale.x = Random.Range(minScale.x, maxScale.x);

        transform.DOMove(currentPos, duration);
        transform.DOScale(currentScale, duration);
    }

    private float GetPosX()
    {
        float delatX, posX;
        do {
            posX = Random.Range(minPosition.x, maxPosition.x);
            delatX = Mathf.Abs((posX - currentPos.x));
        } while (delatX < 4f * xRatio || delatX > 9f * xRatio);
        return posX;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isGameOver = CheckGameOver();
        }
    }
    
    bool CheckGameOver()
    {
        if (_playerScript.Invincibility)
        {
            return false;
        }
        Vector3 origin = new Vector3(playerTransform.position.x, playerTransform.position.y, 50f);
        Vector3 dir = playerTransform.position - origin;
        RaycastHit2D[] hits = Physics2D.RaycastAll( origin, dir, 100f );
        foreach (var hit in hits)
        {
            if (hit.transform.gameObject == gameObject)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator CheckGameOverAfterInvincOver()
    {
        yield return new WaitForSeconds(1.1f);
        isGameOver = CheckGameOver();
    }
}