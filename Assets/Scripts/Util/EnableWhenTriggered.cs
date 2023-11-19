using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWhenTriggered : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _activeTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // trigger가 발동되면 타겟을 활성화함.
        // 이때 타겟을 컴포넌트의 리스트 형태.
        for (int i = 0; i < _activeTarget.Count; i++)
        {
            _activeTarget[i].enabled = true;
        }
    }
}
