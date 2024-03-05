using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

/// <summary>
/// 움직이는 obstacle들의 부모 스크립트입니다.
/// </summary>
public class ObstacleController : MonoBehaviour
{
    //추후 define에 넣을 예정
    public enum ObType
    {
        Move, //단방향으로 움직임
        MoveSide, //왔다갔다
        Follow, //player 따라가게
        Rotate, //돌면서 떨어짐
        Shake, //떨리는 거
        ChangeStatus, //레이어랑 tag 바꿈
        ChangeColor, //색깔 바꿈
        Rolling, //굴러감
        ChangeSize,//사이즈 변화
        ChangeRendererOrder,//렌더러순서 변경
        ChangeAnimation, //애니메이션 변경
        ChangeSprite, //스프라이트 변경
        BlowAway,//플레이어 날려버리기
        Destroy,//안보이면삭제
        flipSprite
    }

    public enum ObDirection//★ diagonal이 되는 부분이 있고 아닌 부분이 있음!! 이거 쓸 때 제대로 알고 씁시다. 설명문서에 예전에 추가해두긴 했는데 혹시나 까먹으셨을까봐.. 실수 방지 위해 설명문서 보고 사용하는 습관 들입시다!
    {
        still, //안움직임
        /// <summary>
        /// Up, Down, Left, Right는 simpleMove용
        /// </summary>
        Up,
        Down,
        Left,
        Right,

        /// <summary>
        /// UpDown, LeftRight은 moveSide랑 Shake용
        /// </summary>
        UpDown,
        LeftRight,

        /// <summary>
        /// Rolling용 diagonal
        /// </summary>
        Diagonal_Left_Up,
        Diagonal_Right_Up,
        Diagonal_Left_Down,
        Diagonal_Right_Down
    }

    public enum IntoColor
    {
        TransParent,
        Opaque
    }

    public enum changeSize
    {
        Bigger,
        Smaller
    }

    public enum RendererOrder
    {
        Def,
        Mostback
    }
    [SerializeField]
    private bool isOpposite = false; //moveside에서 왼쪽이랑 아래쪽으로 먼저 가는지

    [SerializeField]
    private ObType obType; //obstacle의 type을 inspector에서 받아옴.
    [SerializeField]
    private ObDirection obDirection; // direction을 inspector에서 받아옴. (필요할시)
    [SerializeField]
    private float gravity_scale = 4f;//중력스케일 조절

    [SerializeField]
    private bool isMovingPlatform = false; //만약 움직이는 발판이라면 //★이름모호. isMovingPlatform 추천


    [SerializeField]
    private IntoColor color; // into Change할 color결정

    [SerializeField]
    private changeSize size; // into Change할 size결정

    [SerializeField]
    private RendererOrder rend_order; // into Change할 render order결정


    [SerializeField]
    private bool isCol = false; //트리거로 작동하는지 collision으로 작동하는지

    [SerializeField]
    private bool isMoving = false; //시작부터 움직이이려면 true 체크
    public bool IsMoving//캡슐화
    {
        set
        {
            isMoving = value;
        }
    }

    [SerializeField]
    private float waitingTime = 0f;

    [SerializeField]
    private float distance = 0f; //움직일 거리, SimpleMove(얼마나 움직일지) 랑 Shake(얼마만큼 왔다갔다할지)에서 사용  

    [SerializeField]
    private float speed = 11f; //속도

    [SerializeField]
    private string tagName = "Untagged";

    [SerializeField]
    private int layerIndex = 0;

    [SerializeField]
    private Define.Tags colTag = Define.Tags.Player; //player가 default

    [SerializeField]
    private string animName;

    [SerializeField]
    private Sprite img;

    private SpriteRenderer spriteRenderer;

    private Vector3 initialPosition; //움직인 거리를 재기 위해 사용
    private Vector3 movement = Vector3.zero;

    private float movedDistance = 0f;
    [SerializeField]
    private float rotateSpeed = 10000f;

    private Rigidbody2D rigid;
    private Tilemap tilemap;
    private Animator anim;

    //색변환 위한 변수
    private Renderer _renderer;
    private Color my_color;
    private bool is_expired = false;
    private bool is_start = true;

    //사이즈 위한 변수
    private Vector3 transform_scale;

    //플레이어
    private GameObject player;

    //파괴함수에만 쓰이는 변수
    private bool destroy = false;

