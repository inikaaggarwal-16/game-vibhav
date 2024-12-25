using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Speed at which the player moves
    public float moveSpeed = 5f;

    // Reference to the player's Rigidbody2D component
    private Rigidbody2D rb;

    // Movement vector (x and y)
    private Vector2 moveDirection;

    // Store the terrain object temporarily to disable it later
    private GameObject terrainToDisable;

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get the input from the arrow keys (or WASD keys, depending on platform settings)
        float moveX = Input.GetAxisRaw("Horizontal"); // -1 for left, 1 for right
        float moveY = Input.GetAxisRaw("Vertical");   // -1 for down, 1 for up

        // Set the move direction based on input
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Move the player by applying the movement to the Rigidbody2D
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    // Detect collision with objects with the "Terrain" tag
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the "Terrain" tag
        if (collision.gameObject.CompareTag("Terrain"))
        {
            // Store the terrain object to disable it after a delay
            terrainToDisable = collision.gameObject;

            // Invoke the method to disable the terrain after 1 second
            Invoke("DisableTerrain", 1f);
        }
    }

    // This method will be invoked after 1 second to disable the terrain object
    void DisableTerrain()
    {
        if (terrainToDisable != null)
        {
            terrainToDisable.SetActive(false);  // Disable the terrain object
        }
    }
}
