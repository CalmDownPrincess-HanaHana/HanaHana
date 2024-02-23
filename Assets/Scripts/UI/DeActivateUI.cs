using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeActivateUI : MonoBehaviour
{
    public GameObject gameObj;
    public void Activate()
    {
        gameObj.SetActive(true);
    }
    public void Deacitvate()
    {
        gameObj.SetActive(false);
    }
}
