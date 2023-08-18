using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float hit_time=0;
    public SpriteRenderer mirror_renderer;
    [SerializeField]
    public float boss_hp;
    private GameObject bullet;
    private Fire bullet_script;
    [SerializeField]
    private float move_speed;

    private List<Vector3> B_target_positions = new List<Vector3>();
    private int B_current_target_index = 0;

    //���������� ����ȣ��
    //���� �����ϸ� time=0, time+=Time.deltaTime
    //time�� 0�϶��� ������ �ٽ� �ֱ�. & switch �Լ� ȣ��

    //23.05, 0.27: �ʱ���ġ
    private void Awake()
    {
        bullet = GameObject.FindWithTag("bullet").GetComponent<Launch_Fire>().fire;
        bullet_script=bullet.GetComponent<Fire>();

        // ���ϴ� ��ǥ ��ġ���� ����Ʈ�� �߰�
        B_target_positions.Add(new Vector3(23.05f, 0.27f, 0));
        B_target_positions.Add(new Vector3(19.93f, 3.08f, 0));
        B_target_positions.Add(new Vector3(17.52f, 2.07f, 0));
        B_target_positions.Add(new Vector3(18.13f, 0.93f, 0));
        B_target_positions.Add(new Vector3(15.58f, -0.69f, 0));
        B_target_positions.Add(new Vector3(16.46f, -2.62f, 0));
        B_target_positions.Add(new Vector3(18.57f, -1.87f, 0));
        B_target_positions.Add(new Vector3(20.06f, -3.58f, 0));
        B_target_positions.Add(new Vector3(23.48f, -1.96f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        //������ ������ ó��
        hit_time += Time.deltaTime;
        if (hit_time>=0.15f)
            mirror_renderer.color= new Color(1,1,1,0.7f);

        birdPattern();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            mirror_renderer.color = new Color(1, 0.54f, 0.54f, 0.77f);
            boss_hp -= bullet_script.bullet_damage;
            hit_time = 0f;
        }

    }

    private void birdPattern()
    {

        Vector3 targetPosition = B_target_positions[B_current_target_index];
        // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ� ���
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        // ���� �Ÿ� ���� ������ ���� ��ǥ ��ġ�� ����
        if (distanceToTarget <= 0.1f) // ���÷� 0.1f�� ���, ���ϴ� ������ ���� ����
        {
            B_current_target_index = (B_current_target_index+ 1) % B_target_positions.Count;
            targetPosition = B_target_positions[B_current_target_index];
        }

        // ��ǥ ��ġ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * move_speed);
    }
}
