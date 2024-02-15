using System.Collections;
using UnityEngine;

public class FlagSaved : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite newSprite; // 바뀔 스프라이트
    private Sprite originalSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    // OnTriggerEnter 이벤트가 발생했을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 다른 오브젝트와 충돌한 경우 ChangeFlag 코루틴 시작
        StartCoroutine(ChangeFlag());
    }

    // 0.5초 동안 스프라이트를 변경하고 다시 원래 스프라이트로 되돌리는 코루틴
    private IEnumerator ChangeFlag()
    {
        // 새로운 스프라이트로 변경
        spriteRenderer.sprite = newSprite;
        // 0.5초 대기
        yield return new WaitForSeconds(0.5f);

        // 원래 스프라이트로 돌아가기
        spriteRenderer.sprite = originalSprite;
    }
}
