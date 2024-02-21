using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermaidPattern3Light : MonoBehaviour
{
    public float time = 3f; 
    float timer = 0f;
    GameObject player;
    SpriteRenderer playerRenderer;
    public string playerInLayerName = "UI";
    public int playerInLayerNum = 2;
    public string playerOutLayerName = "Default";
    public int playerOutLayerNum = 3;

    public string inLayerName = "UI";
    public int inLayerNum = 1;
    public string outLayerName = "Default";
    public int outLayerNum = 1;
    public bool setPlayerPositionThis=false;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerRenderer = player.GetComponent<SpriteRenderer>();
        if (setPlayerPositionThis)
        {
            player.transform.position = this.gameObject.transform.position;
        }    
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer >= 3f)
        {
            player.GetComponent<Player>().Die(new Vector2(player.transform.position.x,player.transform.position.y));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(Timer());
            timer = 0f;
            playerRenderer.sortingLayerName = playerInLayerName;
            playerRenderer.sortingOrder = playerInLayerNum;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Renderer>().sortingLayerName = inLayerName;
            collision.GetComponent<Renderer>().sortingOrder = inLayerNum;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Timer());
            playerRenderer.sortingLayerName = playerOutLayerName;
            playerRenderer.sortingOrder = playerOutLayerNum;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Renderer>().sortingLayerName = outLayerName;
            collision.GetComponent<Renderer>().sortingOrder = outLayerNum;

        }
    }

    IEnumerator Timer()
    {
        while (true)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
