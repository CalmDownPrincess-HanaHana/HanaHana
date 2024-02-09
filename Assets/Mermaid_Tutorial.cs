using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mermaid_Tutorial : MonoBehaviour
{
    public GameObject SaveLoad;
    public GameObject Button;
    void Start()
    {
        int deathCount=SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("mermaid_tutorial");
        if (deathCount == 0) {
            // this.object의 child인 prologue를 찾아서 처리
            Transform prologue = transform.Find("Prologue");

            if (prologue != null)
            {
                // prologue를 활성화하고 timescale을 0으로 만들기
                Button.SetActive(false);
                prologue.gameObject.SetActive(true);
                Time.timeScale = 0f;

                // 2초 뒤에 Destroy 호출
                StartCoroutine(DestroyAfterDelay(prologue.gameObject, 5f));
            }
        }
    }

    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(obj);
        SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("mermaid_tutorial", 1);
        Button.SetActive(true);
        Time.timeScale = 1f;
    }
}
