using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eskillbullet : MonoBehaviour
{
    public Eskill Eskill;

    public float speed = 50f; // 발사체의 속도
    private Rigidbody rb;

    void Start()
    {
        Eskill = FindObjectOfType<Eskill>();

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 발사체의 방향으로 속도 설정
            rb.velocity = transform.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 충돌 여부 전송
            if (Eskill != null)
            {
                Eskill.ComboEAct(other);
            }
            // 데미지 입력
            Eskill.Damage(other, Eskill.Edmg);
        }
        Debug.Log("발사체 충돌파괴");
        Destroy(gameObject);
    }
}
