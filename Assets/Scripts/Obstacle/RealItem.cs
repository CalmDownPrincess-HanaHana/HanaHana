using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RealItem : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != _player)
        {
            return;
        }
        PlayerPrefs.SetString("RealItem"+ SceneManager.GetActiveScene().name,"true" );
        _player.GetComponent<Player>().ChangeSprites();
        GameObject.Find("RealItemSound").GetComponent<AudioSource>().Play();
        this.gameObject.SetActive(false);
    }
}
