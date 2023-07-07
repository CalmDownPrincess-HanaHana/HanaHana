using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Vector3 followPos;
    [SerializeField]
    private int followDelay;
    public Transform player;
    private Queue<Vector3> playerPos;
    private bool isTriggered = false;
    private Player playerScript;
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;   
            this.gameObject.layer = 3;
            playerScript = collision.gameObject.GetComponent<Player>();
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("bomb!");
            playerScript.onDamaged(collision.transform.position);
        }
    }

    void Awake()
    {
        playerPos = new Queue<Vector3>();
    }
    // Update is called once per frame
    void Update()
    {
        if(isTriggered){
            Watch();
            Follow();
        }   
    }
    
    void Watch()
    {
        if(!playerPos.Contains(player.position))
            playerPos.Enqueue(player.position);
        if(playerPos.Count > followDelay)
            followPos = playerPos.Dequeue();
    }

    void Follow()
    {
        transform.position = followPos;
    }

}
