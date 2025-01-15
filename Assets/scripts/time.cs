// using UnityEngine;
// using System.Collections.Generic;

// public class TimeLeapOnCollision : MonoBehaviour
// {
//     public float timeInterval = 1f;
//     private float timer = 0f;
//     private List<Vector3> positionHistory = new List<Vector3>();
//     private GameObject player;

//     void Start()
//     {
//         player = GameObject.FindGameObjectWithTag("Player");
//     }

//     void Update()
//     {
//         if (player != null)
//         {
//             timer += Time.deltaTime;
//             if (timer >= timeInterval)
//             {
//                 positionHistory.Add(player.transform.position);
//                 timer = 0f;
//             }
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Player"))
//         {
//             if (positionHistory.Count >= 3)
//             {
//                 Vector3 positionToRewind = positionHistory[positionHistory.Count - 3];
//                 player.transform.position = positionToRewind;
//             }
//         }
//     }
// }


using UnityEngine;
using System.Collections.Generic;

public class TimeLeapOnCollision : MonoBehaviour
{
    public float timeInterval = 1f; // Time interval to record positions
    private float timer = 0f;
    private List<Vector2> positionHistory = new List<Vector2>(); // Store Vector2 for 2D positions
    private GameObject player;
    private Rigidbody2D playerRb;
    private playerMovement playerMovementScript;
    private float gameStartTime;

    void Start()
    {
        gameStartTime = Time.time; // Record the game start time
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            playerMovementScript = player.GetComponent<playerMovement>();
        }
    }

    void Update()
    {
        if (player != null)
        {
            timer += Time.deltaTime;
            if (timer >= timeInterval)
            {
                // Record the player's current position periodically
                positionHistory.Add(playerRb.position);
                timer = 0f;

                // Optional: Limit history size to prevent excessive memory use
                if (positionHistory.Count > 100) // Example limit
                {
                    positionHistory.RemoveAt(0);
                }
            }
        }
    }

    // Method to record position from playerMovement
    public void RecordPosition(Vector2 position)
    {
        positionHistory.Add(position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the game has been running for at least 3 seconds
            if (Time.time - gameStartTime >= 3f)
            {
                if (positionHistory.Count >= 3)
                {
                    // Get the position to rewind to (third-last position)
                    Vector2 positionToRewind = positionHistory[positionHistory.Count - 3];

                    // Clear movement states
                    if (playerMovementScript != null)
                    {
                        playerMovementScript.ResetMovement();
                    }

                    // Teleport player using Rigidbody2D and reset velocity
                    playerRb.position = positionToRewind;
                    playerRb.linearVelocity = Vector2.zero;

                    // Remove the rewound positions from history
                    positionHistory.RemoveRange(positionHistory.Count - 3, 3);
                }
            }
            else
            {
                // Optional: Add a log or some feedback if needed
                Debug.Log("Game has not been running for 3 seconds yet. No teleportation.");
            }
        }
    }
}
