using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 키라랏 ★
/// 예시) 스노우화이트맵 꺼졌다켜졌다 가시
/// </summary>
public class Twinkle : MonoBehaviour
{
    //렌더러가져옴 (스프라이트렌더러, 타일맵렌더러 부모클래스)
    private Renderer _renderer;
    //색 바꿔주게 변수 생성
    private Color my_color;

    private void Update()
    {
        //알파값이 일정 이하면 통과해도 ok,태그- transparent, 레이어- transparent
        if (my_color.a <= 0.001f)
        {
            gameObject.tag = "Transparent";
            gameObject.layer = 10;
        }
        //else: 일정 초과면 태그: Enemy, 레이어-Enemy
        else
        {
            gameObject.tag = "Enemy";
            gameObject.layer = 8;
        }
    }

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        my_color = new Color(1f, 1f, 1f, 1f);
        _renderer.material.color = my_color;
        StartCoroutine(TwinkleCoroutine());
    }

    IEnumerator TwinkleCoroutine()
    {
        while (true)
        {
            // 투명해지게
            while (my_color.a > 0f)
            {
                my_color.a -= 0.05f;
                _renderer.material.color = my_color;
                yield return new WaitForSeconds(0.01f);
            }
            // 투명하면 대기
            yield return new WaitForSeconds(0.7f);

            // 대기 후 색 값 리셋
            my_color.a = 1f;
        }
    }
}
