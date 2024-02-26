using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBlink : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color originalColor = Color.white; // 원래 색깔
    public Color blinkColor = Color.red; // 깜빡이는 색깔

    private void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            // 원래 색깔로 변경
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f); // 0.5초 대기

            // 깜빡이는 색깔로 변경
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(0.1f); // 0.5초 대기
        }
    }
}
