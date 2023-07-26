using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlag : MonoBehaviour
{
    public GameObject SaveLoad;
    public GameObject TutorialText;
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player"))
        {
            if(SaveLoad.GetComponent<SaveLoad>().LoadDeathCount("tutorial")==1){
                TutorialText.GetComponent<TutorialText>().TutoEnd();
                SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 3);
            }   
        }
    }
}