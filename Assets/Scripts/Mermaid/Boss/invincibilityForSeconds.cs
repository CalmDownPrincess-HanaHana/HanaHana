using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invincibilityForSeconds : ParentObstacleController
{
    Renderer renderer;
    [SerializeField] private string beforeTag = "Untagged";
    [SerializeField] private int beforelayer = 10;
    [SerializeField] private string afterTag = "Enemy";
    [SerializeField] private int afterlayer= 8;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
        this.tag = beforeTag;
        this.gameObject.layer = beforelayer;
    }
    public override IEnumerator Activate()
    {
        renderer.material.color = new Color(1f, 1f, 1f, 1f);
        this.gameObject.layer = afterlayer;
        this.tag = afterTag;
        yield return base.Activate(); // 부모 클래스의 Activate 메서드 실행 
        //사실 ismoving과 별개로 움직이기 때문에 이걸 굳이 부모 activate를 실행하지 않아도 되지만 후에 test할때를 위해 그냥 실행하겠음. 
    }
}
