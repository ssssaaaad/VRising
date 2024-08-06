using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eskillbullet : MonoBehaviour
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
        // 충돌 여부 전송
        // 충돌 대상이 적이라면 정보 전송

        Debug.Log("발사체 충돌파괴");
        Destroy(gameObject);
    }
}
