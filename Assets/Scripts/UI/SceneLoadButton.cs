using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MonoBehaviour
{
    public string sceneName="BossTrainingScene";
    public bool condition = false;
    public enum DataType
    {
        String,
        Float,
        Int
    }
    public string dataName;
    public string dataState;
    public DataType dataType;
    public void goToScene()
    {
        if (!condition)
        {
            SceneManager.LoadScene(sceneName);
        }
        else//조건붙을 때
        {
            switch (dataType)
            {
                case DataType.Float:
                    if (PlayerPrefs.GetFloat(dataName) == Convert.ToSingle(dataState))
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                    break;
                case DataType.Int:
                    if (PlayerPrefs.GetInt(dataName) == Convert.ToInt32(dataState))
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                    break;
                case DataType.String:
                    if (PlayerPrefs.GetString(dataName) == dataState)
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                    break;
            }

        }

    }
}
