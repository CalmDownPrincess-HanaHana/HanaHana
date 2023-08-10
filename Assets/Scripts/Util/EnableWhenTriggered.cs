using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnableWhenTriggered : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _activeTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < _activeTarget.Count; i++)
        {
            _activeTarget[i].enabled = true;
        }
    }
}
