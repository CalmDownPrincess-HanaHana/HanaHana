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
    public GameObject exitPanel;
    private Player playerScript;
    public bool isSummaryOver=false;
    private bool isOnce=false;
    private Transform skipButton;

    void Start()
    {
        int tutorial_flag = SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("tutorial");
        skipButton = transform.Find("ButtonSkipCanvas");//튜토리얼 스킵버튼

        if (tutorial_flag == 1 || tutorial_flag == 3 || tutorial_flag == 4)
        {
            gameObject.SetActive(false);
        }
        else if (tutorial_flag == 0)
        {

            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerScript = player.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.Invincibility = true;
                    playerScript.enabled=false;
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
            // prologue를 활성화하고
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

    public void TutoSkip() //스킵 버튼 누룰 시, 맵 훑는 거 멈춤
    {
        Camera.GetComponent<CameraZoomInOut>().enabled = false;
        Destroy( Camera.GetComponent<MovingController>());
        Camera.GetComponent<CameraController>().enabled = true;
        isSummaryOver = true;

    }
    IEnumerator DestroyAfterDelay()
    {
        yield return null;
        Destroy(gameObject);
    }

    void Update(){
        if(!isOnce&&isSummaryOver){
            StartCoroutine(StartTutoText());
        }
    }

    IEnumerator StartTutoText()
    {
        if (isOnce) yield break; // 이미 실행되었다면 더 이상 실행하지 않음
        isOnce = true;
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        skipButton.gameObject.SetActive(false);
        Button.SetActive(true);
        playerScript.enabled = true;
        playerScript.Invincibility = false;
        popup_text_prefab.PopupTextList(text_list1_1, true);
        SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 1);
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

        yield return new WaitForSeconds(1.5f);
        skipButton.gameObject.SetActive(true);

        Camera.GetComponent<CameraZoomInOut>().enabled = true;
        Camera.GetComponent<MovingController>().enabled = true;
        Camera.GetComponent<CameraController>().enabled = false;
    }
}
