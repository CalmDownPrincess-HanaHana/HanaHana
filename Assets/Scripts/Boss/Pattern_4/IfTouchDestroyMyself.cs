using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 닿으면 자기자신 파괴되게 하는 스크립트
/// </summary>
public class IfTouchDestroyMyself : MonoBehaviour
{
    //누구랑 닿으면 파괴되게 할지 변수넣기
    [SerializeField]
    private string tagName1 = "Untagged";
    [SerializeField]
    private string tagName2 = "Untagged";
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("왜 안 나옴?");
        if (collision.gameObject.CompareTag(tagName1) || collision.gameObject.CompareTag(tagName2))
        {
            Destroy(this.gameObject);
        }
    }

}
