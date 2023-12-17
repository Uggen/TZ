using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f; 
    public float range = 10f;
    private Transform objectTransform;
    public Vector3 moveDirection = Vector3.up;

    void Awake()
    {
        objectTransform = GetComponent<Transform>();
    } 

    void Update()
    {
        Vector3 currentPosition = objectTransform.localPosition + moveDirection * speed * Time.deltaTime;

        if (currentPosition.y > range)
        {
            currentPosition.y = -range;
        }
        if (currentPosition.x > range)
        {
            currentPosition.x = -range;
        }
        if (currentPosition.z > range)
        {
            currentPosition.z = -range;
        }

        objectTransform.localPosition = currentPosition;
    }
}
