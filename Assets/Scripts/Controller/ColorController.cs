using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : ParentObstacleController
{
    public enum ObType
    {
        Transparent,
        Opaque,
        Twinkle
    }
    [SerializeField] private float value=0.05f;//증감값
    [SerializeField] private ObType obType;
    private Renderer renderer;
    private Color myColor;
    private float time;
    [SerializeField] private int r = 1;
    [SerializeField] private int g = 1;
    [SerializeField] private int b = 1;

    public override IEnumerator Activate()
    {
        switch (obType)
        {
            case ObType.Transparent:
                StartCoroutine(IntoTransparent());
                break;
            case ObType.Opaque:
                StartCoroutine(IntoOpaque());
                break;
            case ObType.Twinkle:
                StartCoroutine(Twinkle());
                break;
        }
        yield return base.Activate(); // 부모 클래스의 Activate 메서드 실행 
        //사실 ismoving과 별개로 움직이기 때문에 이걸 굳이 부모 activate를 실행하지 않아도 되지만 후에 test할때를 위해 그냥 실행하겠음. 
    }
    IEnumerator IntoTransparent()
    {
        while (true)
        {
            myColor.a -= value; // 알파 값 조정
            // 알파 값이 0 이하면 0으로 고정
            if (myColor.a <= 0f)
            {
                myColor.a = 0f;
                renderer.material.color = myColor;
                break;
            }
            renderer.material.color = myColor;
            yield return null;//프레임만큼 기다림
        }
    }
    IEnumerator IntoOpaque()
    {
        while (true)
        {
            myColor.a += value; // 알파 값 조정
            // 알파 값이 1이상이면 면 1으로 고정
            if (myColor.a >= 1f)
            {
                myColor.a = 1f;
                renderer.material.color = myColor;
                break;
            }
            renderer.material.color = myColor;
            yield return null;//프레임만큼 기다림
        }
    }
    IEnumerator Twinkle()
    {
        while (true)
        {
            if (time < 0.3f)
            {
                renderer.material.color = new Color(r, g, b, 1 - time);
            }
            else
            {
                renderer.material.color = new Color(r, g, b, time);
                if (time > 0.5f)
                {
                    time = 0;
                }
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void Awake()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
        switch (obType)
        {
            case ObType.Transparent:
                renderer.material.color = new Color(1f, 1f, 1f, 1f);
                myColor = new Color(1f, 1f, 1f, 1f);
                break;
            case ObType.Opaque:
                renderer.material.color = new Color(1f, 1f, 1f, 0f);
                myColor = new Color(1f, 1f, 1f, 0f);
                break;
        }
    }
}
