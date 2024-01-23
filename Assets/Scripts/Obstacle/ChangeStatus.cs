using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Change this.gameObject into assigned tag and layerIndex*/
public class ChangeStatus : MonoBehaviour
{
    [SerializeField]
    private string tagName="Untagged"; //gameObject will change into this tag
    [SerializeField]
    private int layerIndex=0;//gameObject will change into this layerIndex

    public 

    void Start()
    {
        ChangeStatusTo(tagName, layerIndex);
    }

    private void ChangeStatusTo(string tagName, int layerIndex) 
    {
        this.gameObject.tag = tagName;
        this.gameObject.layer = layerIndex;
    }


}
