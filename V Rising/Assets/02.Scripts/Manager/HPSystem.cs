using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currHP;
    public Image HPImage; // 체력바 이미지 컴포넌트 (UI용)
    
    public Canvas healthBarCanvasPrefab; // 체력바를 포함할 Canvas 프리팹
    public Vector3 healthBarOffset = new Vector3(0, 2, 0); // 체력바가 캐릭터 위에 배치될 오프셋

    private Canvas healthBarCanvasInstance;

    private void Start()
    {
        currHP = maxHealth;
        UpdateHealthBar();


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(150f);
        }
    }

    public void TakeDamage(float damage)
    {
        currHP -= damage;
        if (currHP <= 0)
        {
            currHP = 0;
            Die();
        }
        UpdateHealthBar();
    }
   
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // 또는 다른 죽음 처리 로직
    }

    private void UpdateHealthBar()
    {
        if (HPImage != null)
        {
            HPImage.fillAmount = currHP / maxHealth;
        }
    }
    private void CreateHealthBar()
    {
        if (healthBarCanvasPrefab != null)
        {
            // 체력바 캔버스 프리팹 인스턴스 생성
            healthBarCanvasInstance = Instantiate(healthBarCanvasPrefab, transform.position + healthBarOffset, Quaternion.identity);
            // 체력바 캔버스의 위치 조정
            healthBarCanvasInstance.transform.SetParent(transform);
            healthBarCanvasInstance.transform.localPosition = healthBarOffset;
            healthBarCanvasInstance.transform.localRotation = Quaternion.identity;
            
            // 체력바 이미지 연결
            var healthBar = healthBarCanvasInstance.GetComponentInChildren<Image>();
            if (healthBar != null)
            {
                healthBar.fillAmount = currHP / maxHealth; // 초기 상태로 체력바 설정
            }
        }
    }
}


