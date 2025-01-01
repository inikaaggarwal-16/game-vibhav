using UnityEngine;

public class DoorColorChanger : MonoBehaviour
{
    private bool colorsChanged = false; // Flag to track if colors have been changed
    private bool canChangeColors = false; // Flag to check if the player has picked up an object

    void Update()
    {
        // Check if the player has picked up an object
        if (PlayerPickup.pickupCorner != null && !canChangeColors)
        {
            canChangeColors = true; // Allow color change after the player picks up an object
        }

        // Change colors only if pickupCorner is set and has not already been changed
        if (canChangeColors && PlayerPickup.pickupCorner != ProceduralPropGenerator.selectedCornerKey && !colorsChanged)
        {
            ChangeDoorColors();
            colorsChanged = true; // Mark that colors have been changed
        }
    }

    private void ChangeDoorColors()
    {
        // Find all objects with specific tags and change their color
        GameObject[] doors = GameObject.FindGameObjectsWithTag("WrongDoor");
        foreach (var door in doors)
        {
            ChangeColor(door);
        }

        doors = GameObject.FindGameObjectsWithTag("RightDoorUp");
        foreach (var door in doors)
        {
            ChangeColor(door);
        }

        doors = GameObject.FindGameObjectsWithTag("RightDoorDown");
        foreach (var door in doors)
        {
            ChangeColor(door);
        }
    }

    private void ChangeColor(GameObject door)
    {
        // Get the SpriteRenderer component
        SpriteRenderer spriteRenderer = door.GetComponent<SpriteRenderer>();

        // If there's a SpriteRenderer, change its color
        if (spriteRenderer != null)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value); // Generate random color
            spriteRenderer.color = randomColor;
        }
    }

    // Optional: If you want to allow color change again when the condition changes
    public void ResetColorChange()
    {
        colorsChanged = false; // Reset the flag so colors can be changed again when condition is met
    }
}
