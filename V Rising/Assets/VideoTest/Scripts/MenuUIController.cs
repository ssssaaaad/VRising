using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    public Image startSprite;
    public Image optionSprite;
    public Image endSprite;

    public GameObject option;
    public GameObject menu;

    public Image backOptionSprite;

    public Image fadeOut;

    private void Start()
    {
        Button startButton = startSprite.AddComponent<Button>();
        startButton.onClick.AddListener(OnStartButtonClick);

        Button optionButton = optionSprite.AddComponent<Button>();
        optionButton.onClick.AddListener(OnOptionButtonClick);

        Button endButton = endSprite.AddComponent<Button>();
        endButton.onClick.AddListener(OnEndButtonClick);

        Button backOptionButton = backOptionSprite.AddComponent<Button>();
        backOptionButton.onClick.AddListener(OnBackOptionButtonClick);
    }

    void OnBackOptionButtonClick()
    {
        menu.SetActive(true);
        option.SetActive(false);
    }

    void OnStartButtonClick()
    {
        StartCoroutine(SceneLoad(1));
    }

    void OnOptionButtonClick()
    {
        option.SetActive(true);
    }

    void OnEndButtonClick()
    {
        Application.Quit();
    }

    private void FadeOut()
    {
        fadeOut.DOColor(new Color(0, 0, 0, 1), 1.5f);
    }

    IEnumerator SceneLoad(int index)
    {
        FadeOut();
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(index);
    }
}
