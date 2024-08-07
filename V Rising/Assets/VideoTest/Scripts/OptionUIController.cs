using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionUIController : MonoBehaviour
{

    [Header("퀄리티 옵션")]
    [SerializeField]
    List<RenderPipelineAsset> RenderPipelineAssets;

    [SerializeField]
    TMP_Dropdown dropdown;


    [Header("해상도 옵션")]
    public TMP_Dropdown resolutionDropdown;

    // 원하는 해상도를 리스트로 정의
    private List<Resolution> desiredResolutions = new List<Resolution>
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 2560, height = 1440 }
    };

    void Start()
    {
        PopulateDropdown();
        resolutionDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(resolutionDropdown); });
    }

    void PopulateDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < desiredResolutions.Count; i++)
        {
            string option = desiredResolutions[i].width + " x " + desiredResolutions[i].height;
            options.Add(option);

            if (desiredResolutions[i].width == Screen.currentResolution.width &&
                desiredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        SetResolution(change.value);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = desiredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, false);
    }

    public string GetSelectedDropdownValue()
    {
        return resolutionDropdown.options[resolutionDropdown.value].text;
    }

    public void SetPipeline(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = RenderPipelineAssets[value];
    }
}
