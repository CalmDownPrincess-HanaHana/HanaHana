using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MermaidClear : MonoBehaviour
{
    bool giveItemOnce = false;
    public AudioSource[] audio;
    public GameObject[] texts;
    GameObject player;
    Animator anim;
    private SceneChange clearData=new SceneChange();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //²£À»¶§
           /* if (!giveItemOnce)
            {
                
                audio[0].Play();
                Invoke("TearSound", 2.2f);
                Invoke("Cheers", 9f);
                StartCoroutine(ShowTexts());
                PlayerPrefs.SetString("MermaidClear", "true");
                giveItemOnce = true;
            }*/
            //¸ø²£À»¶§
            if (!giveItemOnce)
            {
                //´ê¾ÒÀ» ¶§ ¶ì·Õ¼Ò¸®
                audio[0].Play();
                giveItemOnce = true;
                //ÄÚ·çÆ¾È£Ãâ
                StartCoroutine(NoItem());
            }
        }


    }
    private void TearSound()
    {
        anim.enabled = true;
        anim.SetBool("Clear", true);
        audio[1].Play();
    }
    private void Cheers()
    {
        audio[2].Play();
        audio[3].Play();
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
    }
    IEnumerator NoItem()
    {
        yield return new WaitForSeconds(0.5f);
        anim.enabled = true;
        anim.SetBool("Clear", false);
        yield return new WaitForSeconds(0.5f);
        Swiming();
        yield return new WaitForSeconds(5f);
        GetComponent<ObstacleController>().enabled = false;
        Destroy(GetComponent<MovingController>());
        GetComponent<MovingController>().enabled = false;
        HitPlayer();
  /*      yield return new WaitForSeconds(2.1f);
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
        SceneManager.LoadScene(Define.Scene.SnowWhite.ToString());*/
        yield return null;
    }

    private void Swiming()
    {
        audio[4].Play();
        GetComponent<ObstacleController>().enabled = true;
    }
    private void HitPlayer()
    {
        player.gameObject.SetActive(false);
        this.transform.position = new Vector3(0, 0, 0);
        anim.SetBool("Hit", true);
        audio[5].Play();
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
        GameObject.Find("FadePrefab").GetComponent<FadeInOut>().StartFade = true;
        yield return new WaitForSeconds(3f);
        clearData.new_Change();
        SceneManager.LoadScene(Define.Scene.MerMaid.ToString()); 
        yield return null;
    }
}
