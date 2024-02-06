using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiatePrefab : MonoBehaviour
{
    public int maxNumber = 1;
    public GameObject prefab;
    string prefabName = "";
    public int existNum = 0;
    // Start is called before the first frame update
       
    void Start()
    {
        prefabName = prefab.name;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (existNum < 3)
        {
            Instantiate(prefab);
            existNum++;
        }

    }

    
}
