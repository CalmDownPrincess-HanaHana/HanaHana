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
    private Vector3 maxPosition = new Vector3(7,1f,0);
    private Vector3 minPosition = new Vector3(-7, -3, 0);
    private bool isPatternEnd = true;
    private bool isGameOver = false;
    
    private Vector3 currentPos, currentScale = new Vector3(0,0,0);
    public CameraController _cameraController;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        _playerScript = playerTransform.gameObject.GetComponent<Player>();
        transform.position = playerTransform.position;
        transform.localScale = initScale;
    }

    private void Update()
    {
        if (!isPatternEnd) return;

        isPatternEnd = false;
        StartCoroutine(WaitAndScale());
    }

    private void LateUpdate()
    {
        if (isGameOver)
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

        Debug.Log($"currentPos: {currentPos}, currentScale: {currentScale}");
        transform.DOMove(currentPos, duration);
        transform.DOScale(currentScale, duration);
    }

    private float GetPosX()
    {
        float delatX, posX;
        do {
            posX = Random.Range(minPosition.x, maxPosition.x);
            delatX = Mathf.Abs((posX - currentPos.x));
        } while (delatX < 4f || delatX > 9f);
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
        Vector3 origin = new Vector3(playerTransform.position.x, playerTransform.position.y, 50f);
        Vector3 dir = playerTransform.position - origin;
        RaycastHit2D[] hits = Physics2D.RaycastAll( origin, dir, 100f );
        foreach (var hit in hits)
        {
            Debug.Log($"{hit.transform.name}");
            if (hit.transform.gameObject == gameObject)
            {
                return false;
            }
        }
        return true;
    }
}