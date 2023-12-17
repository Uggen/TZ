using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float rotationSpeed;
    private Transform objectTransform;
    public Vector3 moveDirection = Vector3.forward;

    void Awake()
    {
        objectTransform = GetComponent<Transform>();
    } 

    void Update()
    {
        objectTransform.Rotate(moveDirection * rotationSpeed * Time.deltaTime);
    }
}
