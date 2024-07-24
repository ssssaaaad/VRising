using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class NewBehaviourScript : MonoBehaviour
{
    public float currentTime;
    public float activeTime;

    void Start()
    {
        
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        Vector3.Lerp(transform.position, transform.position + Vector3.forward * 10, currentTime / activeTime);
    }

}
