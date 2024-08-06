using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rskillbullet : MonoBehaviour
{
    public float speed = 20f; // 발사체의 속도
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 발사체의 방향으로 속도 설정
            rb.velocity = transform.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Debug.Log("발사체 충돌파괴");
    }
}
