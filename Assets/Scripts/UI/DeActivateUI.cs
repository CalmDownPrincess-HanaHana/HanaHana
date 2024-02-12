using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeActivateUI : MonoBehaviour
{
    public GameObject gameObj;
    public void Activate()
    {
        Debug.Log("호출");
        gameObj.SetActive(true);
    }
    public void Deacitvate()
    {
        Debug.Log("끄기");
        gameObj.SetActive(false);
    }
}
