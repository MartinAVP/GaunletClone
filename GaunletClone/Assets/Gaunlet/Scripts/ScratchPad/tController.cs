using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class tController : MonoBehaviour
{
    public float speed = 3;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0) * speed * Time.fixedDeltaTime;
        }

        if(Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -1) * speed * Time.fixedDeltaTime;
        }

        if(Input.GetKey(KeyCode.D)) 
        {
            transform.position += new Vector3(1, 0, 0) * speed * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, 1) * speed * Time.fixedDeltaTime;
        }
    }


}
