using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] PopupText popup_text_prefab;

        private List<string> text_list1_1 = new List<string>() { "동화나라가 어긋났어요!",  "아이템을 전달해 공주를 진정시키세요!"};
        private List<string> text_list1_2 = new List<string>() {  "그거 말고요.", "아이템을 전달하지 못하면 무시무시한 일이 생길 거예요.", "Good Luck!"};
        private List<string> text_list1_3 = new List<string>() {"튜토리얼이 끝났습니다. 이제 모험을 떠나볼까요?"};
        public GameObject SaveLoad;
    
    void Start()
    {
        int tutorial_flag=SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("tutorial");
        Debug.Log("flag"+ tutorial_flag);
        
        if(tutorial_flag==1||tutorial_flag==3){
            gameObject.SetActive(false);
        }

        else if(tutorial_flag==0){
            popup_text_prefab.PopupTextList(text_list1_1, true);
            SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 1);
        }
        
        else if(tutorial_flag==2){//튜토리얼 가짜 아이템에 닿으면 나오는 텍스트
            popup_text_prefab.PopupTextList(text_list1_2, true);
            SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 1);
        }
    }

    public void TutoEnd(){
            popup_text_prefab.PopupTextList(text_list1_3, true);
            SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 1);
            gameObject.SetActive(false);
    }

}
