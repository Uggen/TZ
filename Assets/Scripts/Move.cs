using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
   public float speed = 5f; 
    public float range = 10f; 

    void Update()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.y += speed * Time.deltaTime;

        if (currentPosition.y > range)
        {
            currentPosition.y = -range;
        }

        transform.position = currentPosition;
    }
}
