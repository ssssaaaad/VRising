using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Playerstate : MonoBehaviour
{
    public Animator animator;

    private PlayerManager PM;
    private Cskill Cskill;

    public float hp_Max = 500;
    public float hp_Current;
    public float power = 40;
    public bool dead = false;

    public event Action<float, float> OnHealthChanged;

    public int cameraShakeTypeIndex = 0;

    private SFXAudioSource hitSound;

    void Start()
    {
        animator = GetComponent<Animator>();

        hp_Current = hp_Max;
        PM = GetComponent<PlayerManager>();
        Cskill = GetComponent<Cskill>();
        OnHealthChanged?.Invoke(hp_Current, hp_Max);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(-10f);
        }
    }

    public void TakeDamage(float damage)
    {
        hp_Current -= damage;
        if (hp_Current <= 0)
        {
            hp_Current = 0;
            //Die();
        }
        UpdateHP(damage);
    }

    public void UpdateHP(float dmg)
    {
        if (PM.cskilling)
        {
            Cskill.counter = true;
        }
        else if (hp_Current > 0)
        {
            hp_Current -= dmg;
            
            if (hitSound != null)
            {
                if (!hitSound.audioSource.isPlaying)
                {
                    hitSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.PlayerHit, transform, Vector3.zero);
                }
            }
            else
            {
                hitSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.PlayerHit, transform, Vector3.zero);
            }
            SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.PlayerHit, transform, Vector3.zero);
            CameraShakeManager.instance.ShakeSkillCall(cameraShakeTypeIndex);
        }
            
        if (hp_Current > hp_Max) 
            hp_Current = hp_Max;


        // HP가 변경되면 이벤트를 호출
        OnHealthChanged?.Invoke(hp_Current, hp_Max);

        if (hp_Current <= 0)
        {
            dead = true;
            animator.SetTrigger("Death");
            animator.SetBool("Death_Check", true);
        }
    }
}
