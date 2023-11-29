using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensa = 2f; //mouse sens
    public float maxYAngle = 80f; //max angle by Y-axis
    public float maxXAngle = 80f; 
    public float rotatinX = 0f;
    

    void Start()
    {
        
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.parent.Rotate (Vector3.up * mouseX * sensa);


        rotatinX -= mouseY * sensa;
        rotatinX = Mathf.Clamp(rotatinX, -maxYAngle, maxYAngle);
        transform.localRotation = Quaternion.Euler(rotatinX, 0f,0f);

    }
}
