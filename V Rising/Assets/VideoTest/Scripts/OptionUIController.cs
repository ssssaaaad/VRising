using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class OptionUIController : MonoBehaviour
{
    [SerializeField]
    List<RenderPipelineAsset> RenderPipelineAssets;

    [SerializeField]
    TMP_Dropdown Dropdown;

    public void SetPipeline(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = RenderPipelineAssets[value];
    }
}
