using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleUp : MonoBehaviour
{
    [SerializeField] 
    private float speed = 3f;
    private bool isTriggered = false;
    Rigidbody2D rigid;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;   
        }
    }

    void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(isTriggered)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }
    
}
