using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private Playerstate player;

    [SerializeField] private Enemy bossHealth;
    [SerializeField] private Image bossHealthImage;

    [SerializeField] private Playerstate health;
    [SerializeField] private Image healthBarImage;


    [SerializeField] private Fblood bloodHealth;
    [SerializeField] private Image bloodHealthBarImage;

    [SerializeField] private float updateSpeedSeconds = 0.5f;

    [SerializeField] private TextMeshProUGUI curentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [SerializeField] private GameObject rSkillRock;
    [SerializeField] private GameObject cSkillRock;
    [SerializeField] private GameObject tSkillRock;

    [SerializeField] private Image bloodSkill;
    [SerializeField] private Image itemSkill;

    [SerializeField] private float textScaleAnimationDuration = 0.3f;  // 텍스트 애니메이션 지속 시간
    [SerializeField] private float maxTextScale = 1.5f;  // 텍스트의 최대 크기

    [SerializeField] private float textAnimationDuration = 0.1f;  // 텍스트 변화 사이 시간 간격

    private float targetFillAmount;
    private float currentDisplayHealth;

    public void rockRC()
    {
        rSkillRock.SetActive(false);
        cSkillRock.SetActive(false);
    }

    public void rockT()
    {
        tSkillRock.SetActive(false);
    }



    public static InGameUIController instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Health 클래스의 이벤트에 리스너를 추가합니다.
        health.OnHealthChanged += UpdateHealthBar;
        bossHealth.OnHealthChanged += UpdateBossHealthBar;
        bloodHealth.OnHealthChanged += UpdateBloodHealthBar;

        curentHealthText.transform.localScale = Vector3.one;

        currentDisplayHealth = health.hp_Max;
        //bloodHealthBarImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //ItemSkill_lcon_true(player.transform);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ItemSkill_lcon_false();
        }
    }

    public void ItemSkill_lcon_true(Transform pos)
    {
    }
    public void ItemSkill_lcon_false()
    {
    }

    GameObject image_F;
    public void BloodSkill_lcon_true(Transform pos)
    {
        image_F = pos.GetComponentInParent<Enemy>().image_F;
        if (image_F != null)
        {
            pos.GetComponentInParent<Enemy>().image_F.SetActive(true);
        }
    }
    public void BloodSkill_lcon_false()
    {
        //bloodSkill.gameObject.SetActive(false);
    }

    private IEnumerator BloodSkillCon()
    {
        bloodSkill.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        bloodSkill.gameObject.SetActive(false);
    }

    private void UpdateBloodHealthBar(float currentHealth, float maxHealth)
    {
        bloodHealthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }

    private void UpdateBossHealthBar(float currentHealth, float maxHealth)
    {
        bossHealthImage.fillAmount = (float)currentHealth / maxHealth;
        //StartCoroutine(AnimateHealthBarBoss(currentHealth, maxHealth));
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {

        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
        curentHealthText.text = ((int)currentHealth).ToString();
        maxHealthText.text = " / " + maxHealth.ToString();
    }

    IEnumerator AnimateText(float startHealth, float endHealth)
    {
        int step = startHealth < endHealth ? 1 : -1;
        int currentHealth = (int)startHealth;


        while (currentHealth != endHealth)
        {
            currentHealth += step;
            curentHealthText.text = currentHealth.ToString();
            yield return new WaitForSeconds(textAnimationDuration);
        }

        currentDisplayHealth = endHealth;  // 최종 체력 값 업데이트
    }

    IEnumerator AnimateScaleText()
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

    IEnumerator AnimateHealthBarBoss(float currentHealth, float maxHealth)
    {
        float preChangePct = bossHealthImage.fillAmount;
        float newPct = (float)currentHealth / maxHealth;

        float elapsed = 0f;

        while (elapsed < preChangePct)
        {
            elapsed += Time.deltaTime;
            bossHealthImage.fillAmount = Mathf.Lerp(preChangePct, newPct, elapsed / updateSpeedSeconds);

            yield return null;
        }

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
        bossHealth.OnHealthChanged -= UpdateBossHealthBar;
        bloodHealth.OnHealthChanged -= UpdateBloodHealthBar;
    }
}
