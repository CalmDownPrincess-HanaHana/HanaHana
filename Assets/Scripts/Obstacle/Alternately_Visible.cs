using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Alternately_Visible : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjects;
    [SerializeField] private float _time = 1.0f;
    private int _previousIndex = -1;

    private void Start()
    {
        StartCoroutine(AlternatelyVisible(_time));
    }

    public IEnumerator AlternatelyVisible(float time)
    {
        for (int i = 0; i < _gameObjects.Count + 1; i++)
        {
            i %= _gameObjects.Count;
            yield return new WaitForSeconds(time);
            if (_previousIndex != -1)
            {
                _gameObjects[_previousIndex].SetActive(false);
            }
            _gameObjects[i].SetActive(true);
            _previousIndex = i;
        }
    }
}