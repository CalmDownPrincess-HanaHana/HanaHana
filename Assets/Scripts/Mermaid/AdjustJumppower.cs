using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustJumppower : MonoBehaviour
{
    [SerializeField] private float jumpPower = 10f;
    private Rigidbody2D playerRigidbody;
    private Player playerScript;
    private GameObject player;
    private bool isOnce=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOnce&&collision.gameObject.CompareTag("Player"))
        {
            isOnce=true;
            player = collision.gameObject;
            playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.edit_jump = true;
                playerScript.editted_jump_power=jumpPower;
            }
        }
    }

    //함수가 호출되면 jump power를 원래대로 되돌리고, 해당 오브젝트를 파괴합니다.
    public void ResetJumpPower()
    {
        if(player!=null&&playerScript != null)
        {
            playerScript.edit_jump=false;
            // 현재 오브젝트를 파괴합니다.
            Destroy(gameObject);
        }
    }
}
