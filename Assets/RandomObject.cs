using System.Collections.Generic;
using UnityEngine;

public class RandomObjectManager : MonoBehaviour
{
    public Vector2 gridOrigin = new Vector2(430, 190); // Bottom-left corner of the grid
    public int gridWidth = 8; // Width of the grid in cells
    public int gridHeight = 8; // Height of the grid in cells
    public float cellSize = 0.5f; // Size of each cell
    public float minDistance = 0.5f; // Minimum distance within a cell for fine adjustments

    private HashSet<Vector2> occupiedCells = new HashSet<Vector2>();

    private void Start()
    {
        DistributeObjects("Leap");
        DistributeObjects("Heart");
        DistributeObjects("Solid", offsetY: -0.25f);
    }

    private void DistributeObjects(string tag, float offsetY = 0f)
    {
        // Get all objects with the specified tag
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        // Generate a list of all possible grid positions
        List<Vector2> availableCells = GenerateGridCells();

        // Shuffle the list to randomize cell assignment
        Shuffle(availableCells);

        foreach (GameObject obj in objects)
        {
            if (availableCells.Count == 0)
            {
                Debug.LogWarning($"Not enough cells to distribute all {tag} objects!");
                break;
            }

            // Assign the first available cell
            Vector2 cell = availableCells[0];
            availableCells.RemoveAt(0);

            // Fine-tune the position within the cell to ensure a natural look
            Vector2 fineTunedPosition = FineTunePosition(cell, offsetY);

            // Place the object
            obj.transform.position = fineTunedPosition;

            // Mark the cell as occupied
            occupiedCells.Add(cell);
        }
    }

    private List<Vector2> GenerateGridCells()
    {
        List<Vector2> cells = new List<Vector2>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 cellPosition = new Vector2(
                    gridOrigin.x + x * cellSize,
                    gridOrigin.y + y * cellSize
                );
                if (!occupiedCells.Contains(cellPosition))
                {
                    cells.Add(cellPosition);
                }
            }
        }

        return cells;
    }

    private Vector2 FineTunePosition(Vector2 cell, float offsetY)
    {
        float fineX = Random.Range(-minDistance / 2, minDistance / 2);
        float fineY = Random.Range(-minDistance / 2, minDistance / 2);
        return new Vector2(cell.x + fineX, cell.y + fineY + offsetY);
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void TeleportAndRedistribute()
    {
        occupiedCells.Clear();
        DistributeObjects("Leap");
        DistributeObjects("Heart");
        DistributeObjects("Solid", offsetY: -0.25f);
    }
}
