using UnityEngine;
using UnityEngine.UI;

public class PlayerPickup : MonoBehaviour
{
    public Transform carryPosition; // The position where the picked-up object will be held
    public Button actionButton; // Reference to the action button
    private GameObject carriedObject = null;
    private Vector3 playerPickupPosition; // Store the position when an object is picked up
    public static string pickupCorner; // Store the corner the player is closest to

    // Define grid parameters
    private float gridSize = 0.5f;
    private float gridWidth = 8;
    private float gridHeight = 8;

    private Vector2 bottomLeftCorner = new Vector2(-6.25f, -2.75f);
    private Vector2 topLeftCorner;
    private Vector2 topRightCorner;
    private Vector2 bottomRightCorner;

    void Start()
    {
        // Set up the button's onClick listener
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnActionButtonClick);
            actionButton.gameObject.SetActive(false); // Hide button by default
        }

        // Calculate the corner positions based on grid size
        topLeftCorner = new Vector2(bottomLeftCorner.x, bottomLeftCorner.y + gridHeight * gridSize);
        topRightCorner = new Vector2(bottomLeftCorner.x + gridWidth * gridSize, topLeftCorner.y);
        bottomRightCorner = new Vector2(topRightCorner.x, bottomLeftCorner.y);
    }

    void Update()
    {
        if (carriedObject == null)
        {
            CheckForNearbyObjects();
        }
    }

    private void CheckForNearbyObjects()
    {
        // Detect nearby objects with tags "Fire" or "Lamp"
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.75f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Fire") || collider.CompareTag("Lamp"))
            {
                actionButton.gameObject.SetActive(true); // Show button
                return;
            }
        }

        actionButton.gameObject.SetActive(false); // Hide button if no objects nearby
    }

    public void OnActionButtonClick()
    {
        Debug.Log("Action Button Clicked");
        if (carriedObject == null)
        {
            TryPickupObject();
        }
        else
        {
            // Drop object logic is removed because we only allow one object to be carried
        }
    }

    private void TryPickupObject()
    {
        // Detect nearby objects with tags "Fire" or "Lamp"
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.75f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Fire") || collider.CompareTag("Lamp"))
            {
                
                PickupObject(collider.gameObject);
                actionButton.GetComponentInChildren<Text>().text = "Drop"; // Change button text
                break;
            }
        }
    }

    private void PickupObject(GameObject obj)
    {
        carriedObject = obj;
        playerPickupPosition = transform.position;
        Debug.Log("Player's position when picking up object: " + playerPickupPosition);

        // Determine the closest corner to the player's position
        pickupCorner = GetClosestCorner(playerPickupPosition);
        Debug.Log("Player picked up object near: " + pickupCorner);

        // Disable physics (optional)
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true;

        // Set the object's position and parent
        obj.transform.position = carryPosition.position;
        obj.transform.SetParent(carryPosition);
    }

    
    // Determine which corner the player's position is closest to
    private string GetClosestCorner(Vector3 position)
    {
        // Find the distance from the player's position to each corner
        float distanceToTopLeft = Vector2.Distance(position, topLeftCorner);
        float distanceToTopRight = Vector2.Distance(position, topRightCorner);
        float distanceToBottomLeft = Vector2.Distance(position, bottomLeftCorner);
        float distanceToBottomRight = Vector2.Distance(position, bottomRightCorner);

        // Find the minimum distance and return the corresponding corner
        float minDistance = Mathf.Min(distanceToTopLeft, distanceToTopRight, distanceToBottomLeft, distanceToBottomRight);

        if (minDistance == distanceToTopLeft) return "TopLeft";
        if (minDistance == distanceToTopRight) return "TopRight";
        if (minDistance == distanceToBottomLeft) return "BottomLeft";
        if (minDistance == distanceToBottomRight) return "BottomRight";

        return "Unknown";
    }
}

