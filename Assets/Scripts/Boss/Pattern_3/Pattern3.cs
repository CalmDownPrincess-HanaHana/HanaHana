using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Pattern3 : MonoBehaviour
{

    [SerializeField] List<GameObject> _patterns;
    [SerializeField] Camera _camera;
    [SerializeField] BossHP _bossHP;

    [SerializeField] GameObject _patternChangeGO;
    [SerializeField] private Animator _bossAnim;
    [SerializeField] private GameObject _fadeOutUI;
    Player _player;
    bool once = false;
    [SerializeField] GameObject startText;
    [SerializeField] GameObject naruto;
    [SerializeField] GameObject snowWhite;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // StartCoroutine(InvertScene());
        StartCoroutine(PatternExecute());
    }

    private void Update()
    {
        if (_bossHP.currentHP <= 0 && (!GameManager.instance.isGameover||_player.player_state!=Define.PlayerState.Damaged)&&!once)
        {
            StartCoroutine(PatternChange());
        }
        
    }

    IEnumerator PatternChange()
    {
        once = true;
        foreach (var pattern in _patterns)
        {
            pattern.SetActive(false);
        }
        
        _player.Invincibility = true;
        _patternChangeGO.SetActive(true);
        
        // ī�޶� shaking
        _camera.transform.DOShakePosition(3f, new Vector3(0.1f, 0.1f, 0));

        // ���� �ִϸ��̼� ����
        _bossAnim.SetBool("isHideEye", true);
        yield return new WaitForSeconds(1.5f);
        naruto.SetActive(true);
        snowWhite.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(LastText());
        yield return new WaitForSeconds(4f);
        _fadeOutUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (SceneManager.GetActiveScene().name == "SnowBoss3")
        {
            SceneManager.LoadScene("SnowBoss4");
        }
    }

    IEnumerator BossRaiseHandAnim()
    {
        _bossAnim.SetBool("isRaiseHand", true);
        yield return new WaitForSeconds(1.0f);
        _bossAnim.SetBool("isRaiseHand", false);
    }
    IEnumerator InvertScene()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            CameraController cameraController = _camera.GetComponent<CameraController>();
            cameraController.isReverse = !cameraController.isReverse;
            _bossAnim.SetBool("isRaiseHand", true);
            yield return new WaitForSeconds(2.0f);
            _bossAnim.SetBool("isRaiseHand", false);
        }
    }

    IEnumerator PatternExecute()
    {
        HashSet< int > executedPatterns = new HashSet<int>();
        for (int i = 0; i < _patterns.Count; i++)
        {
            int rand = Random.Range(0, _patterns.Count);
            if (executedPatterns.Contains(rand))
            {
                i--;
                continue;
            }
            //초반멘트
            if (i == 0)
            {
                StartCoroutine(StartText(rand));
            }
            executedPatterns.Add(rand);
            //다른패턴비활성화 & 2초쉬기
            for (int j= 0; j < _patterns.Count; j++)
            {
                if (j == rand)
                {
                    continue;
                }
                _patterns[j].SetActive(false);
            }
            
            // 게임오버가 되었다면 이후 패턴 진행x
            if (GameManager.instance.isGameover)
            {
                break;
            }
            
            yield return new WaitForSeconds(2f);
            //패턴활성화
            PatternActivate(rand);
            if (rand == 3)//원형패턴
            {
                //3초무적
                StartCoroutine(PlayerInvincibility(3f));
            }
            else
            {
                //1초무적
                StartCoroutine(PlayerInvincibility(1f));
            }
            yield return new WaitForSeconds(14.5f);
            if (rand == 0)//사과면 짠탄제거
            {
                DeleteCloneObjects();
            }
        }
    }

    IEnumerator StartText(int rand)
    {
        //초반멘트        
        startText.SetActive(true);
        if (rand == 3)
        {
            startText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Go to circle!";
        }
        yield return new WaitForSeconds(2f);
        startText.SetActive(false);
    }
    IEnumerator LastText()
    {
        //후반멘트        
        startText.SetActive(true);
        
        startText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "백설이가 하늘로 도망갔어요!";
                
        yield return new WaitForSeconds(2f);
        startText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Now you can fly!";

        yield return new WaitForSeconds(2f);
        startText.SetActive(false);
    }

    void PatternActivate(int patternNum)
    {
        for (int i = 0; i < _patterns.Count; i++)
        {
            if (i == patternNum)
            {
                _patterns[i].SetActive(true);
                continue;
            }
            _patterns[i].SetActive(false);
        }
        StartCoroutine(BossRaiseHandAnim());
        //StartCoroutine(PlayerInvincibility());
    }

    IEnumerator PlayerInvincibility(float time)
    {
        SpriteRenderer playerSprite=_player.GetComponent<SpriteRenderer>();
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0.5f);        
        _player.Invincibility = true;
        yield return new WaitForSeconds(time);
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        _player.Invincibility = false;
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
}
