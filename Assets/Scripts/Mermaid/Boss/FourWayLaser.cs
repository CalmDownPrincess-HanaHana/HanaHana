using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourWayLaser : MonoBehaviour
{
    /// <summary>
    /// 해당 프리팹을 4방면으로 쏘는 스크립트
    /// </summary>
    [SerializeField] private GameObject objPrefab; // 발사할 프리팹
    [SerializeField] private float force = 40f; // 발사되는 힘

    void Start()
    {
        SpewLaser();
    }

    private void SpewLaser()
    {
        Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

        foreach (Vector3 direction in directions)
        {
            ShootLaserInDirection(direction);
        }
        Destroy(this.gameObject);
    }

    private void ShootLaserInDirection(Vector3 direction)
    {
        GameObject objInstance = Instantiate(objPrefab, transform.position, Quaternion.identity);
        Rigidbody2D objRigidbody = objInstance.GetComponent<Rigidbody2D>();

        if (objRigidbody != null)
        {
            objRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
