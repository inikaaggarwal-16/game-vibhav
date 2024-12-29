using System.Collections.Generic;
using UnityEngine;

public class RandomObjectManager : MonoBehaviour
{
    public Vector2 gridOrigin = new Vector2(430, 190); // Bottom-left corner of the grid
    public int gridWidth = 8; // Width of the grid in cells
    public int gridHeight = 8; // Height of the grid in cells
    public float cellSize = 0.5f; // Size of each cell
    public float minDistance = 1.0f; // Minimum distance between objects of the same tag
    public float boundaryMargin = 1;

    private HashSet<Vector2> occupiedCells = new HashSet<Vector2>(); // All occupied cells
    private HashSet<Vector2> heartOccupiedCells = new HashSet<Vector2>(); // Cells occupied by Heart
    private HashSet<Vector2> solidOccupiedCells = new HashSet<Vector2>(); // Cells occupied by Solid
    private HashSet<Vector2> leapOccupiedCells = new HashSet<Vector2>(); // Cells occupied by Leap
    private HashSet<Vector2> portalOccupiedCells = new HashSet<Vector2>(); // Cells occupied by Portal (fixed positions)

    private void Start()
    {
        // Store the positions of all Portal objects (fixed positions)
        StorePortalPositions();

        // Randomize Heart, Leap and Solid objects
        RandomizeObjects("Leap");
        RandomizeObjects("Heart");
        RandomizeSolidObjects();
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
                // Generate a random position within the grid
                int randomX = Random.Range(0, gridWidth);
                int randomY = Random.Range(0, gridHeight);

                newPosition = new Vector2(
                    gridOrigin.x + randomX * cellSize,
                    gridOrigin.y + randomY * cellSize
                );

                attempts++;
                if (attempts > 100) break; // Avoid infinite loops
            }
            // Ensure the position is not occupied by another object, not too close, and not overlapping with any Portal position
            while (occupiedCells.Contains(newPosition) || IsTooClose(newPosition) ||
                   portalOccupiedCells.Contains(newPosition) || 
                   (tag == "Heart" && heartOccupiedCells.Contains(newPosition)) ||
                   (tag == "Solid" && solidOccupiedCells.Contains(newPosition)) ||
                   (tag == "Leap" && leapOccupiedCells.Contains(newPosition)));

            // Place the object and mark the cell as occupied
            obj.transform.position = newPosition;
            occupiedCells.Add(newPosition);

            // Track the position as occupied by the specific tag
            if (tag == "Heart")
            {
                heartOccupiedCells.Add(newPosition);
            }
            else if (tag == "Solid")
            {
                solidOccupiedCells.Add(newPosition);
            }
            else if (tag == "Leap")
            {
                leapOccupiedCells.Add(newPosition);
            }
        }
    }

    // Randomizes Solid objects, ensuring they don't overlap with other tags and the Portal
    public void RandomizeSolidObjects()
    {
        GameObject[] solidObjects = GameObject.FindGameObjectsWithTag("Solid");

        foreach (GameObject obj in solidObjects)
        {
            Vector2 newPosition;
            int attempts = 0;

            do
            {
                int randomX = (int)Random.Range(boundaryMargin , gridWidth-boundaryMargin);
                int randomY = (int)Random.Range(boundaryMargin, gridHeight-boundaryMargin);

                newPosition = new Vector2(
                    gridOrigin.x + randomX * cellSize,
                    gridOrigin.y + randomY * cellSize
                );

                attempts++;
                if (attempts > 100) break; // Avoid infinite loops
            }
            // Ensure no overlap with other objects and Portal positions
            while (occupiedCells.Contains(newPosition) || IsTooClose(newPosition) ||
                   heartOccupiedCells.Contains(newPosition) || solidOccupiedCells.Contains(newPosition) ||
                   leapOccupiedCells.Contains(newPosition) || portalOccupiedCells.Contains(newPosition));

            // Adjust Y position for Solid objects
            newPosition.y -= 0.24f;

            // Place the object and mark the cell as occupied
            obj.transform.position = newPosition;
            occupiedCells.Add(newPosition);

            // Track this position as occupied by a Solid object
            solidOccupiedCells.Add(newPosition);
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
        heartOccupiedCells.Clear();
        solidOccupiedCells.Clear();
        leapOccupiedCells.Clear();
        portalOccupiedCells.Clear(); // Keep portal positions intact, don't clear them

        RandomizeObjects("Leap");
        RandomizeObjects("Heart");
        RandomizeSolidObjects();
    }
}
// using System.Collections.Generic;
// using System.Numerics;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.InputSystem;
// using UnityEngine.Tilemaps;

// public class SimpleDungonGeneration : MonoBehaviour
// {
//     [SerializeField]
//     private Vector2Int roomsize = new Vector2Int(10,10);
//     [SerializeField]
//     private Tilemap roomMap, colliderMap;
//     [SerializeField]
//     private TileBase roomFloorTile, pathFloorTile;
//     [SerializeField]
//     private InputActionReference generate;
//     public UnityEvent OnFinishedRoomGeneration;
//     public static List<Vector2Int>fourDirection = new(){
//         Vector2Int.up,
//         Vector2Int.down,
//         Vector2Int.left,
//         Vector2Int.right
//     };

//     private DungeonData dungeonData;
    
//     private void Awake(){
//         dungeonData = FindObjectOfType<DungeonData>();
//         if(dungeonData == null)
//            dungeonData = gameObject.AddComponent<DungeonData>();
        
//         generation.action.performed += generate;
//     }
    
//     private void Generate(InputAction.CallbackContext obj){
//         dungeonData.Reset();

//         dungeonData Rooms Add(
//             GenerateRectangulatRoomAt(Vector2.Zero, roomSize)
//         );
        
        
//     }
//     }