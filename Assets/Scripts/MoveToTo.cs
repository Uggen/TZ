using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTo : MonoBehaviour
{
 public float speed = 5f; 
    public float range = 4.5f; 

    void Update()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x += speed * Time.deltaTime;

        if (Mathf.Abs(currentPosition.x) > range)
        {
            speed *= -1; 
        }
        transform.position = currentPosition;
    }
}
