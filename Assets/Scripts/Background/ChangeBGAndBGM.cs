using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBGAndBGM : MonoBehaviour
{
    public enum SceneType
    {
        None,
        SnowWhite,
        EndingScene,
        MerMaid
    }

    [SerializeField] Transform _transform; 
    [SerializeField] SceneType sceneName;
    [SerializeField] GameObject Sky;
    [SerializeField] Sprite[] backgrounds;
    [SerializeField] AudioSource[] audioSources = null;
    //초기세팅 사운드 값
    float settingSoundValue;
    private void Awake()
    {
        //초기세팅 브금값 가져오기
        settingSoundValue = PlayerPrefs.GetFloat("bgmValue");
        //초기세팅값적용
        foreach (AudioSource audio in audioSources)
        {
            audio.volume = settingSoundValue;
        }
    }
    // Update is called once per frame
    void Update()
    { 
        //플레이어가 사운드 값 바꿨을 시 반영되도록
        if (settingSoundValue != PlayerPrefs.GetFloat("bgmValue"))
        {
            settingSoundValue = PlayerPrefs.GetFloat("bgmValue");
            foreach(AudioSource audio in audioSources)
            {
                audio.volume = settingSoundValue;
            }
        }
        switch (sceneName) {
            case SceneType.None:
                return;
            case SceneType.MerMaid:
                if(_transform.position.x< 219)
                {
                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[0];
                    if (audioSources != null)
                    {
                        audioSources[0].enabled = true;
                        audioSources[1].enabled = false;
                        audioSources[2].enabled = false;
                    }
                }
                if (_transform.position.x > 219&& _transform.position.x < 448)
                {
                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[1];
                    if (audioSources != null)
                    {
                        audioSources[0].enabled = false;
                        audioSources[1].enabled = true;
                        audioSources[2].enabled = false;
                    }
                }
                if (_transform.position.x > 448)
                {
                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[2];
                    if (audioSources != null)
                    {
                        audioSources[0].enabled = false;
                        audioSources[1].enabled = false;
                        audioSources[2].enabled = true;
                    }
                }
                return;
            case SceneType.SnowWhite:
                if (_transform.position.x < 382) { 
                
                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[0];
                    if (audioSources != null)
                    {
                        audioSources[0].enabled = true;
                        audioSources[1].enabled = false;
                        audioSources[2].enabled = false;
                        audioSources[3].enabled = false;
                    }
                }

                if (_transform.position.x >= 382 && _transform.position.x < 650)
                {
                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[1];
                    if (audioSources != null)
                    {
                        audioSources[1].enabled = true;
                        audioSources[2].enabled = false;
                        audioSources[0].enabled = false;
                        audioSources[3].enabled = false;
                    }
                }

                if (_transform.position.x >= 650)
                {
                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[2];
                    if (audioSources != null)
                    {
                        audioSources[2].enabled = true;
                        audioSources[0].enabled = false;
                        audioSources[1].enabled = false;
                        audioSources[3].enabled = true;
                    }

                }
                break;
                case SceneType.EndingScene:
                audioSources[0].volume += 0.01f;
                if (_transform.position.x < 382)
                {

                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[0];
                }

                if (_transform.position.x >= 382 && _transform.position.x < 650)
                {
                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[1];
                }

                if (_transform.position.x >= 650)
                {
                    Image sky_image = Sky.GetComponent<Image>();
                    sky_image.sprite = backgrounds[2];
                }

                break;
        }

    }
}
