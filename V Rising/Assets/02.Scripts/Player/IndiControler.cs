using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndiControler : MonoBehaviour
{
    public GameObject Indi_e;
    public GameObject Indi_r;
    public GameObject Indi_t;
    public GameObject Model;

    private float IndiLength_E = 10f;      //인디케이터 길이
    private float IndiWidth_E = 0.5f;      //인디케이터 폭

    private float IndiLength_R = 16f;      //인디케이터 길이
    private float IndiWidth_R = 0.5f;      //인디케이터 폭

    private float IndiLength_T = 14f;      //인디케이터 길이
    private float IndiWidth_T = 1f;      //인디케이터 폭

    private Eskill Eskill;
    private Rskill Rskill;
    private Tskill Tskill;

    private GameObject Indi;
    private Vector3 Pos;
    private Quaternion Rot = Quaternion.Euler(90, 0, 0);

    void Start()
    {
        Eskill = GetComponent<Eskill>();
        Rskill = GetComponent<Rskill>();
        Tskill = GetComponent<Tskill>();

    }

    public void Indi_E()
    {
        Pos = new Vector3(0, 0.5f, IndiLength_E / 2);

        Indi = Instantiate(Indi_e);

        Indi.transform.SetParent(Model.transform);
        Indi.transform.localPosition = Pos;
        Indi.transform.localRotation = Rot;
        Vector3 a = new Vector3(IndiWidth_E, IndiLength_E, 0);
        print(a);
        Indi.transform.localScale = a;
        print(Indi.transform.localScale);
    }

    public void Indi_R()
    {
        Pos = new Vector3(0, 0.5f, IndiLength_R / 2);

        Indi = Instantiate(Indi_r);

        Indi.transform.SetParent(Model.transform);
        Indi.transform.localPosition = Pos;
        Indi.transform.localRotation = Rot;
        Vector3 a = new Vector3(IndiWidth_R, IndiLength_R, 0);
        print(a);
        Indi.transform.localScale = a;
        print(Indi.transform.localScale);
    }
    public void Indi_T()
    {
        Pos = new Vector3(0, 0.5f, IndiLength_T / 2);

        Indi = Instantiate(Indi_t);

        Indi.transform.SetParent(Model.transform);
        Indi.transform.localPosition = Pos;
        Indi.transform.localRotation = Rot;
        Vector3 a = new Vector3(IndiWidth_T, IndiLength_T, 0);
        print(a);
        Indi.transform.localScale = a;
        print(Indi.transform.localScale);
    }

    public void Indi_E_break()
    {
        Destroy(Indi);
    }
    public void Indi_R_break()
    {
        Destroy(Indi);
    }
    public void Indi_T_break()
    {
        Destroy(Indi);
    }
}