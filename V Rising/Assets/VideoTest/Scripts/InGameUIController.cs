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

    private void Start()
    {
        // Health 클래스의 이벤트에 리스너를 추가합니다.
        health.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // 체력바 업데이트 
        StartCoroutine(AnimateHealthBar(currentHealth, maxHealth));
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
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        health.OnHealthChanged -= UpdateHealthBar;
    }
}
