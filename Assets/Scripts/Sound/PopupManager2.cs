using UnityEngine;
using UnityEngine.UI;

public class PopupManager2 : MonoBehaviour
{
    public GameObject popup2; // 팝업창 게임 오브젝트

    private void Start()
    {   
        
        // 팝업창 초기화
        popup2.SetActive(false);
        
    }

    // 팝업창 활성화
    public void ShowPopup()
    {
        popup2.SetActive(true);

    }

    // 팝업창 닫기
    public void ClosePopup()
    {
        popup2.SetActive(false);

    }

}
