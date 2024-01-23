using UnityEngine;

public class seaweed : MonoBehaviour
{
    private bool isPlayerInside = false;
    private Rigidbody2D playerRigidbody;
    private int jumpCount = 0;
    private float jumpButtonEnd = Screen.width;
    private void Start()
    {
        playerRigidbody = null;
        // Z 회전을 고정
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    private void Update()
    {
        if (isPlayerInside)
        {
            // 플레이어의 움직임을 고정
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero;
                // Z 회전을 고정
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        // 점프 키 체크(mobile)
        Touch[] touches = Input.touches;
        //모바일
        foreach (Touch touch in touches)
        {
            //점프키범위
            if (touch.position.x >= Screen.width * 0.5f && touch.position.x < jumpButtonEnd)
            {
                jumpCount++;

                if (jumpCount >= 3)
                {
                    // 플레이어의 움직임 고정 해제
                    if (playerRigidbody != null)
                    {
                        // 여기에 움직임을 고정 해제하는 코드를 작성
                        // 예를 들어, Rigidbody의 constraints를 초기화하거나 고정된 위치를 해제하는 등의 조치를 취할 수 있습니다.
                        playerRigidbody.constraints = RigidbodyConstraints2D.None;
                        // Z 회전을 고정
                        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                        // 초기화
                        ResetState();
                    }
                }
            }
        }
        // 점프 키 체크(pc)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpCount++;

            if (jumpCount >= 3)
            {
                // 플레이어의 움직임 고정 해제
                if (playerRigidbody != null)
                {
                    // 여기에 움직임을 고정 해제하는 코드를 작성
                    // 예를 들어, Rigidbody의 constraints를 초기화하거나 고정된 위치를 해제하는 등의 조치를 취할 수 있습니다.
                    playerRigidbody.constraints = RigidbodyConstraints2D.None;
                    // Z 회전을 고정
                    playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                    // 초기화
                    ResetState();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerRigidbody = other.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 영역을 벗어났을 때 초기화
            ResetState();
        }
    }

    private void ResetState()
    {
        // 여기에 플레이어의 상태를 초기화하는 코드를 작성
        // 예를 들어, Rigidbody의 constraints를 초기화하거나 고정된 위치를 해제하는 등의 조치를 취할 수 있습니다.

        isPlayerInside = false;
        playerRigidbody = null;
        // Z 회전을 고정
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

    }
}
