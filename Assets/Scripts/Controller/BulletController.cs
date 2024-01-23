using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : ParentObstacleController //타일맵에서는 안됨. 
{
    public enum ObType
    {
        SpewWithCircle, //원 형태에서 개수로 던질 것인지 
        SpewWithDegree, //각도로 던질 것인지
        RandomSpew //아니면 그냥 랜덤으로 툭툭 던질 것인지. 
    }

    [SerializeField] private ObType obType;

    [SerializeField]
    private bool wantParent = false; //부모에 상속시키려고

    [SerializeField]
    private Transform parentTransform; //부모에 상속시키려고 -> 이러면 부모가 setacitve false 됐을때 함께 사라짐 유용함.


    [SerializeField] private float spewInterval = 1f; // 불을 뱉는 간격
    [SerializeField] private float spewDuration = 3f; // 불이 사라지는 시간
    [SerializeField] private float force = 10f; //불에게 주어지는 힘

    [SerializeField] private int roundNum = 15;

    // 각도 증가 단계
    [SerializeField]
    private float angleStep = 30f;

    [SerializeField] 
    private GameObject objPrefab;       // 작은 불 프리팹

    //각도 시작. 여기서 조심할게... 내가 설정을 잘못해서 우리가 흔히 아는 x=0 측이 각도 0 이 아니라 -90도 각도가 0으로 취급된댜. 
    [SerializeField]
    private float angleStart = 0f;

    //각도의 끝
    [SerializeField]
    private float angleEnd = 180f;

    //설명하자면 이대로라면 0도 (지각 아래방향), 30도, 60도, 90도, 120도, 150도, 180도 (직각 위 방향)로 동시에 불이 나온다.
    private GameObject objInstance;
    public override IEnumerator Activate()
    {
        switch (obType)
        {
            case ObType.SpewWithCircle:
                StartCoroutine(SpewWithCircle());
                break;
            case ObType.SpewWithDegree:
                StartCoroutine(SpewWithDegree());
                break;
            case ObType.RandomSpew: 
                StartCoroutine(RandomSpew());
                break;
        }
        yield return base.Activate(); // 부모 클래스의 Activate 메서드 실행 
    }

    IEnumerator SpewWithCircle()
    {   
        while(true){
            for (int i = 0; i < roundNum; i++)
                {
                    objInstance = Instantiate(objPrefab, transform.position, Quaternion.identity);
                    Rigidbody2D objRigidbody = objInstance.GetComponent<Rigidbody2D>();

                    Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                    
                    if (wantParent)
                    {
                        objInstance.transform.parent = parentTransform;
                    }

                    if (objRigidbody != null)
                    {
                         objRigidbody.AddForce(dirVec.normalized * force, ForceMode2D.Impulse);
                    }

                    yield return new WaitForSeconds(spewDuration);
                    Destroy(objInstance);
                }
        yield return new WaitForSeconds(spewInterval);
        }
    }

    IEnumerator SpewWithDegree()
    {   
        while (true)
        {
            Vector3 startDirection = new Vector3(0f, -1f, 0f);
            Vector3 targetDirection = new Vector3(0f, 1f, 0f);

            if (objPrefab != null)
            {
                for (float angle = angleStart; angle <= angleEnd; angle += angleStep)
                {
                    Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                    Vector3 direction = rotation * startDirection;

                    objInstance = Instantiate(objPrefab, transform.position, Quaternion.identity);
                    if (wantParent)
                    {
                        objInstance.transform.parent = parentTransform;
                    }
                    Rigidbody2D objRigidbody = objInstance.GetComponent<Rigidbody2D>();

                    if (objRigidbody != null)
                    {
                        objRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
                    }
                }
                yield return new WaitForSeconds(spewDuration);
                Destroy(objInstance);
            }
            yield return new WaitForSeconds(spewInterval); // 일정 시간 간격으로 반복
        }
    }

    IEnumerator RandomSpew()
    {   
        while (true)
        {
            if (objPrefab != null)
            {
                objInstance = Instantiate(objPrefab, transform.position, Quaternion.identity);
                if (wantParent)
                {
                    objInstance.transform.parent = parentTransform;
                }
                Rigidbody2D objRigidbody = objInstance.GetComponent<Rigidbody2D>();
                if (objRigidbody != null)
                {
                    Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
                    objRigidbody.AddForce(randomDirection * force, ForceMode2D.Impulse);
                }

                yield return new WaitForSeconds(spewDuration);
                Destroy(objInstance);
            }

            yield return new WaitForSeconds(spewInterval); // 일정 시간 간격으로 반복
        }
    }
  
}
