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
//     private HashSet<Vector2> WrongOccupiedCells = new HashSet<Vector2>(); // Cells occupied by wrong door
//     private HashSet<Vector2> RightOccupiedCells = new HashSet<Vector2>(); // Cells occupied by right door
//     private HashSet<Vector2> portalOccupiedCells = new HashSet<Vector2>(); // Cells occupied by Portal (fixed positions)

//     // New variables for Fire and Lamp props
//     public GameObject firePrefab;
//     public GameObject lampPrefab;

//     private void Start()
//     {
//         // Store the positions of all Portal objects (fixed positions)
//         StorePortalPositions();

//         // Randomize Fire and Lamp objects in the corners
//         RandomizeProps();

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
//             new Vector2(gridOrigin.x, gridOrigin.y),                                                           // Bottom-left corner
//             new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y),                              // Bottom-right corner
//             new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 1) * cellSize),                             // Top-left corner
//             new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize) // Top-right corner
//         };

//         // List to hold the props
//         GameObject[] props = new GameObject[] { firePrefab, lampPrefab, firePrefab, lampPrefab };

//         // Randomize the order of props
//         ShuffleArray(props);

//         // Place 1 Fire and 1 Lamp in the corners
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
//                     newPosition = new Vector2(
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
//             position.y -= 0.26f; // Decrease the Y position
//             door.transform.position = position;
//         }
//     }

//     // Check if an object is too close to any other object in the grid
//     private bool IsTooClose(Vector2 position)
//     {
//         foreach (Vector2 occupied in occupiedCells)
//         {
//             if (Vector2.Distance(position, occupied) < minDistance)
//             {
//                 return true;
//             }
//         }
//         return false;
//     }

//     // Clears all occupied positions and re-randomizes all objects
//     public void TeleportAndRandomize()
//     {
//         occupiedCells.Clear();
//         WrongOccupiedCells.Clear();
//         RightOccupiedCells.Clear();
//         portalOccupiedCells.Clear(); // Keep portal positions intact, don't clear them

//         RandomizeObjects("WrongDoor");
//         RandomizeObjects("RightDoorUp");
//         RandomizeObjects("RightDoorDown");
//     }
// }



using System.Collections.Generic;
using UnityEngine;

public class ProceduralPropGenerator : MonoBehaviour
{
    public Vector2 gridOrigin = new Vector2(-6.25f, -2.75f); // Bottom-left corner of the grid
    public int gridWidth = 8; // Width of the grid in cells
    public int gridHeight = 8; // Height of the grid in cells
    public float cellSize = 0.5f; // Size of each cell
    public float minDistance = 1.0f; // Minimum distance between objects of the same tag
    public float boundaryMargin = 1; // Margin near the boundary where objects should not spawn
    public GameObject insectPrefab; // Reference to the insect prefab

    private HashSet<Vector2> occupiedCells = new HashSet<Vector2>(); // All occupied cells
    private HashSet<Vector2> WrongOccupiedCells = new HashSet<Vector2>(); // Cells occupied by wrong door
    private HashSet<Vector2> RightOccupiedCells = new HashSet<Vector2>(); // Cells occupied by right door
    private HashSet<Vector2> portalOccupiedCells = new HashSet<Vector2>(); // Cells occupied by Portal (fixed positions)

    private void Start()
    {
        // Store the positions of all Portal objects (fixed positions)
        StorePortalPositions();

        // Randomize Fire and Lamp objects in the corners
        RandomizeCornerProps();

        // Randomize other objects
        RandomizeObjects("WrongDoor");
        RandomizeObjects("RightDoorUp");
        RandomizeObjects("RightDoorDown");

        // After the game starts, adjust Y position for the doors
        AdjustDoorPositions();

        // Generate insect props near corners
        GenerateInsectPropsNearCorners();
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
    private void RandomizeCornerProps()
    {
        // Get all objects with the tags "Fire" and "Lamp"
        GameObject[] fireProps = GameObject.FindGameObjectsWithTag("Fire");
        GameObject[] lampProps = GameObject.FindGameObjectsWithTag("Lamp");

        // Ensure there are enough objects in the scene
        if (fireProps.Length == 0 || lampProps.Length == 0)
        {
            Debug.LogError("Insufficient Fire or Lamp objects in the scene.");
            return;
        }

        // Combine all objects into a single list
        List<GameObject> allProps = new List<GameObject>();
        allProps.AddRange(fireProps);
        allProps.AddRange(lampProps);

        // Shuffle the list of all props
        ShuffleList(allProps);

        // Select 4 objects for placement
        HashSet<GameObject> selectedProps = new HashSet<GameObject>();
        selectedProps.Add(fireProps[0]); // Ensure at least one fire is selected
        selectedProps.Add(lampProps[0]); // Ensure at least one lamp is selected

        // Add remaining random props until we have exactly 4
        foreach (GameObject prop in allProps)
        {
            if (selectedProps.Count >= 4) break;
            selectedProps.Add(prop);
        }

        // Define the 4 corners
        Vector2[] corners = new Vector2[]
        {
            new Vector2(gridOrigin.x, gridOrigin.y),                                                           // Bottom-left corner
            new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y),                              // Bottom-right corner
            new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 1) * cellSize),                             // Top-left corner
            new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize) // Top-right corner
        };

        // Shuffle the corners for random placement
        ShuffleArray(corners);

        // Place the selected props at the corners
        int index = 0;
        foreach (GameObject prop in selectedProps)
        {
            Vector2 corner = corners[index];
            prop.transform.position = corner;
            occupiedCells.Add(corner); // Mark the corner as occupied
            index++;
        }
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

                newPosition = new Vector2(
                    gridOrigin.x + (float)randomX * cellSize,
                    gridOrigin.y + (float)randomY * cellSize
                );

                attempts++;
                if (attempts > 100) break; // Avoid infinite loops
            }
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

    // Helper method to shuffle a list
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Helper method to shuffle an array
    private void ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    // Generate insect props near corners
    // Generate insect props near corners based on conditions
