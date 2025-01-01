using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player; 
    public Vector3 offset;    
    void Start()
    {
        
        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
    }
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
