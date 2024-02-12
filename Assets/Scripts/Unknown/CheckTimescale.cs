using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTimescale : MonoBehaviour
{
    public GameObject[] gameObj;


    // Update is called once per frame
    void Update()
    {
        
        if (gameObj[0].active||gameObj[1].active)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
