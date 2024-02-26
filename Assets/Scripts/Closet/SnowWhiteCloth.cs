using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowWhiteCloth : MonoBehaviour
{
    private string selected;
    [SerializeField] private Sprite[] clothes;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("SnowWhiteClear") == "true")
        {
            GameObject.Find("closetSnow").gameObject
             .transform.GetChild(0).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetString("SnowWhiteCloth") == "true")
        {
            GameObject.Find("Player").gameObject.GetComponent<Image>().sprite = clothes[0];
        }
        if (PlayerPrefs.GetString("MermaidClear") == "true")
        {
            GameObject.Find("closetMermaid").gameObject
             .transform.GetChild(0).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetString("MermaidCloth") == "true")
        {
            GameObject.Find("Player").gameObject.GetComponent<Image>().sprite = clothes[2];
        }
    }

    public void SelectSnow()
    {
        selected = "closetSnow";
    }
    public void SelectMermaid()
    {
        selected = "closetMermaid";
        Debug.Log(selected);
    }
    public void SelectBasic()
    {
        selected = "closetBasic";
    }

    public void PutOnSnowWhiteCloth()
    {
        if (PlayerPrefs.GetString("SnowWhiteClear") == "true"&&selected== "closetSnow")
        {
            GameObject.Find("Player").gameObject.GetComponent<Image>().sprite = clothes[0];
            PlayerPrefs.SetString("SnowWhiteCloth", "true");
        }
    }
    public void PutOnMermaidCloth()
    {
        if (PlayerPrefs.GetString("MermaidClear") == "true" && selected == "closetMermaid")
        {
            GameObject.Find("Player").gameObject.GetComponent<Image>().sprite = clothes[2];
            PlayerPrefs.SetString("MermaidCloth", "true");
        }
    }
    public void PutOnBasicCloth()
    {
        if(selected == "closetBasic")
        {
            GameObject.Find("Player").gameObject.GetComponent<Image>().sprite = clothes[1];
            PlayerPrefs.SetString("SnowWhiteCloth", "false");
        }
    }
}
