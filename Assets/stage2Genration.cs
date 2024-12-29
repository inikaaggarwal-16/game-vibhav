using System.Collections.Generic;
using UnityEngine;

public class ProceduralPropGenerator : MonoBehaviour
{
    public Vector2 gridOrigin = new Vector2(430, 190); // Bottom-left corner of the grid
    public int gridWidth = 8; // Width of the grid in cells
    public int gridHeight = 8; // Height of the grid in cells
    public float cellSize = 0.5f; // Size of each cell
    public float minDistance = 1.0f; // Minimum distance between objects of the same tag
    public float boundaryMargin = 1; // Margin near the boundary where objects should not spawn

    private HashSet<Vector2> occupiedCells = new HashSet<Vector2>(); // All occupied cells
    private HashSet<Vector2> WrongOccupiedCells = new HashSet<Vector2>(); // Cells occupied by wrong door
    private HashSet<Vector2> RightOccupiedCells = new HashSet<Vector2>(); // Cells occupied by right door
    private HashSet<Vector2> portalOccupiedCells = new HashSet<Vector2>(); // Cells occupied by Portal (fixed positions)

    // New variables for Fire and Lamp props
    public GameObject firePrefab;
    public GameObject lampPrefab;

    private void Start()
    {
        // Store the positions of all Portal objects (fixed positions)
        StorePortalPositions();

        // Randomize Fire and Lamp objects in the corners
        RandomizeProps();

        // Randomize other objects
        RandomizeObjects("WrongDoor");
        RandomizeObjects("RightDoorUp");
        RandomizeObjects("RightDoorDown");

        // After the game starts, adjust Y position for the doors
        AdjustDoorPositions();
    }

    // Store the positions of Portal objects (fixed positions)
    private void StorePortalPositions()
    {
        GameObject[] portalObjects = GameObject.FindGameObjectsWithTag("Portal");
        foreach (GameObject portal in portalObjects)
        {
            Vector2 portalPosition = portal.transform.position;
            portalOccupiedCells.Add(portalPosition); // Mark portal positions as occupied
            occupiedCells.Add(portalPosition); // Also mark in the general occupied cells
        }
    }

