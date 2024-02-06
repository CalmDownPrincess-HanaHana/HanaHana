using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowFish : MonoBehaviour
{
    Renderer renderer;
    AudioSource audio;
    float timer=0f;
    bool biggerTrigger = false;
    InitiatePrefab initiatePrefab;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        audio = GameObject.Find("PopSound").GetComponent<AudioSource>();
        this.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f,3f),0f);
        renderer.material.color = new Color(1, 1, 1, 0.5f);
        initiatePrefab = GameObject.Find("Initiate").GetComponent<InitiatePrefab>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!biggerTrigger)
        {
            timer += Time.deltaTime;
            if (timer >= 1.5f)
            {
                renderer.material.color = new Color(1f, 1f, 1f, 1f);
                biggerTrigger = true;
                this.tag = "Enemy";
                this.gameObject.layer = 8;
            }
        }
        else 
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + Time.deltaTime, this.transform.localScale.y + Time.deltaTime, 0f);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (biggerTrigger&&(collision.gameObject.name == "Missile"||collision.gameObject.CompareTag("Player")))
        {   
            //소리재생
            audio.Play();
            //존재하는 복어숫자 --
            initiatePrefab.existNum--;
            //본인파괴
            Destroy(this.gameObject);       
        }
    }
}