// Generate insect props near corners based on conditions
private void GenerateInsectPropsNearCorners()
{
    // Retrieve all objects with tags "Lamp" and "Fire" and store their positions
    Dictionary<string, List<Vector2>> objectPositions = new Dictionary<string, List<Vector2>>
    {
        { "Lamp", GetTaggedObjectPositions("Lamp") },
        { "Fire", GetTaggedObjectPositions("Fire") }
    };

    // Define corner positions and their neighbors
    Dictionary<string, List<Vector2>> cornerPositions = new Dictionary<string, List<Vector2>>
    {
        { "TopLeft", new List<Vector2>
            {
                new Vector2(gridOrigin.x, gridOrigin.y), // Top-left corner
                new Vector2(gridOrigin.x + cellSize, gridOrigin.y), // Right neighbor
                new Vector2(gridOrigin.x, gridOrigin.y + cellSize), // Top neighbor
                new Vector2(gridOrigin.x + cellSize, gridOrigin.y + cellSize) // Diagonal neighbor
            }
        },
        { "TopRight", new List<Vector2>
            {
                new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y), // Top-right corner
                new Vector2(gridOrigin.x + (gridWidth - 2) * cellSize, gridOrigin.y), // Left neighbor
                new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + cellSize), // Bottom neighbor
                new Vector2(gridOrigin.x + (gridWidth - 2) * cellSize, gridOrigin.y + cellSize) // Diagonal neighbor
            }
        },
        { "BottomLeft", new List<Vector2>
            {
                new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 1) * cellSize), // Bottom-left corner
                new Vector2(gridOrigin.x + cellSize, gridOrigin.y + (gridHeight - 1) * cellSize), // Right neighbor
                new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 2) * cellSize), // Top neighbor
                new Vector2(gridOrigin.x + cellSize, gridOrigin.y + (gridHeight - 2) * cellSize) // Diagonal neighbor
            }
        },
        { "BottomRight", new List<Vector2>
            {
                new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize), // Bottom-right corner
                new Vector2(gridOrigin.x + (gridWidth - 2) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize), // Left neighbor
                new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 2) * cellSize), // Top neighbor
                new Vector2(gridOrigin.x + (gridWidth - 2) * cellSize, gridOrigin.y + (gridHeight - 2) * cellSize) // Diagonal neighbor
            }
        }
    };

    // First process one random corner with a Lamp
    foreach (var corner in cornerPositions)
    {
        if (IsObjectAtPosition(corner.Value[0], objectPositions["Lamp"]))
        {
            InstantiateInsectsAtPositions(corner.Value);
            break; // Process only one corner with a Lamp
        }
    }

    // Process other corners with a Fire
    foreach (var corner in cornerPositions)
    {
        if (IsObjectAtPosition(corner.Value[0], objectPositions["Fire"]))
        {
            InstantiateInsectsAtPositions(corner.Value);
        }
    }
}

// Get positions of all objects with the specified tag
private List<Vector2> GetTaggedObjectPositions(string tag)
{
    GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
    List<Vector2> positions = new List<Vector2>();

    foreach (GameObject obj in objects)
    {
        positions.Add(obj.transform.position);
    }

    return positions;
}

// Check if any object position matches a given position within a tolerance
private bool IsObjectAtPosition(Vector2 position, List<Vector2> objectPositions)
{
    float tolerance = 0.1f; // Small tolerance for floating-point discrepancies
    foreach (Vector2 objPos in objectPositions)
    {
        if (Vector2.Distance(position, objPos) <= tolerance)
        {
            return true;
        }
    }
    return false;
}

// Instantiate insects at the specified positions
private void InstantiateInsectsAtPositions(List<Vector2> positions)
{
    foreach (Vector2 pos in positions)
    {
        for (int i = 0; i < 3; i++) // Instantiate 3 insects at each valid position
        {
            GameObject insect = Instantiate(insectPrefab, pos, Quaternion.identity);
            Debug.Log($"Insect instantiated at: {pos}");

            // Set insect movement area (optional)
            InsectMovement insectMovement = insect.GetComponent<InsectMovement>();
            if (insectMovement != null)
            {
                insectMovement.SetValidPositions(positions);
            }
        }
    }
}



}