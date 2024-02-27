using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SnowBossClear : MonoBehaviour
{

    [SerializeField] GameObject[] stage;
    bool giveItemOnce = false;
    [SerializeField] GameObject[] texts; 
    Animator anim;
    //페이드아웃 판넬 UI 
    [SerializeField] private Image fadeOutPanel;
    //페이드아웃 bool 변수
    private bool fade_out = false;
    //타이머
    private float timer = 0;
    [SerializeField] AudioSource[] audio;
    [SerializeField] GameObject goToEndingPotal;

    private Player playerScript;
    private SceneChange clearData;
    private void Awake()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerScript.ChangeSprites();
        anim = GetComponent<Animator>();
        anim.SetBool("isCalm", true);
        //페이드아웃 켜지고
        fade_out = true;
        //페이드아웃 판넬의 부모오브젝트(캔버스) 가져옴
        Transform parent_trans = fadeOutPanel.gameObject.transform.parent;
        //켜주기
        parent_trans.gameObject.SetActive(true);
        clearData = new SceneChange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //아이템을 얻었는가?
            string realItem = PlayerPrefs.GetString("RealItem"+Define.Scene.SnowWhite.ToString());
            switch (realItem)
            {
                //얻었으면
                case "true":
                    if (!giveItemOnce)
                    {
                        audio[0].Play();
                        anim.SetBool("isCalm", false);
                        anim.SetBool("isClear", true);
                        Invoke("SqueekSound", 2.2f);
                        Invoke("Cheers", 9f);
                        StartCoroutine(ShowTextsAndStage());
                        PlayerPrefs.SetString("SnowWhiteClear", "true");
                        giveItemOnce = true;
                    }
                    
                    break;
                //못얻었으면
                default:
                    if (!giveItemOnce)
                    {
                        //닿았을 때 띠롱소리
                        audio[0].Play();
                        //애니메이션 바꾸기
                        anim.SetBool("isCalm", false);
                        anim.SetBool("isClear", false);
                        giveItemOnce = true;
                        //코루틴호출
                        StartCoroutine(NoItem());

                    }
                   
                    
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //페이드아웃 조건변수 켜지면
        if (fade_out)
        {
            //타이머 키고
            timer += Time.deltaTime;
            //색 밝아지게
            fadeOutPanel.color += new Color(0, 0, 0, -0.01f);
        }
        if (timer <= 0f)
        {
            fade_out = false;
        }
    }
    IEnumerator NoItem()
    {
        yield return new WaitForSeconds(1.5f);
        TwinkleEyes();
        yield return new WaitForSeconds(2f);
        CatchPlayer();
        playerScript.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.1f);
        ThrowPlayer();
        ThrowedPlayer();
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("idle", true);
        yield return new WaitForSeconds(3f);
        texts[2].SetActive(true);
        yield return new WaitForSeconds(3f);
        texts[2].SetActive(false);
        texts[3].SetActive(true);
        yield return new WaitForSeconds(2f);
        texts[3].SetActive(false);
        GameObject.Find("FadePrefab").GetComponent<FadeInOut>().StartFade = true;
        yield return new WaitForSeconds(3f);
        clearData.new_Change();
        SceneManager.LoadScene(Define.Scene.SnowWhite.ToString());
        yield return null;
    }
    private void TwinkleEyes()
    {
        audio[4].Play();
    }
    private void CatchPlayer()
    {
        audio[5].Play();
    }
    private void ThrowPlayer()
    {
        audio[6].Play();
    }
    private void ThrowedPlayer()
    {
        audio[7].Play();
    }
    private void SqueekSound()
    {
        audio[1].Play();
    }
    private void Cheers()
    {
        audio[2].Play();
        audio[3].Play();
    }
    IEnumerator ShowTextsAndStage()
    {
        yield return new WaitForSeconds(12f);
        texts[0].SetActive(true);
        yield return new WaitForSeconds(5f);
        texts[0].SetActive(false);
        texts[1].SetActive(true);
        yield return new WaitForSeconds(5f);
        texts[1].SetActive(false);
        yield return new WaitForSeconds(2f);
        stage[0].SetActive(true);
        yield return new WaitForSeconds(4f);
        stage[0].SetActive(false);
        stage[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        stage[1].SetActive(false);
        goToEndingPotal.SetActive(true);
    }
}
