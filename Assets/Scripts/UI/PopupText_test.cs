using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText_test : MonoBehaviour
{
    [SerializeField] private PopupText _popupText;
    
    private List<string> text_list1_1 = new List<string>() { "동화나라가 어긋났어요!",  "아이템을 전달해 공주를 진정시키세요!"};
    private List<string> text_list1_2 = new List<string>() {  "그거 말고요.", "아이템을 전달하지 못하면 무시무시한 일이 생길 거예요.", "Good Luck!"};
    private List<string> text_list1_3 = new List<string>() {"튜토리얼이 끝났습니다. 이제 모험을 떠나볼까요?"};

    void Start()
    {
        // 튜토리얼 텍스트 띄우기
        _popupText.PopupTextList(text_list1_1, true);
    }


}
