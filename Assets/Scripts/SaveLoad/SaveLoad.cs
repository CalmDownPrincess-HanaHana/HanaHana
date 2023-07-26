using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public void SaveDeathCount(string key, int count)
    {
        PlayerPrefs.SetInt(key, count);
    }

    public int LoadDeathCount(string key)
    {
        int count=PlayerPrefs.GetInt(key, 0);
        return count;

    }
    
    public void SaveRespawn(string key, Vector3 value)
    {
        PlayerPrefs.SetFloat(key+"X", value.x);
        PlayerPrefs.SetFloat(key+"Y", value.y);
        PlayerPrefs.SetFloat(key+"Z", value.z);

    }

    public Vector3 LoadRespawn(string key)
    {
        Vector3 v3=Vector3.zero;
        v3.x=PlayerPrefs.GetFloat(key+"X");
        v3.y=PlayerPrefs.GetFloat(key+"Y");
        v3.z=PlayerPrefs.GetFloat(key+"Z");
        return v3;
    }
}