using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermaidTutorial : MonoBehaviour
{
    public GameObject SaveLoad;
    public GameObject Button;

    // Start is called before the first frame update
    void Start()
    {
        int tutorial_flag = SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("mermaid_tutorial");
        if (tutorial_flag == 0)
        {
            StartCoroutine(StartStory());
        }
        else {
            Destroy(this.gameObject);
        }
    }

    IEnumerator StartStory() {
        Transform prologue = transform.Find("mPrologue");
        Transform prologue2 = transform.Find("mPrologue2");
        Transform prologue3 = transform.Find("mPrologue3");

        if (prologue==null||prologue2 == null || prologue3 == null)
        {
            Debug.LogError("Prologue2 or Prologue3 not found!");
            yield break;
        }

        Button.SetActive(false);

        prologue.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        
        prologue2.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);

        prologue3.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);

        SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("mermaid_tutorial", 1);
        Button.SetActive(true);
        Destroy(this.gameObject);

    }
}
