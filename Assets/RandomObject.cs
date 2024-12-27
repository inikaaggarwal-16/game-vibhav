using System.Collections.Generic;
using UnityEngine;

public class LeapObjectManager : MonoBehaviour
{
    public Vector2 gridOrigin = new Vector2(430, 190); // Bottom-left corner of the grid
    public int gridWidth = 8; // Width of the grid in cells
    public int gridHeight = 8; // Height of the grid in cells
    public float cellSize = 0.5f; // Size of each cell

    private HashSet<Vector2> occupiedCells = new HashSet<Vector2>();

    private void Start()
    {
        RandomizeObjects("Leap");
        RandomizeObjects("Heart");
        RandomizeSolidObjects();
    }

    public void RandomizeObjects(string tag)
    {
        // Find all objects with the specified tag
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            Vector2 newPosition;
            do
            {
                // Generate a random position within the grid
                int randomX = Random.Range(0, gridWidth);
                int randomY = Random.Range(0, gridHeight);

                newPosition = new Vector2(
                    gridOrigin.x + randomX * cellSize,
                    gridOrigin.y + randomY * cellSize
                );
            }
            // Ensure the cell isn't already occupied
            while (occupiedCells.Contains(newPosition));

            // Place the object and mark the cell as occupied
            obj.transform.position = newPosition;
            occupiedCells.Add(newPosition);
        }
    }

    public void RandomizeSolidObjects()
    {
        // Find all objects with the "Solid" tag
        GameObject[] solidObjects = GameObject.FindGameObjectsWithTag("Solid");

        foreach (GameObject obj in solidObjects)
        {
            Vector2 newPosition;
            do
            {
                // Generate a random position within the grid
                int randomX = Random.Range(0, gridWidth);
                int randomY = Random.Range(0, gridHeight);

                newPosition = new Vector2(
                    gridOrigin.x + randomX * cellSize,
                    gridOrigin.y + randomY * cellSize
                );
            }
            // Ensure the cell isn't already occupied
            while (occupiedCells.Contains(newPosition));

            // Adjust Y position for "Solid" objects
            newPosition.y -= 0.25f;

            // Place the object and mark the cell as occupied
            obj.transform.position = newPosition;
            occupiedCells.Add(new Vector2(newPosition.x, newPosition.y + 0.25f)); // Store original cell position
        }
    }

    public void TeleportAndRandomize()
    {
        occupiedCells.Clear();
        RandomizeObjects("Leap");
        RandomizeObjects("Heart");
        RandomizeSolidObjects();
    }
}
