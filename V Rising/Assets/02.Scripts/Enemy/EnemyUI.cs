using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Enemy enemy;
    public Image image_HPBar;
    public Image image_DrainIcon;
    public Image image_FindIcon;
    public GameObject canvas;

    private Coroutine coroutine_FindIcon;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void UpdateHP()
    {
       
        if (enemy.Drain())
        {
            image_DrainIcon.gameObject.SetActive(true);
        }
        else
        {
            image_DrainIcon.gameObject.SetActive(false);
            if(enemy.hp_Current == 0)
            {
                canvas.SetActive(false);
            }
        }

        HPAnimation();
    }
    public void InactiveUI()
    {
        canvas.SetActive(false);
    }

    private void HPAnimation()
    {
        image_HPBar.fillAmount = enemy.hp_Current / enemy.hp_Max;
    }

    public void ActiveFindIcon()
    {
        if (coroutine_FindIcon != null)
        {
            StopCoroutine(coroutine_FindIcon);
        }
        coroutine_FindIcon = StartCoroutine(Coroutine_FindIcon());
    }

    private IEnumerator Coroutine_FindIcon()
    {
        image_FindIcon.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        image_FindIcon.gameObject.SetActive(false);
    }
}
