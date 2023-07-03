using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    [SerializeField] private List<string> text_list;
    [SerializeField] private TextMeshProUGUI popupText_txt;
    [SerializeField] private int currentText = 0;
    void Awake()
    {
        string tuto_text0 = "공주님이 어쩐일인지 화가 잔뜩 났어요!";
        string tuto_text1 = "아이템을 전달해 공주를 진정시키세요.";
        string tuto_text2 = "그거말고요.";
        string tuto_text3 = "아이템을 전달 못하면 무시무시한 일이 생길거예요!";
        string tuto_text4 = "세이브 포인트로 저장하세요!";
        string tuto_text5 = "Good Luck!";
        
        text_list.Add(tuto_text0);
        text_list.Add(tuto_text1);
        text_list.Add(tuto_text2);
        text_list.Add(tuto_text3);
        text_list.Add(tuto_text4);
        text_list.Add(tuto_text5);
    }

    private void Start()
    {
        WriteText(text_list[0]);
    }

    public void WriteText(string text)
    {
        popupText_txt.text = text;
    }
}
