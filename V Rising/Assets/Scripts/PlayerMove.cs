using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 5f;
    Camera characterCamera;
    public GameObject go;
    CharacterController cc;

    public float gravity = -20f;
    float yVelocity = 0;
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    
    private void Awake()
    {
        characterCamera = Camera.main;
    }
    
    void Update()
    {
        float ad = Input.GetAxis("Horizontal");
        float ws = Input.GetAxis("Vertical");

        Vector3 dirad = transform.right * ad;
        Vector3 dirws = transform.forward * ws;

        Vector3 dir = dirad + dirws;
        dir.Normalize();
        LookMouseCursor();
        

        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        cc.Move(dir * playerSpeed * Time.deltaTime);

        

    }
    
        
       
        
    
    public void LookMouseCursor()
    {
        var ray = characterCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;
        if (Physics.Raycast(ray, out hitResult))
        {
            Vector3 mouseDir = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
            go.transform.forward = mouseDir;

            
            
        }
    }
}
