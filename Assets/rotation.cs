using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    
    public float rotationSpeed = 50f;

    void Update()
    {
        
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}

