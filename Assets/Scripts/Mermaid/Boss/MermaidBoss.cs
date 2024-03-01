using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MermaidBoss : MonoBehaviour
{
    BulletController bulletController;
    public Slider hp;
    public GameObject[] patterns;
    public GameObject player;
    public AudioSource[] audio;
    public GameObject fadeUI;
    invincibilityForSeconds playerInvicibility;
    Player playerScript;
    GameObject _camera;
    bool once = false;
    private void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");

        playerInvicibility = player.GetComponent<invincibilityForSeconds>();
        playerScript = player.GetComponent<Player>();
        //패턴 재생
        StartCoroutine(Pattern());
        bulletController = new BulletController();
    }
    private void Update()
    {
        //죽었으면, 패턴 재생x
        if (playerScript.player_state == Define.PlayerState.Damaged&&!once)
        {
            StopCoroutine(Pattern());
        }
        //꺴음
        if (hp.value <= 0&&!once)
        {
            StartCoroutine(EndingCorutine());
            once = true;
        }
    }

    IEnumerator EndingCorutine()
    {
        //패턴6끄기
        patterns[5].SetActive(false);
        //프리팹지우기
        bulletController.DeleteCloneObjects();
        playerScript.Invincibility = true;
        yield return null;
        //화면흔들린다.
        _camera.transform.DOShakePosition(3f, new Vector3(0.5f, 0.5f, 0));
        //무너지는 소리
        audio[0].Play();

        //비누방울 깨진다.
        yield return new WaitForSeconds(2.5f);
        //비누방울 깨지는 소리
        audio[1].Play();
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().enabled = true;
        //페이드아웃
        yield return new WaitForSeconds(3f);
        fadeUI.SetActive(true);
        //씬 이동한다
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(Define.Scene.MerMaidBossClear.ToString());

    }
    IEnumerator Pattern()
    {
        //무적
        StartCoroutine(playerInvicibility.Initialization(1f));
        //패턴1
        patterns[0].SetActive(true);
        yield return new WaitForSeconds(15f);
        //패턴1끄기
        patterns[0].SetActive(false);

        //무적
        StartCoroutine(playerInvicibility.Initialization(3f));
        //패턴2
        patterns[1].SetActive(true);
        yield return new WaitForSeconds(15f);
        //패턴2끄기
        patterns[1].SetActive(false);

        //무적
        StartCoroutine(playerInvicibility.Initialization(3f));
        //패턴3
        patterns[2].SetActive(true);
        yield return new WaitForSeconds(15f);
        //패턴3끄기
        patterns[2].SetActive(false);

        //무적
        StartCoroutine(playerInvicibility.Initialization(3f));
        //패턴4
        patterns[3].SetActive(true);
        player.transform.position = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(15f);
        //패턴4끄기
        patterns[3].SetActive(false);

        //무적
        StartCoroutine(playerInvicibility.Initialization(3f));
        //패턴5
        patterns[4].SetActive(true);
        player.transform.position = new Vector3(1.5f, 1.5f, 0) ;
        yield return new WaitForSeconds(15f);
        //패턴5끄기
        patterns[4].SetActive(false);

        //무적
        StartCoroutine(playerInvicibility.Initialization(3f));
        //패턴6
        patterns[5].SetActive(true);
        yield return new WaitForSeconds(15f);
        
        
        yield return null;
    }
}
