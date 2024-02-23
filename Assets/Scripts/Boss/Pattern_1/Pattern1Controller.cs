using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// pattern1 controller
/// 초단위로 계산해서 아래 phase object 활성/비활성화함
/// 1. phase object를 함수로 만들어서 하나의 스크립트/여러개 스크립트에 넣기
/// 2. 이것도 다시 개선
/// </summary>
public class Pattern1Controller : MonoBehaviour
{
    //public GameObject phases=new List<GameObject>();
    private Animator animator;
    public GameObject pattern1;
    public GameObject pattern2;
    public GameObject pattern3;
    public GameObject pattern4;
    public GameObject pattern5;
    public Rigidbody2D other;
    public GameObject SaveLoad;
    public AudioSource background;
    public GameObject pattern1Text;

    /// <summary>
    /// Player불러오기
    /// </summary>
    private Player player;
    //카메라
    private GameObject camera;
    //패턴체인지효과
    [SerializeField] GameObject patternChangeGO;
    //페이드아웃
    [SerializeField] GameObject _fadeOutUI;
    void Start()
    {
        //플레이어 불러오기
        player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
        //카메라 불러오기
        camera = GameObject.FindWithTag("MainCamera").gameObject;
        Transform snowhiteTransform = transform.Find("SnowWhite");

        if (snowhiteTransform != null)
        {
            // Get the Animator component of the Snowhite object
            animator = snowhiteTransform.GetComponent<Animator>();
            if (animator == null) {
            }
        }

        StartCoroutine(Pattern());
    }

    void Update()
    {
        if (player.player_state == Define.PlayerState.Damaged)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator Pattern()
    {
        background.Play();
        other.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetInteger("level", 1);//phase1_wave
        //Run!
        pattern1Text.SetActive(true);
        yield return new WaitForSeconds(2f);
        //Run!
        pattern1Text.SetActive(false);
        other.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.SetInteger("level", 0);
        pattern1.SetActive(true);
        yield return new WaitForSeconds(12f);
        pattern1.SetActive(false);
        DeleteCloneObjects();

        animator.SetInteger("level", 4);//phase4_bottomup
        yield return new WaitForSeconds(2f);
        animator.SetInteger("level", 0);
        pattern4.SetActive(true);
        StartCoroutine(PlayerInvincibility(1f));
        yield return new WaitForSeconds(12f);
        other.constraints = RigidbodyConstraints2D.FreezeRotation;
        pattern4.SetActive(false);
        DeleteCloneObjects();

        animator.SetInteger("level", 2);//phase2_fireball
        yield return new WaitForSeconds(2f);
        animator.SetInteger("level", 0);
        pattern2.SetActive(true);
        StartCoroutine(PlayerInvincibility(1f));
        yield return new WaitForSeconds(12f);
        other.constraints = RigidbodyConstraints2D.FreezeRotation;
        pattern2.SetActive(false);
        DeleteCloneObjects();

        animator.SetInteger("level", 3);//phase3_goflame
        yield return new WaitForSeconds(2f);
        animator.SetInteger("level", 0);
        pattern3.SetActive(true);
        StartCoroutine(PlayerInvincibility(1f));
        yield return new WaitForSeconds(12f);
        other.constraints = RigidbodyConstraints2D.FreezeRotation;
        pattern3.SetActive(false);
        DeleteCloneObjects();

        animator.SetInteger("level", 5);//phase5_hideeyes
        yield return new WaitForSeconds(2f);
        background.Stop();
        pattern5.SetActive(true);
        StartCoroutine(PlayerInvincibility(1f));
        yield return new WaitForSeconds(2.3f);
        animator.SetInteger("level", 6);//phase6
        yield return new WaitForSeconds(1.7f);
        animator.SetInteger("level", 5);//phase5_hideeyes
        yield return new WaitForSeconds(2.3f);
        animator.SetInteger("level", 6);//phase6
        yield return new WaitForSeconds(1.7f);
        animator.SetInteger("level", 5);//phase5_hideeyes
        yield return new WaitForSeconds(2.3f);
        animator.SetInteger("level", 6);//phase6
        yield return new WaitForSeconds(1.7f);
        DeleteCloneObjects();
        pattern5.SetActive(false);

        //다음 씬()으로 로드
        if (player.player_state != Define.PlayerState.Damaged && SceneManager.GetActiveScene().name=="SnowBoss1")
        {
            StartCoroutine("PatternChange");
        }
    }

    //짠탄제거
    public void DeleteCloneObjects()
    {
        // 씬 내의 모든 게임 오브젝트 가져오기
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            //클론된 애면
            if (IsClone(obj))
            {
                Destroy(obj); // 클론 오브젝트 삭제
            }
        }
    }

    //클론된애인지 판별
    private bool IsClone(GameObject obj)
    {
        // 이름에 "(Clone)" 문자열이 포함되어 있는지 검사
        return obj.name.Contains("(Clone)");
    }
    //패턴체인지
    IEnumerator PatternChange()
    {
        player.Invincibility = true;
        patternChangeGO.SetActive(true);

        // camera shaking
        camera.transform.DOShakePosition(3f, new Vector3(0.1f, 0.1f, 0));

        // 눈비비기
        animator.SetInteger("level", 5);

        
        yield return new WaitForSeconds(2f);
        _fadeOutUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SnowBoss2");
    }
    //무적
    IEnumerator PlayerInvincibility(float time)
    {
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0.5f);
        player.Invincibility = true;
        yield return new WaitForSeconds(time);
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        player.Invincibility = false;
    }


}
