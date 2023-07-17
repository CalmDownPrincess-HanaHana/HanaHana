using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] PopupText popup_text_prefab;

    private List<string> text_list1_1 = new List<string>() { "��ȭ���� ��߳����!" };
    private List<string> text_list1_2 = new List<string>() { "�������� ������ ���ָ� ������Ű����!" };
    void Start()
    {
        popup_text_prefab.PopupTextList(text_list1_1, true);
        popup_text_prefab.PopupTextList(text_list1_2, true);
    }

}
