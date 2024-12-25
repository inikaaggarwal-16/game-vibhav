using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float moveSpeed = 5f;

    
    private Rigidbody2D rb;

    
    private Vector2 moveDirection;

    
    private GameObject terrainToDisable;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical");   

        
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Terrain"))
        {
            
            terrainToDisable = collision.gameObject;

            
            Invoke("DisableTerrain", 1f);
        }
    }

   
    void DisableTerrain()
    {
        if (terrainToDisable != null)
        {
            terrainToDisable.SetActive(false);  
        }
    }
}