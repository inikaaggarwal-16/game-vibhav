using UnityEngine;
using System.Collections.Generic;

public class TimeLeapOnCollision : MonoBehaviour
{
    // Time interval to store position updates (in seconds)
    public float timeInterval = 1f;  // Store every second
    private float timer = 0f;

    // List to store the player's positions over time
    private List<Vector3> positionHistory = new List<Vector3>();

    // Reference to the player
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // Find the player by its tag
    }

    void Update()
    {
        // Store the player's position over time
        if (player != null)
        {
            timer += Time.deltaTime;

            // Save the position at regular intervals (every "timeInterval" seconds)
            if (timer >= timeInterval)
            {
                positionHistory.Add(player.transform.position);  // Add current position to history
                timer = 0f;  // Reset the timer
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with the "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if we have enough history to go back 3 seconds
            if (positionHistory.Count >= 3)
            {
                // Get the position from 3 seconds ago
                Vector3 positionToRewind = positionHistory[positionHistory.Count - 3]; // 3 seconds ago
                
                // Move the player to that position
                player.transform.position = positionToRewind;
                Debug.Log("Player rewound to: " + positionToRewind);
            }
        }
    }
}

