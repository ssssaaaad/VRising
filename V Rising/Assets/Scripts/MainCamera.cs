using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    public float rotSpeed = 1000f;
    public bool rotate;
    public float camDistance = 10f;
    float mx = 0;
    float my = 0;

    public float camcurrentinterval = 10;
    public float camcurrentinterval_Max = 20;

    
    private void Start()
    {
        // 초기 카메라 설정
        Vector3 offset = new Vector3(0, 5f, -camDistance);
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position);
    }

    
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            rotate = true;
            
            
            

            
        }
        if (Input.GetMouseButtonUp(1))
        {
            rotate = false;
           


        }

        
        Rotation();
        
    }
    public void Rotation()
    {
        if (rotate)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");


            mx += mouseX * rotSpeed * Time.deltaTime;
            my -= mouseY * rotSpeed * Time.deltaTime;

            my = Mathf.Clamp(my, 30f, 60f);

            


            transform.localEulerAngles = new Vector3(my, mx, 0);
        }

        
        camcurrentinterval -= Input.GetAxis("Mouse ScrollWheel") * 3;

        camcurrentinterval = Mathf.Clamp(camcurrentinterval, 4.5f, camcurrentinterval_Max);

        
        Vector3 cameraDir =  -transform.forward * camcurrentinterval;
        
        //카메라는 플레이어 위치 + 벡터의 위치에 있다
        //카메라의 회전은 사이의 벡터를 특정 축으로 회전한 벡터를 구하고 
        //쿼터니언과 벡터의 곱에서 힌트
        //다시 카메라의 위치를 플레이어 + 구한벡터 위치로

        transform.position = player.transform.position + cameraDir;



    }
    

}
