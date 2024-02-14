using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomSound : MonoBehaviour
{
    Slider slider;
    [SerializeField] bool effect = false;
    [SerializeField] bool bgm = false;
    AudioSource audio;
    //초기세팅 사운드 값
    float settingSoundValue=1f;

    private void Awake()
    {

        slider = GetComponent<Slider>();
        audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            //초기세팅 값 가져오기
            if (bgm)
            {
                settingSoundValue = PlayerPrefs.GetFloat("bgmValue",1f);
            }
            if (effect)
            {
                settingSoundValue = PlayerPrefs.GetFloat("effectValue", 1f);
            }
            //초기세팅값적용
            audio.volume = settingSoundValue;
        }
        if (slider != null)
        {

            if (bgm)
            {
                slider.value= PlayerPrefs.GetFloat("bgmValue",1f);
            }
            if (effect)
            {
                slider.value = PlayerPrefs.GetFloat("effectValue", 1f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (slider != null)
        {
            if (bgm)
            {
                PlayerPrefs.SetFloat("bgmValue", slider.value);
            }
            if (effect)
            {
                PlayerPrefs.SetFloat("effectValue", slider.value);
            }
        }
        if (audio != null)
        {

            if (bgm)
            {
                //플레이어가 사운드 값 바꿨을 시 반영되도록
                if (settingSoundValue != PlayerPrefs.GetFloat("bgmValue", 1f))
                {
                    settingSoundValue = PlayerPrefs.GetFloat("bgmValue", 1f);
                    audio.volume = settingSoundValue;
                }
            }
            if (effect)
            {
                //플레이어가 사운드 값 바꿨을 시 반영되도록
                if (settingSoundValue != PlayerPrefs.GetFloat("effectValue", 1f))
                {
                    settingSoundValue = PlayerPrefs.GetFloat("effectValue", 1f);
                    audio.volume = settingSoundValue;
                }
            }
        }
    }
}
