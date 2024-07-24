using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    public float rotSpeed = 1000f;
    private bool rotate;
    float mx = 0;
    float my = 0;

    public float camcurrentinterval = 10;
    

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            rotate = true;
        }
        if(Input.GetMouseButtonUp(1))
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

        camcurrentinterval = Mathf.Clamp(camcurrentinterval, 4.5f, 12f);


        Vector3 cameraDir =  -transform.forward * camcurrentinterval;

        //ī�޶�� �÷��̾� ��ġ + ������ ��ġ�� �ִ�
        //ī�޶��� ȸ���� ������ ���͸� Ư�� ������ ȸ���� ���͸� ���ϰ� 
        //���ʹϾ�� ������ ������ ��Ʈ
        //�ٽ� ī�޶��� ��ġ�� �÷��̾� + ���Ѻ��� ��ġ��

        transform.position = player.transform.position + cameraDir;



    }

}
