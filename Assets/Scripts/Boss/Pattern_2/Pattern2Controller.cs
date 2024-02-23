using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//보스 패턴 랜덤으로 돌리는 코드입니다.
//한 스크립트에 다 연결했는데 걍 스크립트를 PATTERN OBSTACLE들한테 부착해도 되지 않을까 생각중.
//부모 클래스로 만들면 재활용에 용이할듯합니다. 부모 말고도 script 이름만 맞추면 잘 쓸수 있을지도??

public class Pattern2Controller : MonoBehaviour
{
    private System.Random random = new System.Random();
    //패턴을 넣습니다. 패턴별로 부모 스크립트에 넣고, 비활성화했다가 활성화 되면 움직이는 식으로 만들었어요 
    private List<string> bossAnimation = new List<string> { "isCollectingEnergy" };

    public List<GameObject> bossPatterns = new List<GameObject>();
    [SerializeField] Camera camera;

    [SerializeField] GameObject patternChangeGO;

    public Slider slHP; //보스 피받아오기

    private float currentTime; //현재 시간
    private float currentHP;//현재 HP 

    private int previousIndex = -1;
    public Animator animator;
    [SerializeField] GameObject startMessage;
    //player
    Player player;
    //페이드아웃
    [SerializeField] GameObject _fadeOutUI;
    private void Start()
    {
        //플레이어 스크립트
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        currentHP = slHP.maxValue; //슬라이더 시작값 받아오기
        currentTime = slHP.maxValue;
        StartCoroutine(showStartMessage());
        StartNextPattern();
    }

    private void Update()
    {
        
        if (slHP.value <= 0)
        {
            if (player.player_state != Define.PlayerState.Damaged)
            {
                StartCoroutine("PatternChange"); //이거는 전체 보스맵 4개의 패턴에서의 패턴. scene change로 바꾸면 좋을 것 같음.
            }
        }

        if (slHP.value <= currentHP - 17f && bossPatterns.Count > 0) //17초 지나면, 그리고 가능한 패턴이 남아있다면
        {
            currentHP -= 17f;
            StartNextPattern(); //여기서는 보스맵 2 안의 패턴
        }

    }

    

    //랜덤으로 패턴 시작하기. 과거 나온 패턴은 리스트에서 삭제한다. 
    private void StartNextPattern()
    {

        // 이전 패턴을 제외한 패턴들로 리스트 갱신
        if (previousIndex != -1)
        {
            bossPatterns[previousIndex].SetActive(false);
            bossPatterns.RemoveAt(previousIndex);
        }

        // 랜덤한 패턴 선택
        int randomIndex = random.Next(bossPatterns.Count);
        // 선택된 패턴 실행
        bossPatterns[randomIndex].SetActive(true);
        animator.Play("Boss_HitTable", -1, 0f);
        StartCoroutine(PlayerInvincibility(1f));
        // 이전 패턴 업데이트
        previousIndex = randomIndex;
    }
    //패턴체인지
    IEnumerator PatternChange()
    {
        player.Invincibility = true;
        patternChangeGO.SetActive(true);

        // camera shaking
        camera.transform.DOShakePosition(3f, new Vector3(0.1f, 0.1f, 0));

        // 눈비비기
        animator.SetBool("isHideEye", true);

        yield return new WaitForSeconds(2f);
        _fadeOutUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (SceneManager.GetActiveScene().name == "SnowBoss2") {
            SceneManager.LoadScene("SnowBoss3");
        }
        
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
    IEnumerator showStartMessage()
    {
        startMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        startMessage.SetActive(false);
        yield return null;
    }
}
