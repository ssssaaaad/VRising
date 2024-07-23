using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float rotateSpeed = 5.0f;
    public float moveSpeed = 3.5f;
    public float limitAngle = 70.0f;

    private new Transform transform;
    private bool isRotate;
    private float mouseX;
    private float mouseY;

    private void Start()
    {
        transform = GetComponent<Transform>();

        mouseX = transform.rotation.eulerAngles.y;  //마우스 가로(x)는 세로축(y) 이 중심
        mouseY = -transform.rotation.eulerAngles.x; //마우스 세로(y)는 가로축(-x) 이 중심
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //카메라를 기준으로 앞, 옆으로 이동시킵니다.
        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        movement = movement.normalized * (Time.deltaTime * moveSpeed);
        transform.position += movement;

        //마우스 오른쪽 클릭 시 회전시킴
        if (Input.GetMouseButtonDown(1))
        {
            isRotate = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotate = false;
        }

        if (isRotate)
        {
            Rotation();
        }
    }

    public void Rotation()
    {
        mouseX += Input.GetAxis("Mouse X") * rotateSpeed; // AxisX = Mouse Y
        mouseY = Mathf.Clamp(mouseY + Input.GetAxis("Mouse Y") * rotateSpeed, -limitAngle, limitAngle);

        //mouseX (가로 움직임) 은 Y축에 영향을 줌
        //mouseY (세로 움직임) 은 X축에 영향을 줌
        transform.rotation = Quaternion.Euler(transform.rotation.x - mouseY, transform.rotation.y + mouseX, 0.0f);
    }
}
