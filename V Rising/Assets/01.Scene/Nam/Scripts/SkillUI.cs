using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Text text_CoolTime;
    public Image image_fill;
    public float time_cooltime = 10;
    private float time_current;
    private float time_start;
    private bool isEnded = true;

    //    변수(variable)

    //text_CoolTime : 남은 시간 표시(Text)
    //image_fill : 남은 시간 시각적으로 표시(Image)

    //time_cooltime : 쿨타임으로 사용할 시간(float)
    //time_current : 스킬 재사용까지 남은 시간(float)
    //time_start : time.Time과 비교해서 time_current를 만들기 위해 시간을 저장(float)
    //isEnded : 쿨타임이 끝났을 때 true (bool)

    //    변수(variable)

    //text_CoolTime : 남은 시간 표시(Text)
    //image_fill : 남은 시간 시각적으로 표시(Image)

    //time_cooltime : 쿨타임으로 사용할 시간(float)
    //time_current : 스킬 재사용까지 남은 시간(float)
    //time_start : time.Time과 비교해서 time_current를 만들기 위해 시간을 저장(float)
    //isEnded : 쿨타임이 끝났을 때 true (bool)
    void Start()
    {

    }

    void Update()
    {
        if (isEnded)
            return;
        Check_CoolTime();
    }
    public void coolTimeImage()
    {
        Init_UI();
        Trigger_Skill();
    }
    private void Init_UI()
    {
        image_fill.type = Image.Type.Filled;
        image_fill.fillMethod = Image.FillMethod.Radial360;
        image_fill.fillOrigin = (int)Image.Origin360.Top;
        image_fill.fillAmount = 0;
        image_fill.fillClockwise = false;
    }

    private void Check_CoolTime()
    {
        time_current = Time.time - time_start;
        if (time_current < time_cooltime)
        {
            Set_FillAmount(time_cooltime - time_current);
        }
        else if (!isEnded)
        {
            End_CoolTime();
        }
    }

    private void End_CoolTime()
    {
        Set_FillAmount(0);
        isEnded = true;
        text_CoolTime.gameObject.SetActive(false);

        Debug.Log("Skills Available!");
    }

    private void Trigger_Skill()
    {
        if (!isEnded)
        {
            Debug.LogError("Hold On");
            return;
        }

        Reset_CoolTime();
        Debug.LogError("Trigger_Skill!");
    }

    private void Reset_CoolTime()
    {
        text_CoolTime.gameObject.SetActive(true);
        time_current = time_cooltime;
        time_start = Time.time;
        Set_FillAmount(time_cooltime);
        isEnded = false;
    }
    private void Set_FillAmount(float _value)
    {
        image_fill.fillAmount = _value / time_cooltime;
        string txt = _value.ToString("0.0");
        text_CoolTime.text = txt;

        Debug.Log(txt);
    }
}

