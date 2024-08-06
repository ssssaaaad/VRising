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


    private void Start()
    {
        Button startButton = startSprite.AddComponent<Button>();
        startButton.onClick.AddListener(OnStartButtonClick);

        Button optionButton = optionSprite.AddComponent<Button>();
        optionButton.onClick.AddListener(OnOptionButtonClick);

        Button endButton = endSprite.AddComponent<Button>();
        endButton.onClick.AddListener(OnEndButtonClick);


    }

    void OnStartButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    void OnOptionButtonClick()
    {
        option.SetActive(true);
    }

    void OnEndButtonClick()
    {
        Application.Quit();
    }

}
