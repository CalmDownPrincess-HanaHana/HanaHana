using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteController : ParentObstacleController
{
    public enum ObType
    {
        ChangeAnimator,//애니메이터변경
        ChangeRendererOrder,//렌더러순서 변경
        ChangeSprite,//스프라이트변경
        FlipSprite,//플립
        StartAnimation,//애니메이션 한번 호출. 더 많이 하려면 스크립트 수정해야함. 현민에게 문의
        FlipTile//타일용플립
        //애초에 타일맵은 애니메이터 존재 X 타일맵은 애니메이션 바꾸려면 그냥 타일맵을 하나 삭제하고 새로운 걸 나오게 하든지 해야함
    
    }
    public enum RendererOrder
    {
        Default,
        Layer1,
        Layer2,
        Layer3,
        UI
    }
    public enum FlipType
    {
        X,
        Y
    }

    private Renderer renderer;
    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;
    private Vector3Int cellPosition;
    [SerializeField] private ObType obType;

    /// <summary>
    /// ChangeAnimator변수
    /// </summary>
    [SerializeField] private Animator anim;
    [SerializeField] private string animName; //실행시킬 애니메이션 이름
    [SerializeField] string path;
    /// <summary>
    /// RendererOrder변수
    /// </summary>
    [SerializeField] private RendererOrder rendererOrder;
    [SerializeField] private int orderNumber = 0;
    /// <summary>
    /// ChangeSprite 변수
    /// </summary>

    [SerializeField] Sprite inputImage;
    /// <summary>
    /// FlipSprite변수
    /// </summary>
    [SerializeField] private FlipType flipType;
    public override IEnumerator Activate()
    {
        switch (obType)
        {
            case ObType.ChangeAnimator:
                StartCoroutine(ChangeAnimator(anim, path));
                break;
            case ObType.StartAnimation:
                StartCoroutine(StartAnimation(anim, animName));
                break;
            case ObType.ChangeRendererOrder: //타일맵, 스프라이트 모두 가능
                StartCoroutine(ChangeRendererOrder());
                break;
            case ObType.ChangeSprite:
                StartCoroutine(ChangeSprite());
                break;
            case ObType.FlipSprite:
                StartCoroutine(FlipSprite());
                break;
            case ObType.FlipTile:
                StartCoroutine(FlipTile());
                break;
        }
        yield return base.Activate(); // 부모 클래스의 Activate 메서드 실행 
        //사실 ismoving과 별개로 움직이기 때문에 이걸 굳이 부모 activate를 실행하지 않아도 되지만 후에 test할때를 위해 그냥 실행하겠음. 
    }


    /// <summary>
    /// 애니메이터 바꿔주기
    /// path: 경로명
    /// anim: 바꿔줄 오브젝트의 애니메이터
    /// </summary>
    IEnumerator ChangeAnimator(Animator anim, string path)
    {
        path = path.Replace("Assets/Resources/", "");
        path = path.Replace(".controller", "");
        anim.runtimeAnimatorController =
            (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(path,
                typeof(RuntimeAnimatorController)));
        yield return null;
    }

    IEnumerator StartAnimation(Animator anim, string animName)
    {
        anim.Play(animName, 0, 0f);
        yield return null;
    }

    IEnumerator ChangeRendererOrder()
    {
        switch (rendererOrder)
        {
            case RendererOrder.Default:
                renderer.sortingLayerName = "Default";
                break;
            case RendererOrder.Layer1:
                renderer.sortingLayerName = "Layer1";
                break;
            case RendererOrder.Layer2:
                renderer.sortingLayerName = "Layer2";
                break;
            case RendererOrder.Layer3:
                renderer.sortingLayerName = "Layer3";
                break;
            case RendererOrder.UI:
                renderer.sortingLayerName = "UI";
                break;
        }

        renderer.sortingOrder = orderNumber;

        yield return null;
    }
    IEnumerator FlipTile()
    {
        Tilemap tilemap=this.gameObject.GetComponent<Tilemap>();
        tilemap.orientation = Tilemap.Orientation.Custom;
        // 현재의 orientationMatrix를 가져옵니다.
        Matrix4x4 matrix = tilemap.orientationMatrix;
        
        // 원하는 각도로 회전 행렬을 구성합니다.
        Matrix4x4 rotationMatrix=new Matrix4x4();
        if (flipType == FlipType.X)
        {
            rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 180f, 0f));
        }
        if (flipType == FlipType.Y)
        {
            rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 180f, 180f));
        }

        // 회전된 행렬을 현재의 orientationMatrix에 곱하여 새로운 orientationMatrix를 얻습니다.
        matrix = rotationMatrix * matrix;
        // 새로운 orientationMatrix를 Tilemap에 설정합니다.
        tilemap.orientationMatrix = matrix;

        yield return null;
    }
    IEnumerator ChangeSprite()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            spriteRenderer.sprite = inputImage;
        }

        yield return null;
    }
    IEnumerator FlipSprite()
    {
        if (flipType == FlipType.X)
        {
            spriteRenderer.flipX = true;
        }
        else if (flipType == FlipType.Y)
        {
            spriteRenderer.flipY = true;
        }
        yield return null;
    }

    private void Awake()
    {
        base.Awake();
        renderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
