using UnityEngine;

public class DoorSortingOrder : MonoBehaviour
{
    public GameObject player; 
    public GameObject door;   

    private SpriteRenderer doorSpriteRenderer; 

    void Start()
    {
        
        doorSpriteRenderer = door.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        float playerY = player.transform.position.y;
        float doorY = door.transform.position.y;

         
        if (playerY < doorY)
        {
            
            doorSpriteRenderer.sortingOrder = 0;
        }
        
        else if (doorY < playerY)
        {
            
            doorSpriteRenderer.sortingOrder = 1;
        }
    }
}
