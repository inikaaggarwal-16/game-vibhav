using UnityEngine;

public class SpecialObjectVisibility : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public Vector2 objectCell; // The cell position of this special object
    public float cellSize = 1f; // Size of each cell in your grid

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the player reference is set
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Initially hide the special object
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        // Get the player's cell position
        Vector2 playerPosition = player.transform.position;
        Vector2 playerCell = new Vector2(
            Mathf.Floor(playerPosition.x / cellSize),
            Mathf.Floor(playerPosition.y / cellSize)
        );

        // Check if the player is in the same cell as the object
        if (playerCell == objectCell)
        {
            spriteRenderer.enabled = true; // Show the special object
        }
        else
        {
            spriteRenderer.enabled = false; // Hide the special object
        }
    }
}
