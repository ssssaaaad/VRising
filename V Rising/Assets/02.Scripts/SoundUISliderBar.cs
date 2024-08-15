using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUISliderBar : MonoBehaviour
{
    public Slider slider;
    public bool bgm;
    void Start()
    {
        slider = GetComponent<Slider>();

        if (bgm)
        {
            slider.value = SoundManager.instance.bgmVolume;
        }
        else
        {
            slider.value = SoundManager.instance.sfxVolume;
        }
    }
}
