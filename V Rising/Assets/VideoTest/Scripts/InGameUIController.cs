using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private Playerstate health;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private float updateSpeedSeconds = 0.5f;

    [SerializeField] private TextMeshProUGUI curentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [SerializeField] private float textAnimationDuration = 0.3f;  // 텍스트 애니메이션 지속 시간
    [SerializeField] private float maxTextScale = 1.5f;  // 텍스트의 최대 크기

    private void Start()
    {
        // Health 클래스의 이벤트에 리스너를 추가합니다.
        health.OnHealthChanged += UpdateHealthBar;
        curentHealthText.transform.localScale = Vector3.one;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // 체력바 업데이트 
        StartCoroutine(AnimateHealthBar(currentHealth, maxHealth));
        
        curentHealthText.text = currentHealth.ToString();
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        float elapsed = 0f;

        // 텍스트 크기를 키우는 단계
        while (elapsed < textAnimationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, maxTextScale, elapsed / (textAnimationDuration / 2));
            curentHealthText.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        elapsed = 0f;

        // 텍스트 크기를 원래대로 되돌리는 단계
        while (elapsed < textAnimationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(maxTextScale, 1f, elapsed / (textAnimationDuration / 2));
            curentHealthText.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        curentHealthText.transform.localScale = Vector3.one;  // 크기를 원래대로 설정
    }

    IEnumerator AnimateHealthBar(float currentHealth, float maxHealth)
    {
        float preChangePct = healthBarImage.fillAmount;
        float newPct = (float)currentHealth / maxHealth;
        
        float elapsed = 0f;

        while (elapsed < preChangePct)
        {
            elapsed += Time.deltaTime;
            healthBarImage.fillAmount = Mathf.Lerp(preChangePct, newPct, elapsed / updateSpeedSeconds);

            yield return null;
        }

        
        maxHealthText.text = " / " + maxHealth.ToString();
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        health.OnHealthChanged -= UpdateHealthBar;
    }
}
