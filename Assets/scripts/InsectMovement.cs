using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectMovement : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed at which the insect moves
    private List<Vector2> validPositions = new List<Vector2>(); // List of valid positions to move between
    private Vector2 currentTarget; // The target position the insect is moving towards
    private bool isMoving = false; // Whether the insect is currently moving

    void Start()
    {
        // Set the first valid position as the target
        if (validPositions.Count > 0)
        {
            currentTarget = validPositions[Random.Range(0, validPositions.Count)];
            StartCoroutine(MoveToTarget());
        }
    }

    public void SetValidPositions(List<Vector2> positions)
    {
        validPositions = positions; // Set the movement area based on spawn location
    }

    private IEnumerator MoveToTarget()
    {
        while (true)
        {
            // If the insect is close to the target position, select a new target position
            if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
            {
                // Select a new target randomly from the valid positions
                currentTarget = validPositions[Random.Range(0, validPositions.Count)];
            }

            // Move towards the current target position
            float step = moveSpeed * Time.deltaTime; // Adjust the movement speed
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);

            // Wait for the next frame before continuing the movement
            yield return null;
        }
    }
}