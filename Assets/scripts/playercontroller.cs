using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveStep = 0.2f;  
    public float moveSpeed = 2f; // Removed the extra 'a'

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private GameObject terrainToDisable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            moveDirection = Vector2.up;
            MovePlayer();
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            moveDirection = Vector2.down;
            MovePlayer();
        }
        else if (Input.GetKeyDown(KeyCode.A)) 
        {
            moveDirection = Vector2.left;
            MovePlayer();
        }
        else if (Input.GetKeyDown(KeyCode.D)) 
        {
            moveDirection = Vector2.right;
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        rb.MovePosition(rb.position + moveDirection * moveStep);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            terrainToDisable = collision.gameObject;
            DisableTerrain();
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
