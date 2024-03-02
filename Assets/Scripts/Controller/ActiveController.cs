using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveController : ParentObstacleController
{   
     public enum ObType
    {
        Twinkle, //반짝반짝,
        AlertBlink, //위험한 거 경고용  
        DestroyMyself, //스스로 destroy 한다. 
        SwitchActivateStatus, //트리거되면 활성화 <-> 비활성화
        AlternatelyActivate, //순차적으로 obj를 active 해준다. 
        ChangeStatus, //obj의 tag나 layer을 바꿔준다.
        SwitchColliderStatus //콜라이더 활성화 비활성화를 도움.
    }

    [SerializeField] private ObType obType;
    [SerializeField] private Color alertColor = new Color(1f, 0.87f, 0.0039f, 1f); // RGB 및 알파값으로 수정
    [SerializeField] private bool repeat=true; //alternately visible 용.
    [SerializeField] private List<GameObject> altObj; //alternately visible 용. 
    [SerializeField] private GameObject actObj; //사라졌다 생기는 obj
    [SerializeField] private float durationTime = 1.0f; //변화하는 시간 
    
    private Renderer renderer;
    //색 바꿔주게 변수 생성
    private Color objColor;

    private Collider2D myCollider;
    
    private Color originColor;

    private void Awake()
    {
        base.Awake();
        renderer = GetComponent<Renderer>();
        myCollider = GetComponent<Collider2D>();
        if (renderer != null)
        {
            originColor = renderer.material.color;
        }
    }

    private void update(){
        
    }

    public override IEnumerator Activate()
    {
        switch (obType)
        {
            case ObType.Twinkle:
                objColor = new Color(1f, 1f, 1f, 1f);
                renderer.material.color = objColor;

                StartCoroutine(Twinkle());
                break;
            case ObType.AlertBlink:
                StartCoroutine(AlertBlink());
                break;
            case ObType.DestroyMyself: 
                StartCoroutine(DestroyMyself());
                break;
            case ObType.SwitchActivateStatus:
                StartCoroutine(SwitchActivateStatus());
                break;
            case ObType.AlternatelyActivate:
                StartCoroutine(AlternatelyActivate());
                break;
            case ObType.SwitchColliderStatus:
                StartCoroutine(SwitchColliderStatus());
                break;
        }
        yield return base.Activate(); // 부모 클래스의 Activate 메서드 실행 
    }

    IEnumerator Twinkle()
    { 
        while (true)
        {
            // 투명해지게
            while (objColor.a > 0f)
            {
                objColor.a -= 0.05f;
                renderer.material.color = objColor;
                yield return new WaitForSeconds(0.01f);
            }
            // 투명하면 layer 바꿔준 다음에 대기
            gameObject.tag = "Transparent";
            gameObject.layer = 10;
            yield return new WaitForSeconds(0.7f);
            // 다시 불투명해질 것이기 때문에 다시 enemy로 레이어 바꿔준다.
            gameObject.tag = "Enemy";
            gameObject.layer = 8;

            // 대기 후 색 값 리셋
            objColor.a = 1f;
        }
    }

    IEnumerator AlertBlink()
    {
        float time = 0f;
        float realTime = 0f;
        while (realTime < durationTime)
        {
            if (time < 0.3f)
            {
                renderer.material.color = new Color(alertColor.r, alertColor.g, alertColor.b, 1 - time);
            }
            else
            {
                renderer.material.color = new Color(alertColor.r, alertColor.g, alertColor.b, time);
                if (time > 0.5f)
                {
                    time = 0f;
                }
            }

            time += Time.deltaTime;
            realTime += Time.deltaTime;
            yield return null;
        }
        renderer.material.color = originColor;
    }

    //isCol이랑 waiting time으로 조절 해줘야함
    IEnumerator DestroyMyself()
    {   
        Destroy(this.gameObject);
        yield return null;
    }

    IEnumerator SwitchActivateStatus()
    {
        actObj.gameObject.SetActive(!actObj.gameObject.activeSelf); 
        yield return null;
    }

    IEnumerator AlternatelyActivate()
    {
        int _previousIndex = -1;

        for (int i = 0; i < altObj.Count + 1; i++)
        {
            if (!repeat && i==altObj.Count) { break; }
            i %= altObj.Count;
            yield return new WaitForSeconds(durationTime);
            if (_previousIndex != -1)
            {
                altObj[_previousIndex].SetActive(false);
            }
            altObj[i].SetActive(true);
            _previousIndex = i;
        }
    }

    IEnumerator SwitchColliderStatus()
    {
        // 콜라이더 상태 변경
        myCollider.enabled = !myCollider.enabled;
        yield return null;
    }
}
