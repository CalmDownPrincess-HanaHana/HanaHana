using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField]
    private GameObject fix_panel;

    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private GameObject Soul; //움직이는 커다란 정령

    [SerializeField]
    private float fadeDuration = 1.0f; // 페이드 인/아웃에 걸리는 시간 (초)


    private CameraController camScript;
    private CanvasRenderer panelRenderer;
    private bool hasTriggered = false;

    private void Start()
    {
        // 패널의 CanvasRenderer 컴포넌트 가져오기
        panelRenderer = fix_panel.GetComponent<CanvasRenderer>();
        camScript = camera.GetComponent<CameraController>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!hasTriggered)
        {
            hasTriggered = true;

            // 패널 활성화, Soul 활성화, Time.timeScale 조정 등의 코드 실행
            fix_panel.SetActive(true);
            Soul.SetActive(true); //유령 활성화.
            Time.timeScale = 0;
            //여기서 일시정지를 위해 0f 했는데 조금 더 똑똑하게 할 수 없었나 싶다. 
            //이게 0f로 되니까 잔상이 안사라졌다... 그런대로 킹받게 잘 나와서 다행이지만 원하던 효과는 아니었던 것.
            StartCoroutine(FadeInPanel());

            // 패널 점점 어두워짐
        }
    }

    private IEnumerator FadeInPanel()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            // 경과 시간에 따른 알파 값 변경
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            panelRenderer.SetAlpha(alpha);

            // 다음 프레임으로 이동
            yield return null;

            // 경과 시간 갱신
            elapsedTime += Time.unscaledDeltaTime;
        }

        // 알파 값 최종 설정
        panelRenderer.SetAlpha(1f);

        // 페이드 인 효과가 끝나면 패널 비활성화
        camScript.isReverse = true; //reverse카메라 
        fix_panel.SetActive(false);
        Soul.SetActive(false);
        Time.timeScale = 1f;
    }
}
