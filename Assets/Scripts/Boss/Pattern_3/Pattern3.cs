using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern3 : MonoBehaviour
{

    [SerializeField] List<GameObject> _patterns;
    

    bool _isInverted = false;
    Player _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        PatternChange();
    }

    void PatternChange()
    {
        // ī�޶� shaking
        Camera.main.transform.DOShakePosition(4, 1);

        // ���� �ִϸ��̼� ����
    }

    IEnumerator InvertScene()
    {
        while (true)
        {
            
        }
    }
}
