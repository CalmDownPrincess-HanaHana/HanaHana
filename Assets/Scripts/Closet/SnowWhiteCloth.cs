using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowWhiteCloth : MonoBehaviour
{
    private string snowSelected;
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
        snowSelected = "closetSnow";
    }
    public void SelectMermaid()
    {
        snowSelected = "closetMermaid";
    }
    public void SelectBasic()
    {
        snowSelected = "closetBasic";
    }

    public void PutOnSnowWhiteCloth()
    {
        if (PlayerPrefs.GetString("SnowWhiteClear") == "true"&&snowSelected== "closetSnow")
        {
            GameObject.Find("Player").gameObject.GetComponent<Image>().sprite = clothes[0];
            PlayerPrefs.SetString("SnowWhiteCloth", "true");
        }
    }
    public void PutOnMermaidCloth()
    {
        if (PlayerPrefs.GetString("MermaidClear") == "true" && snowSelected == "closetMermaid")
        {
            GameObject.Find("Player").gameObject.GetComponent<Image>().sprite = clothes[2];
            PlayerPrefs.SetString("MermaidCloth", "true");
        }
    }
    public void PutOnBasicCloth()
    {
        if(snowSelected == "closetBasic")
        {
            GameObject.Find("Player").gameObject.GetComponent<Image>().sprite = clothes[1];
            PlayerPrefs.SetString("SnowWhiteCloth", "false");
        }
    }
}
