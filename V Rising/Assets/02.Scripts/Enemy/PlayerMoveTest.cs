using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerMoveTest : MonoBehaviour
{
    private float x;
    private float z;
    public float speed = 3;
    private Vector3 movementDirection;

    void Start()
    {

    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        movementDirection = new Vector3(x, 0, z).normalized;

        transform.position += movementDirection * speed * Time.deltaTime;
    }

}
