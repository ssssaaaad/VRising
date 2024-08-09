using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndiControler : MonoBehaviour
{
    public GameObject Indi;

    public float IndiLength_E = 10f;      //인디케이터 길이
    public float IndiWidth_E = 0.5f;      //인디케이터 폭

    public float IndiLength_R = 10f;      //인디케이터 길이
    public float IndiWidth_R = 0.5f;      //인디케이터 폭

    public float IndiLength_T = 10f;      //인디케이터 길이
    public float IndiWidth_T = 0.5f;      //인디케이터 폭

    private Eskill Eskill;
    private Rskill Rskill;
    private Tskill Tskill;

    void Start()
    {
        Eskill = GetComponent<Eskill>();
        Rskill = GetComponent<Rskill>();   
        Tskill = GetComponent<Tskill>();
    }

    public void Indi_E()
    {

    }

    public void Indi_R()
    {

    }
    public void Indi_T()
    {

    }
}
