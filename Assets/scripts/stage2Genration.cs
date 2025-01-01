
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;
public class ProceduralPropGenerator : MonoBehaviour
{
    public Vector2 gridOrigin = new Vector2(-6.25f, -2.75f); // Bottom-left corner of the grid
    public int gridWidth = 8; // Width of the grid in cells
    public int gridHeight = 8; // Height of the grid in cells
    public float cellSize = 0.5f; // Size of each cell
    public float minDistance = 1.0f; // Minimum distance between objects of the same tag
    public float boundaryMargin = 1; // Margin near the boundary where objects should not spawn
    public GameObject insectPrefab; // Reference to the insect prefab
    public GameObject stage3door;
    public float activationRange;
    public Button openRightButton;

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

    void Update()
    {
        if (IsNearObject())
        {
            // If the player is near, show the button.
            if (openRightButton != null && !openRightButton.gameObject.activeSelf)
            {
                openRightButton.gameObject.SetActive(true);
            }
        }
        else
        {
            // If the player is not near, hide the button.
            if (openRightButton != null && openRightButton.gameObject.activeSelf)
            {
                openRightButton.gameObject.SetActive(false);
            }
        }
    }

    bool IsNearObject()
    {
        // Find the player by its "Player" tag.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject rightupdoor = GameObject.FindGameObjectWithTag("RightDoorUp");
        GameObject rightdowndoor = GameObject.FindGameObjectWithTag("RightDoorDown");

        if (player != null)
        {
            // Check if the player is within range of this object.
            return (Vector2.Distance(player.transform.position, rightupdoor.transform.position) <= activationRange ||
             (Vector2.Distance(player.transform.position, rightdowndoor.transform.position) >= activationRange + 0.1f &&
              Vector2.Distance(player.transform.position, rightdowndoor.transform.position) <= activationRange + 0.25f));
        }
        else
        {
            Debug.LogError("Player not found. Make sure the Player tag is assigned.");
            return false;
        }
    }

    public void OnActivateButtonClicked()
    {
        if (IsNearObject())
        {
           GameObject player = GameObject.FindGameObjectWithTag("Player");
           
           Vector3 teleportPosition = stage3door.transform.position + new Vector3(0, 0.35f, 0);
           player.transform.position = teleportPosition;
        }
        else
        {
            Debug.Log("Player is not near the object.");
        }
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
// In your GameManager or a relevant class
    public static string selectedCornerKey;
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
        // Define the corner positions

    { "TopLeft", new List<Vector2>
        {
            new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 1) * cellSize), // Top-left corner
            new Vector2(gridOrigin.x + cellSize, gridOrigin.y + (gridHeight - 1) * cellSize), // Right neighbor
            new Vector2(gridOrigin.x, gridOrigin.y + (gridHeight - 2) * cellSize), // Top neighbor
            new Vector2(gridOrigin.x + cellSize, gridOrigin.y + (gridHeight - 2) * cellSize) // Diagonal neighbor
        }
    },
    { "TopRight", new List<Vector2>
        {
            new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize), // Top-right corner
            new Vector2(gridOrigin.x + (gridWidth - 2) * cellSize, gridOrigin.y + (gridHeight - 1) * cellSize), // Left neighbor
            new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + (gridHeight - 2) * cellSize), // Bottom neighbor
            new Vector2(gridOrigin.x + (gridWidth - 2) * cellSize, gridOrigin.y + (gridHeight - 2) * cellSize) // Diagonal neighbor
        }
    },
    { "BottomLeft", new List<Vector2>
        {
            gridOrigin, // Bottom-left corner
            new Vector2(gridOrigin.x + cellSize, gridOrigin.y), // Right neighbor
            new Vector2(gridOrigin.x, gridOrigin.y + cellSize), // Top neighbor
            new Vector2(gridOrigin.x + cellSize, gridOrigin.y + cellSize) // Diagonal neighbor
        }
    },
    { "BottomRight", new List<Vector2>
        {
            new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y), // Bottom-right corner
            new Vector2(gridOrigin.x + (gridWidth - 2) * cellSize, gridOrigin.y), // Left neighbor
            new Vector2(gridOrigin.x + (gridWidth - 1) * cellSize, gridOrigin.y + cellSize), // Top neighbor
            new Vector2(gridOrigin.x + (gridWidth - 2) * cellSize, gridOrigin.y + cellSize) // Diagonal neighbor
        }
    }
};


    


    // Randomly select one corner from the dictionary
var randomCornerKey = cornerPositions.Keys.ElementAt(Random.Range(0,cornerPositions.Count)); 
var randomCornerPositions = cornerPositions[randomCornerKey];

// Store the randomly chosen key for use in the pickup script.
   selectedCornerKey = randomCornerKey; // or use your manager class name.
   Debug.Log("selectedcornerkey " + selectedCornerKey);

// Check if the first position in the randomly selected corner contains a Lamp.
if (IsObjectAtPosition(randomCornerPositions[0], objectPositions["Lamp"]))
{
    InstantiateInsectsAtPositions(randomCornerPositions);
}

// After checking the randomly selected corner, check the remaining three corners for Fire.
foreach (var corner in cornerPositions)
{
    // Skip the randomly selected corner
    if (corner.Key == randomCornerKey) continue;

    // Check if the first position in the corner contains Fire
    if (IsObjectAtPosition(corner.Value[0], objectPositions["Fire"]))
    {
        // Instantiate 
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