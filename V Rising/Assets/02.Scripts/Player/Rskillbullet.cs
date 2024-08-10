using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rskillbullet : MonoBehaviour
{
    public float speed = 20f; // 발사체의 속도
    private Rigidbody rb;
    private Rskill Rskill;
    private Playerstate PS;

    void Start()
    {
        Rskill = FindObjectOfType<Rskill>();
        PS = FindObjectOfType<Playerstate>();
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 발사체의 방향으로 속도 설정
            rb.velocity = transform.forward * speed;
        }

        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.RSkill_Fire, transform, Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Rskill.Damage(other, Rskill.Rdmg);
            PS.UpdateHP(Rskill.Rdmg * PS.power);
        }

        Destroy(gameObject);
        Debug.Log("발사체 충돌파괴");
    }
}
