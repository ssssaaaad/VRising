using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Enemy enemy;
    private Coroutine coroutine_HPAnimation;
    public Image image_HPBar;
    public Image image_DrainIcon;
    public Image image_FindIcon;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void UpdateHP()
    {
        if(coroutine_HPAnimation != null)
        {
            StopCoroutine(coroutine_HPAnimation);
        }

        if (enemy.Drain())
        {
            image_DrainIcon.gameObject.SetActive(true);
        }
        else
        {
            image_DrainIcon.gameObject.SetActive(false);
        }

        coroutine_HPAnimation = StartCoroutine(HPAnimation());
    }

    private IEnumerator HPAnimation()
    {
        float percent = image_HPBar.fillAmount;
        print(1);
        for (int i = 0; i < 10; i++)
        {
            image_HPBar.fillAmount = Mathf.Lerp(percent, enemy.hp_Current / enemy.hp_Max, (i + 1) / 10);
            yield return null;
        }
        print(2);
        coroutine_HPAnimation = null;
    }
}
