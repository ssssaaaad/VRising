using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCam : MonoBehaviour
{
    private enum Mode
    {
        
        CameraForward,
        
    }

    [SerializeField] private Mode mode;
    private void Update()
    {
        switch (mode)
        {
            
            case Mode.CameraForward:
                //* 카메라 방향으로 Z축 (앞뒤)을 바꿔주기
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }
    }
}
    