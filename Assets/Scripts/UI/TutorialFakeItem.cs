using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFakeItem : MonoBehaviour
{
    public GameObject SaveLoad;
    
    private void OnCollisionEnter2D(Collision2D collision){
        SaveLoad.GetComponent<SaveLoad>().SaveDeathCount("tutorial", 2);
    }
}