    // Randomizes Fire and Lamp objects in the corners
    private void RandomizeProps()
    {
        // Define the 4 corners
        Vector2[] corners = new Vector2[]
        {
            new Vector2(gridOrigin.x, gridOrigin.y),                                                           // Bottom-left corner
            new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y),                              // Bottom-right corner
            new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 1) * cellSize),                             // Top-left corner
            new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize) // Top-right corner
        };

        // List to hold the props
        GameObject[] props = new GameObject[] { firePrefab, lampPrefab, firePrefab, lampPrefab };

        // Randomize the order of props
        ShuffleArray(props);

        // Place 1 Fire and 1 Lamp in the corners
        for (int i = 0; i < 4; i++)
        {
            Vector2 corner = corners[i];
            InstantiateProp(props[i], corner);
            occupiedCells.Add(corner); // Mark the corner as occupied
        }
    }

    // Helper method to shuffle an array (for random prop placement)
    private void ShuffleArray(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    // Instantiate a prop (Fire or Lamp) at a specified position
    private void InstantiateProp(GameObject prefab, Vector2 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.localScale = new Vector3(0.25f, 0.25f, 1); // Halve the size (0.25 to fit within the 0.5 cell)
    }

    // Randomizes objects based on the tag, ensuring no overlap between different object types and Portal positions
    public void RandomizeObjects(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            Vector2 newPosition;
            int attempts = 0;

            do
            {
                // Generate a random position within the grid, ensuring we stay within boundaries
                int randomX = (int)Random.Range(boundaryMargin, gridWidth - boundaryMargin);
                int randomY = (int)Random.Range(boundaryMargin, gridHeight - boundaryMargin);

                // Explicitly cast randomX and randomY to float before multiplying by cellSize
                newPosition = new Vector2(
                    gridOrigin.x + (float)randomX * cellSize,  // Cast randomX to float
                    gridOrigin.y + (float)randomY * cellSize   // Cast randomY to float
                );

                attempts++;
                if (attempts > 100) break; // Avoid infinite loops
            }
            // Ensure the position is not occupied by another object, not too close, and not overlapping with any Portal position
            while (occupiedCells.Contains(newPosition) || IsTooClose(newPosition) ||
                   portalOccupiedCells.Contains(newPosition) || 
                   (tag == "WrongDoor" && WrongOccupiedCells.Contains(newPosition)) ||
                   ((tag == "RightDoorUp" || tag == "RightDoorDown") && RightOccupiedCells.Contains(newPosition))
                   );

            // Place the object and mark the cell as occupied
            obj.transform.position = newPosition;
            occupiedCells.Add(newPosition);

            // Track the position as occupied by the specific tag
            if (tag == "WrongDoor")
            {
                WrongOccupiedCells.Add(newPosition);
            }
            else if (tag == "RightDoorUp" || tag == "RightDoorDown")
            {
                RightOccupiedCells.Add(newPosition);
            }
        }
    }

    // Adjust Y position for the doors after the game starts
    private void AdjustDoorPositions()
    {
        GameObject[] wrongDoors = GameObject.FindGameObjectsWithTag("WrongDoor");
        GameObject[] rightDoorsUp = GameObject.FindGameObjectsWithTag("RightDoorUp");
        GameObject[] rightDoorsDown = GameObject.FindGameObjectsWithTag("RightDoorDown");

        AdjustYPositionForDoors(wrongDoors);
        AdjustYPositionForDoors(rightDoorsUp);
        AdjustYPositionForDoors(rightDoorsDown);
    }

    // Helper method to adjust the Y position of the doors
    private void AdjustYPositionForDoors(GameObject[] doors)
    {
        foreach (GameObject door in doors)
        {
            Vector3 position = door.transform.position;
            position.y -= 0.26f; // Decrease the Y position
            door.transform.position = position;
        }
    }

    // Check if an object is too close to any other object in the grid
    private bool IsTooClose(Vector2 position)
    {
        foreach (Vector2 occupied in occupiedCells)
        {
            if (Vector2.Distance(position, occupied) < minDistance)
            {
                return true;
            }
        }
        return false;
    }

    // Clears all occupied positions and re-randomizes all objects
    public void TeleportAndRandomize()
    {
        occupiedCells.Clear();
        WrongOccupiedCells.Clear();
        RightOccupiedCells.Clear();
        portalOccupiedCells.Clear(); // Keep portal positions intact, don't clear them

        RandomizeObjects("WrongDoor");
        RandomizeObjects("RightDoorUp");
        RandomizeObjects("RightDoorDown");
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ProceduralPropGenerator : MonoBehaviour
// {
//     public Vector2 gridOrigin = new Vector2(430, 190); // Bottom-left corner of the grid
//     public int gridWidth = 8; // Width of the grid in cells
//     public int gridHeight = 8; // Height of the grid in cells
//     public float cellSize = 0.5f; // Size of each cell
//     public float minDistance = 1.0f; // Minimum distance between objects of the same tag
//     public float boundaryMargin = 1; // Margin near the boundary where objects should not spawn

//     private HashSet<Vector2> occupiedCells = new HashSet<Vector2>(); // All occupied cells
//     private HashSet<Vector2> portalOccupiedCells = new HashSet<Vector2>(); // Cells occupied by Portal (fixed positions)

//     // Prefabs for Fire, Lamp, and Insects
//     public GameObject firePrefab;
//     public GameObject lampPrefab;
//     public GameObject insectPrefab; // Insect prefab

//     private void Start()
//     {
//         // Store the positions of all Portal objects (fixed positions)
//         StorePortalPositions();

//         // Randomize Fire and Lamp objects in the corners
//         RandomizeProps();

//         // After Fire and Lamp have been spawned, spawn insects at the corners
//         StartCoroutine(SpawnInsectsAtCornersAfterProps());

//         // Randomize other objects
//         RandomizeObjects("WrongDoor");
//         RandomizeObjects("RightDoorUp");
//         RandomizeObjects("RightDoorDown");

//         // After the game starts, adjust Y position for the doors
//         AdjustDoorPositions();
//     }

//     // Store the positions of Portal objects (fixed positions)
//     private void StorePortalPositions()
//     {
//         GameObject[] portalObjects = GameObject.FindGameObjectsWithTag("Portal");
//         foreach (GameObject portal in portalObjects)
//         {
//             Vector2 portalPosition = portal.transform.position;
//             portalOccupiedCells.Add(portalPosition); // Mark portal positions as occupied
//             occupiedCells.Add(portalPosition); // Also mark in the general occupied cells
//         }
//     }

//     // Randomizes Fire and Lamp objects in the corners
//     private void RandomizeProps()
//     {
//         // Define the 4 corners
//         Vector2[] corners = new Vector2[]
//         {
//             new Vector2(gridOrigin.x, gridOrigin.y), // Bottom-left corner
//             new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y), // Bottom-right corner
//             new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 1) * cellSize), // Top-left corner
//             new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize) // Top-right corner
//         };

//         // List to hold the props (Fire and Lamp)
//         GameObject[] props = new GameObject[] { firePrefab, lampPrefab, firePrefab, lampPrefab };

//         // Randomize the order of props (Fire and Lamp can be in any corner)
//         ShuffleArray(props);

//         // Place Fire and Lamp at the corners
//         for (int i = 0; i < 4; i++)
//         {
//             Vector2 corner = corners[i];
//             InstantiateProp(props[i], corner);
//             occupiedCells.Add(corner); // Mark the corner as occupied
//         }
//     }

//     // Helper method to shuffle an array (for random prop placement)
//     private void ShuffleArray(GameObject[] array)
//     {
//         for (int i = 0; i < array.Length; i++)
//         {
//             int randomIndex = Random.Range(i, array.Length);
//             GameObject temp = array[i];
//             array[i] = array[randomIndex];
//             array[randomIndex] = temp;
//         }
//     }

//     // Instantiate a prop (Fire or Lamp) at a specified position
//     private void InstantiateProp(GameObject prefab, Vector2 position)
//     {
//         GameObject obj = Instantiate(prefab, position, Quaternion.identity);
//         obj.transform.localScale = new Vector3(0.25f, 0.25f, 1); // Halve the size (0.25 to fit within the 0.5 cell)
//     }

//     // Wait for props to be spawned and then spawn insects at the corners
//     private IEnumerator SpawnInsectsAtCornersAfterProps()
//     {
//         // Wait for the props to be placed first
//         yield return new WaitForSeconds(1f);

//         // Define the 4 corners
//         Vector2[] corners = new Vector2[]
//         {
//             new Vector2(gridOrigin.x, gridOrigin.y), // Bottom-left corner
//             new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y), // Bottom-right corner
//             new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 1) * cellSize), // Top-left corner
//             new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize) // Top-right corner
//         };

//         // Randomly pick a corner and decide if it's Fire or Lamp
//         int randomCornerIndex = Random.Range(0, 4);
//         Vector2 chosenCorner = corners[randomCornerIndex];
//         GameObject cornerObject = GetObjectAtPosition(chosenCorner);

//         if (cornerObject != null && cornerObject.CompareTag("Fire"))
//         {
//             // If the randomly chosen corner is Fire, spawn insects there
//             SpawnInsectsAtCorner(chosenCorner);
//         }

//         // For all remaining 3 corners, spawn insects only if it's Fire (not Lamp)
//         for (int i = 0; i < 4; i++)
//         {
//             if (i == randomCornerIndex) continue; // Skip the randomly chosen corner

//             Vector2 corner = corners[i];
//             cornerObject = GetObjectAtPosition(corner);

//             if (cornerObject != null && cornerObject.CompareTag("Fire"))
//             {
//                 // If it's Fire, spawn insects at this corner
//                 SpawnInsectsAtCorner(corner);
//             }
//         }
//     }

//     // Check what object is at the given position (corner)
//     private GameObject GetObjectAtPosition(Vector2 position)
//     {
//         Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f); // Small radius to check the position
//         foreach (Collider2D collider in colliders)
//         {
//             return collider.gameObject;
//         }
//         return null;
//     }

//     // Spawn insects at a specific corner
//     private void SpawnInsectsAtCorner(Vector2 corner)
//     {
//         // Spawn 3 insects at this corner with slightly different sizes
//         for (int i = 0; i < 3; i++)
//         {
//             float sizeRandomness = Random.Range(0.8f, 1.2f); // Random size variation
//             GameObject insect = Instantiate(insectPrefab, corner, Quaternion.identity);
//             insect.transform.localScale = new Vector3(sizeRandomness, sizeRandomness, 1);
//             StartCoroutine(MoveInsect(insect, corner));
//         }
//     }

//     // Coroutine to move the insect randomly around the corner
//     private IEnumerator MoveInsect(GameObject insect, Vector2 corner)
//     {
//         float moveRadius = 1.0f; // Define how far the insect can move from the corner
//         float speed = 0.5f; // Movement speed

//         while (true)
//         {
//             // Move to a random position around the corner within the moveRadius
//             Vector2 targetPosition = corner + new Vector2(Random.Range(-moveRadius, moveRadius), Random.Range(-moveRadius, moveRadius));
            
//             // Smoothly move the insect to the target position
//             while (Vector2.Distance(insect.transform.position, targetPosition) > 0.05f)
//             {
//                 insect.transform.position = Vector2.MoveTowards(insect.transform.position, targetPosition, speed * Time.deltaTime);
//                 yield return null;
//             }

//             // Wait for a brief moment before moving to the next random position
//             yield return new WaitForSeconds(Random.Range(1f, 3f));
//         }
//     }

//     // Randomizes objects based on the tag, ensuring no overlap between different object types and Portal positions
//     public void RandomizeObjects(string tag)
//     {
//         GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

//         foreach (GameObject obj in objects)
//         {
//             Vector2 newPosition;
//             int attempts = 0;

//             do
//             {
//                 // Generate a random position within the grid, ensuring we stay within boundaries
//                 int randomX = (int)Random.Range(boundaryMargin, gridWidth - boundaryMargin);
//                 int randomY = (int)Random.Range(boundaryMargin, gridHeight - boundaryMargin);

//                 // Explicitly cast randomX and randomY to float before multiplying by cellSize
//                 newPosition = new Vector2(
//                     gridOrigin.x + (float)randomX * cellSize,  // Cast randomX to float
//                     gridOrigin.y + (float)randomY * cellSize   // Cast randomY to float
//                 );

//                 attempts++;
//                 if (attempts > 100) break; // Avoid infinite loops
//             }
//             // Ensure the position is not occupied by another object, not too close, and not overlapping with any Portal position
//             while (occupiedCells.Contains(newPosition) || IsTooClose(newPosition) ||
//                    portalOccupiedCells.Contains(newPosition) || 
//                    (tag == "WrongDoor" && WrongOccupiedCells.Contains(newPosition)) ||
//                    ((tag == "RightDoorUp" || tag == "RightDoorDown") && RightOccupiedCells.Contains(newPosition))
//                    );

//             // Place the object and mark the cell as occupied
//             obj.transform.position = newPosition;
//             occupiedCells.Add(newPosition);

//             // Track the position as occupied by the specific tag
//             if (tag == "WrongDoor")
//             {
//                 WrongOccupiedCells.Add(newPosition);
//             }
//             else if (tag == "RightDoorUp" || tag == "RightDoorDown")
//             {
//                 RightOccupiedCells.Add(newPosition);
//             }
//         }
//     }

//     // Adjust Y position for the doors after the game starts
//     private void AdjustDoorPositions()
//     {
//         GameObject[] wrongDoors = GameObject.FindGameObjectsWithTag("WrongDoor");
//         GameObject[] rightDoorsUp = GameObject.FindGameObjectsWithTag("RightDoorUp");
//         GameObject[] rightDoorsDown = GameObject.FindGameObjectsWithTag("RightDoorDown");

//         AdjustYPositionForDoors(wrongDoors);
//         AdjustYPositionForDoors(rightDoorsUp);
//         AdjustYPositionForDoors(rightDoorsDown);
//     }

//     // Helper method to adjust the Y position of the doors
//     private void AdjustYPositionForDoors(GameObject[] doors)
//     {
//         foreach (GameObject door in doors)
//         {
//             Vector3 position = door.transform.position;
//             position.y -= 0.21f; // Adjust by a small amount (e.g., -0.21f)
//             door.transform.position = position;
//         }
//     }
// }
