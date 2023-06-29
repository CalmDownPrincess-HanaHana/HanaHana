using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    /// <summary>
    /// 씬 이동
    /// </summary>
    /// <param name="type"></param>
    public void LoadScene(Define.Scene type)
    {
        // 현재 Scene을 Clear()
        CurrentScene.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    /// <summary>
    /// 현재 열려져 있는 씬의 이름을 반환
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
     string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }
}
