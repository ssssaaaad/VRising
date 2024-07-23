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

        mouseX = transform.rotation.eulerAngles.y;  //���콺 ����(x)�� ������(y) �� �߽�
        mouseY = -transform.rotation.eulerAngles.x; //���콺 ����(y)�� ������(-x) �� �߽�
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //ī�޶� �������� ��, ������ �̵���ŵ�ϴ�.
        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        movement = movement.normalized * (Time.deltaTime * moveSpeed);
        transform.position += movement;

        //���콺 ������ Ŭ�� �� ȸ����Ŵ
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

        //mouseX (���� ������) �� Y�࿡ ������ ��
        //mouseY (���� ������) �� X�࿡ ������ ��
        transform.rotation = Quaternion.Euler(transform.rotation.x - mouseY, transform.rotation.y + mouseX, 0.0f);
    }
}
