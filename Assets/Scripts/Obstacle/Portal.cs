using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Portal : MonoBehaviour
{
    [SerializeField] private Scene _nextScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SceneManagerEx sceneManagerEx = new SceneManagerEx();
        sceneManagerEx.LoadScene(_nextScene);
    }
}
