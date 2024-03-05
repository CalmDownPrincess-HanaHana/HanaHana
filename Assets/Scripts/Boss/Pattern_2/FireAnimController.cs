using UnityEngine;
using System.Collections;

public class FireAnimController : MonoBehaviour
{
    private Animator f_Animator;

    [SerializeField] private float delayTime = 5.0f; // 애니메이션 시작을 기다릴 시간

    [SerializeField] private float waitingTime = 0f; //몇초 후에 시작할지

    [SerializeField] private string aniName = "";
    void Awake()
    {
        f_Animator = GetComponent<Animator>();
        StartCoroutine(FirstAnimation());
    }

    IEnumerator FirstAnimation()
    {
        yield return new WaitForSeconds(waitingTime); // 지정된 시간 동안 기다림
        // 애니메이션 시작
        f_Animator.Play(aniName, 0, 0f);
    }


    IEnumerator StartAnimationAfterDelay()
    {
        yield return new WaitForSeconds(delayTime); // 지정된 시간 동안 기다림
        // 애니메이션 시작
        f_Animator.Play(aniName, 0, 0f);
    }

}
