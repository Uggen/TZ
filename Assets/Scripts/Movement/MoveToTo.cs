using UnityEngine;

public class MoveToTo : MonoBehaviour
{
    public float speed = 5f; 
    public float range = 4.5f; 
    private Transform objectTransform;
    public Vector3 moveDirection = Vector3.right;

    void Awake()
    {
        objectTransform = GetComponent<Transform>();
    }
    void Update()
    {
        Vector3 currentPosition = objectTransform.localPosition + moveDirection * speed * Time.deltaTime;

        if (Mathf.Abs(currentPosition.x) > range)
        {
            speed *= -1; 
        }
        if (Mathf.Abs(currentPosition.y) > range)
        {
            speed *= -1; 
        }
        if (Mathf.Abs(currentPosition.z) > range)
        {
            speed *= -1; 
        }
        objectTransform.localPosition = currentPosition;
    }
}