    /// <summary>
    /// isTriggered 처리가 된 collider와 부딪혔을때 
    /// 부딪혔다는 사실을 isTriggred로 알리고, tag도 부딪히면 죽게 Enemy로 바꿔줍니다.
    /// 만약 isTriggred 처리된 collider를 사용하고 싶다면 주의해서 사용해주셔야해요.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCol && collision.gameObject.CompareTag(colTag.ToString()))
        {
            StartCoroutine(SetIsmoving(true));//★
        }
    }

    /// <summary>
    /// 트리거 없이 collisoin에 부딪혓을때 작동하는 경우.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(colTag.ToString()))
        {
            if (isCol)
                StartCoroutine(SetIsmoving(true));
            if (isMovingPlatform)
            {
                collision.transform.SetParent(transform);
            }
            isCol = false;//★ false하는 이유는 한번 작동하고 끄려고? ok.
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isMovingPlatform && collision.transform.CompareTag(colTag.ToString()))
        {
            collision.transform.SetParent(null);
        }
    }



    private void Awake()//★
    {
        initialPosition = transform.position;
        tilemap = GetComponent<Tilemap>();
        _renderer = GetComponent<Renderer>();

        //불투명으로 바뀌게 하는거면 시작부터 투명하게
        if (obType == ObType.ChangeColor && color == IntoColor.Opaque)
        {
            my_color = new Color(0f, 0f, 0f, 0f);
            _renderer.material.color = my_color;
        }
        else if (obType == ObType.Follow)
        {
            rigid = GetComponent<Rigidbody2D>();
        }


    }

    private void FixedUpdate()
    {
        if (isMoving)//★
        {
            initialPosition = transform.position;
            ObstacleMove(obType);//isMoving 이면 동작타입에 따라 동작.
        }
    }


    private void ObstacleMove(ObType obType)//★핵심. 코루틴 수행 후 isMoving에 false대입해 멈춰주고있음
    {
        switch (obType)
        {
            case ObType.Move:
                StartCoroutine(MoveToTarget());
                isMoving = false;
                break;
            case ObType.MoveSide: //shake로 대체 가능 나중에 rigidbody로 바뀌어서 enemy로 옮겨갈 예정...
                //MoveSide();
                StartCoroutine(MoveSideCoroutine());
                isMoving = false;
                break;
            case ObType.Rotate:
                Rotate();//★isMoving false가 없는애도 간혹있음. 화면에 보이는동안 계속 돌아가야하니까. 근데 shake처럼 코루틴으로 만들고, 반복문에 넣고 isMoving false가 나아보임
                break;
            case ObType.Shake:
                StartCoroutine(ShakeCoroutine());
                isMoving = false;//★얘도 코루틴문 자체에서 반복문 돌기 때문에 isMoving false처리
                break;
            case ObType.ChangeStatus:
                ChangeStatus(tagName, layerIndex);
                isMoving = false;
                break;
            case ObType.ChangeColor:
                ChangeColor(color);//★isMoving false없는 애 2 . 동작원리 : changeColor함수 자체에서는 반복문이없지만, ObstacleMove가 update에서 계속 불려서 반복문의 효과.
                break;//반복문을 계속 돌려주기 위해 isMoving false처리를 안했지만, 얘도 코루틴 자체에서 반복문 돌게하고 isMoving = false처리가 나아보임
            case ObType.Rolling:
                Rolling(obDirection, speed, gravity_scale);//★isMoving false처리 안한 애 3. isMoving=false를 함수 안에서 해주고 있기때문                
                break;
            case ObType.ChangeSize:
                ChangeSize(size);//★ isMoving=false를 함수 안에서 해 주고 있음
                break;
            case ObType.ChangeRendererOrder:
                ChangeRendOrder(rend_order);
                isMoving = false;//★ 추가한 코드 . isMoving=false가 없었음. 유진이 스크립트파악 미숙할 때 짰던거라 
                break;
            case ObType.ChangeSprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                ChangeSprite();
                isMoving = false;
                isCol = true;
                break;
            case ObType.ChangeAnimation:
                anim = GetComponent<Animator>();
                ChangeAnimation();
                isMoving = false;
                break;
            case ObType.BlowAway:
                BlowAway(obDirection);//isCol로 판단. ★collider에 닿는동안 계속 불어줘야하는 애여서 isMoving=false처리안함. 반복문 효과를 내기위해서. changeColor와 같음
                isCol = true;
                break;
            case ObType.Destroy:
                destroy = true;
                isMoving = false;//★true였던걸 false로 수정. destroy=true하면 OnBecameInvisible에서 destroy함. OnBecameInvisible자체가 업뎃문처럼 계속 체크하는거기 때문에 isMoving=false여도 됨
                break;
            case ObType.flipSprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                FlipSprite();
                isMoving = false;//★없던 코드 추가 .
                break;
        }

        if (this.gameObject.GetComponent<BoxCollider2D>())//★BoxCollider는 트리거로만 씀. 이거 기억 나시나요? 트리거로만 쓰자고했던거. 이거 까먹을 것 같으니 여기 제대로 써두고 그 이후는 ObstacleController 수정 절대 하지말고 냅둡시다
        {
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());//그래서 트리거 끝나면 destroy..
        }

    }

    private void OnBecameInvisible()
    {
        if (destroy)
        {
            Destroy(this.gameObject);
        }
    }
    private void FlipSprite()
    {
        if (obDirection == ObDirection.LeftRight)
        {
            spriteRenderer.flipX = true;
        }
        else if (obDirection == ObDirection.UpDown)
        {
            spriteRenderer.flipY = true;
        }
    }
    private void ChangeSprite()
    {
        spriteRenderer.sprite = img;
    }

    private void BlowAway(ObDirection obDirection)
    {

        Rigidbody2D player_rigid = player.GetComponent<Rigidbody2D>();
        switch (obDirection)
        {
            case ObDirection.Up:
                player_rigid.AddForce(new Vector2(0, speed));
                isMoving = false;
                break;
        }

    }
    private void ChangeAnimation()
    {
        anim.SetBool(animName, true);
    }

    private void ChangeRendOrder(RendererOrder rend_order)
    {
        switch (rend_order)
        {
            case RendererOrder.Def:
                _renderer.sortingOrder = 0;
                break;
            case RendererOrder.Mostback:
                _renderer.sortingOrder = 1;
                break;
        }
    }
    private void ChangeSize(changeSize size)
    {
        if (!is_expired)
        {
            switch (size)
            {
                case changeSize.Bigger:
                    if (this.gameObject.transform.localScale.x >= 100 || this.gameObject.transform.localScale.y >= 100 || this.gameObject.transform.localScale.z >= 100)
                    {
                        this.gameObject.transform.localScale = new Vector3(100, 100, 100);
                        is_expired = true;
                    }
                    transform_scale.x += 0.5f;
                    transform_scale.y += 0.5f;
                    transform_scale.z += 0.5f;
                    this.gameObject.transform.localScale = transform_scale;
                    break;

                case changeSize.Smaller:
                    if (this.gameObject.transform.localScale.x <= 0 || this.gameObject.transform.localScale.y <= 0 || this.gameObject.transform.localScale.z <= 0)
                    {
                        this.gameObject.transform.localScale = new Vector3(0, 0, 0);
                        is_expired = true;
                    }
                    else
                    {
                        transform_scale.x -= 0.5f;
                        transform_scale.y -= 0.5f;
                        transform_scale.z -= 0.5f;
                        this.gameObject.transform.localScale = transform_scale;
                    }
                    break;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// ~로 굴러감
    /// </summary>
    private void Rolling(ObDirection obDirection, float speed, float gravity_scale)
    {

        if (is_start)
        {
            rigid = GetComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.gravityScale = gravity_scale;

            is_start = false;
            switch (obDirection)
            {
                case ObDirection.Left:
                    rigid.velocity = new Vector3(-speed, 0, 0);
                    break;
                case ObDirection.Right:
                    rigid.velocity = new Vector3(speed, 0, 0);
                    break;
                case ObDirection.Diagonal_Left_Up:
                    rigid.AddForce(new Vector2(-1, 1) * speed, ForceMode2D.Impulse);
                    break;
                case ObDirection.Diagonal_Right_Up:
                    rigid.AddForce(new Vector2(1, 1) * speed, ForceMode2D.Impulse);
                    break;

            }
        }
        else
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// ~로 색 바꾸기
    /// </summary>
    /// <param name="color"></param>
    private void ChangeColor(IntoColor color)
    {
        if (is_start)
        {
            _renderer = GetComponent<Renderer>();
            switch (color)
            {
                case IntoColor.TransParent:
                    _renderer.material.color = new Color(1f, 1f, 1f, 1f);
                    my_color = new Color(1f, 1f, 1f, 1f);
                    break;
                case IntoColor.Opaque:
                    _renderer.material.color = new Color(1f, 1f, 1f, 0f);
                    my_color = new Color(1f, 1f, 1f, 0f);
                    break;
            }
            is_start = false;
        }

        if (!is_expired)
        {
            switch (color)
            {
                case IntoColor.TransParent:
                    my_color.a -= 0.05f; // 알파 값 조정
                    // 알파 값이 0 이하면 0으로 고정
                    if (my_color.a <= 0f)
                    {
                        my_color.a = 0f;
                        is_expired = true;
                    }
                    // 오브젝트의 머티리얼 또는 스프라이트 렌더러의 색상 설정
                    _renderer.material.color = my_color;
                    break;
                case IntoColor.Opaque:
                    my_color.a += 0.05f; // 알파 값 조정
                    // 알파 값이 1을 넘어가면 1로 고정
                    if (my_color.a >= 1f)
                    {
                        my_color.a = 1f;
                        is_expired = true;
                        isMoving = false;
                    }
                    // 오브젝트의 머티리얼 또는 스프라이트 렌더러의 색상 설정
                    _renderer.material.color = my_color;
                    break;
            }
        }
    }
    /// <summary>
    /// 이제 주어진 방향, 속도, 거리만큼 obstacle이 움직입니다. 
    /// </summary>
    private IEnumerator MoveToTarget()
    {
        Vector3 targetPosition = CalculateTargetPosition(obDirection, distance);

        while (movedDistance < distance)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            movedDistance = Vector3.Distance(initialPosition, newPosition);
            transform.position = newPosition;
            yield return null;
        }
    }

    private Vector3 CalculateTargetPosition(ObDirection obDirection, float movement)
    {
        //대각선방향: 중력영향 받아서는 안 됨.
        //out: 포인터
        if (obDirection == ObDirection.Diagonal_Left_Up || obDirection == ObDirection.Diagonal_Right_Up || obDirection == ObDirection.Diagonal_Right_Down || obDirection == ObDirection.Diagonal_Left_Down)
        {
            rigid = GetComponent<Rigidbody2D>();
            if (rigid != null && rigid.bodyType == RigidbodyType2D.Dynamic)
            {
                rigid.bodyType = RigidbodyType2D.Static;
            }
        }

        switch (obDirection)
        {
            case ObDirection.Up:
                return initialPosition + Vector3.up * movement;
            case ObDirection.Down:
                return initialPosition + Vector3.down * movement;
            case ObDirection.Left:
                return initialPosition + Vector3.left * movement;
            case ObDirection.Right:
                return initialPosition + Vector3.right * movement;
            case ObDirection.Diagonal_Left_Up:
                return initialPosition + new Vector3(-1, 1, 0) * movement;
            case ObDirection.Diagonal_Right_Up:
                return initialPosition + new Vector3(1, 1, 0) * movement;
            case ObDirection.Diagonal_Left_Down:
                return initialPosition + new Vector3(-1, -1, 0) * movement;
            case ObDirection.Diagonal_Right_Down:
                return initialPosition + new Vector3(1, -1, 0) * movement;
            default:
                return initialPosition;
        }
    }



    /// <summary>
    /// 회전
    /// </summary>
    private void Rotate()
    {
        transform.Rotate(0, 0, -Time.deltaTime * rotateSpeed, Space.Self);
    }

    /// <summary>
    /// 위아래로 움직이는데 움직이는데 속도 차이 때문에 둥둥 떠있는것처럼 보임. 기획자분들한테 맡길게요.
    /// </summary>
    private IEnumerator ShakeCoroutine()
    {

        while (true)
        {
            //만약 좌우로 움직이게 하고 싶으면
            float newX = (obDirection == ObDirection.LeftRight) ? initialPosition.x + Mathf.Sin(Time.time * speed) * distance : transform.position.x;
            //만약 상하로 움직이게 하고 싶으면
            float newY = (obDirection == ObDirection.UpDown) ? initialPosition.y + Mathf.Sin(Time.time * speed) * distance : transform.position.y;
            transform.position = new Vector3(newX, newY, transform.position.z);
            yield return null;
        }
    }

    /// <summary>
    /// 일정한속력으로 왔다갔다거림. 단, 한쪽 끝에서 시작하는 것만 가능함 ex. 중간에서 시작해 왼쪽 갔다가 오른쪽 가는게 안됨. 맨 왼쪽에서 시작할 수 밖에 없음.
    /// </summary>
    private IEnumerator MoveSideCoroutine()
    {
        float nowTime = Time.time;
        while (true)
        {
            // 시간에 따라 이동할 거리 계산
            float moveDistance = Mathf.PingPong((Time.time - nowTime) * speed, distance);
            float direction = (isOpposite) ? -1 : 1;

            // 좌우로 움직이는 경우
            if (obDirection == ObDirection.LeftRight)
            {
                float newX = initialPosition.x + moveDistance * direction;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
            // 상하로 움직이는 경우
            else if (obDirection == ObDirection.UpDown)
            {
                float newY = initialPosition.y + moveDistance * direction;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }

            yield return null;
        }
    }

    //tag 와 layer을 변경
    private void ChangeStatus(string tagName, int layerIndex)
    {
        this.gameObject.tag = tagName;
        this.gameObject.layer = layerIndex;
    }

    IEnumerator SetIsmoving(bool n)
    {
        yield return new WaitForSeconds(waitingTime);//★waiting time설정한 만큼 기다리고
        this.isMoving = n;//인자로 넣은 bool 값 넣어주고. 
    }



}
