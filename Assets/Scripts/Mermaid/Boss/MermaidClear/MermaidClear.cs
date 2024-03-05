using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MermaidClear : MonoBehaviour
{
    bool giveItemOnce = false;
    public AudioSource[] _audio;
    public GameObject[] texts;
    GameObject player;
    Player playerScript;
    Animator anim;
    private SceneChange clearData=new SceneChange();
    public GameObject FadePrefab;
    private void Awake()
    {

        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerScript.ChangeSprites();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //깼을때
            //아이템을 얻었는가?
            string realItem = PlayerPrefs.GetString("RealItem" + Define.Scene.MerMaid.ToString());
            switch (realItem)
            {
                case "true":
                    //꺴을때
                    if (!giveItemOnce)
                    {

                        _audio[0].Play();
                        Invoke("TearSound", 2.2f);
                        Invoke("Cheers", 9f);
                        StartCoroutine(ShowTexts());
                        PlayerPrefs.SetString("MermaidClear", "true");
                        giveItemOnce = true;
                    }

                    break;
                default:
                    //못깼을때
                    if (!giveItemOnce)
                    {
                        //닿았을 때 띠롱소리
                        _audio[0].Play();
                        giveItemOnce = true;
                        //코루틴호출
                        StartCoroutine(NoItem());
                    }

                    break;
            }
        }
    }
    private void TearSound()
    {
        anim.enabled = true;
        anim.SetBool("Clear", true);
        _audio[1].Play();
    }
    private void Cheers()
    {
        _audio[2].Play();
        _audio[3].Play();
    }

    IEnumerator ShowTexts()
    {
        yield return new WaitForSeconds(12f);
        texts[0].SetActive(true);
        yield return new WaitForSeconds(5f);
        texts[0].SetActive(false);
        texts[1].SetActive(true);
        yield return new WaitForSeconds(5f);
        texts[1].SetActive(false);
        FadePrefab.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainScene");
    }
    IEnumerator NoItem()
    {
        yield return new WaitForSeconds(0.5f);
        anim.enabled = true;
        anim.SetBool("Clear", false);
        yield return new WaitForSeconds(0.5f);
        Swiming();
        yield return new WaitForSeconds(5f);
        Destroy(GetComponent<ObstacleController>());
        Destroy(GetComponent<MovingController>());
        GetComponent<MovingController>().enabled = false;
        HitPlayer();
        
        yield return null;
    }

    private void Swiming()
    {
        _audio[4].Play();
        GetComponent<ObstacleController>().enabled = true;
    }
    private void HitPlayer()
    {
        player.gameObject.SetActive(false);
        this.transform.position = new Vector3(0, 0, 0);
        anim.SetBool("Hit", true);
        _audio[5].Play();
        StartCoroutine(ShowNoClearTexts());
    }

    IEnumerator ShowNoClearTexts()
    {
        yield return new WaitForSeconds(3f);
        texts[2].SetActive(true);
        yield return new WaitForSeconds(3f);
        texts[2].SetActive(false);
        texts[3].SetActive(true);
        yield return new WaitForSeconds(2f);
        texts[3].SetActive(false);
        FadePrefab.SetActive(true);
        yield return new WaitForSeconds(3f);
        clearData.new_Change();
        SceneManager.LoadScene("MerMaid");
        yield return null;
    }
}
