using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act_Range : MonoBehaviour
{
    public Collider something;

    private void OnTriggerEnter(Collider other)
    {
        something = other;
    }

    private void OnTriggerExit(Collider other)
    {
        something = null;
    }
}
