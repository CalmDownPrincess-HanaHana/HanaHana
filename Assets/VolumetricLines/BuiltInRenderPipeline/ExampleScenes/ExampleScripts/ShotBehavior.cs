﻿using System.Collections;
using UnityEngine;

public class ShotBehavior : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 1000f;

    }
}
