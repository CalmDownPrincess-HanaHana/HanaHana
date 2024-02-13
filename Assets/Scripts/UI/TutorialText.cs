using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] PopupText popup_text_prefab;

    private List<string> text_list1_1 = new List<string>() { "동화나라가 어긋났어요!", "아이템을 전달해 공주를 진정시키세요!" };
    private List<string> text_list1_2 = new List<string>() { "그거 말고요.", "아이템을 전달하지 못하면 무시무시한 일이 생길 거예요.", "Good Luck!" };
    private List<string> text_list1_3 = new List<string>() { "튜토리얼이 끝났습니다. 이제 모험을 떠나볼까요?" };

    public GameObject SaveLoad;
    public GameObject Button;
    public GameObject Camera;
    private GameObject player;
    private Player playerScript;
    public bool isSummaryOver=false;
    private bool isOnce=false;

    void Start()
    {
        int tutorial_flag = SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("tutorial");

        if (tutorial_flag == 1 || tutorial_flag == 3 || tutorial_flag == 4)
        {
            gameObject.SetActive(false);
        }
        else if (tutorial_flag == 0)
        {
            // prologue를 활성화하고 timescale을 0으로 만들기
            Button.SetActive(false);

            // 2초 뒤에 Destroy 호출
            StartCoroutine(ChangePrologueSprite(transform.Find("Prologue").gameObject));
        }
        else if (tutorial_flag == 2)
        {//튜토리얼 가짜 아이템에 닿으면 나오는 텍스트
            popup_text_prefab.PopupTextList(text_list1_2, true);
            SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 3);
        }
    }
    /// <summary>
    /// 맨마지막, flag 닿으면 모든 튜토리얼 진행 사항을 끄고 간다. 
    /// </summary>
    public void TutoEnd()
    {
        popup_text_prefab.PopupTextList(text_list1_3, true);
        SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 4);
        Time.timeScale = 1f;
        Button.SetActive(true);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return null;
        Destroy(gameObject);
    }

    void Update(){
    if(!isOnce&&isSummaryOver){
            playerScript.Invincibility = false;
            popup_text_prefab.PopupTextList(text_list1_1, true);
            SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 1);
            isOnce=true;
        }
    }

    IEnumerator ChangePrologueSprite(GameObject prologue)
    {
        Transform prologue2 = transform.Find("Prologue2");
        Transform prologue3 = transform.Find("Prologue3");

        if (prologue2 == null || prologue3 == null)
        {
            Debug.LogError("Prologue2 or Prologue3 not found!");
            yield break;
        }

        prologue.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        prologue.SetActive(false);

        prologue2.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        Destroy(prologue2.gameObject);

        prologue3.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        Destroy(prologue3.gameObject);

        //위에 프롤로그 컷툰 보여줌.

        Time.timeScale = 0f;
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.Invincibility = true;
            }
            else
            {
                Debug.LogError("Player script not found!");
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }

        Camera.GetComponent<CameraZoomInOut>().enabled = true;
        Camera.GetComponent<MovingController>().enabled = true;
        Camera.GetComponent<CameraController>().enabled = false;
    }
}
