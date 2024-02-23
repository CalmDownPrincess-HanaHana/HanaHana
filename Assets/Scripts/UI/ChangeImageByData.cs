using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageByData : MonoBehaviour
{
    public enum DataType
    {
        String,
        Float,
        Int
    }
    public string dataName;
    public string dataState;
    public DataType dataType ;
    public Sprite changeImage;
    // Start is called before the first frame update
    void Start()
    {
        switch(dataType)
        {
            case DataType.Float:
                if(PlayerPrefs.GetFloat(dataName)== Convert.ToSingle(dataState))
                {
                    GetComponent<Image>().sprite = changeImage;
                }
                break;
            case DataType.Int:
                if (PlayerPrefs.GetInt(dataName) == Convert.ToInt32(dataState))
                {
                    GetComponent<Image>().sprite = changeImage;
                }
                break;
            case DataType.String:
                if (PlayerPrefs.GetString(dataName) == dataState)
                {
                    GetComponent<Image>().sprite = changeImage;
                }
                break;
        }  
    }


    
}

