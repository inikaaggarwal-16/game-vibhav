
// using UnityEngine;

// public class ContinuousRotation : MonoBehaviour
// {
//     public float rotationSpeed = 50f;

//     private void Update()
//     {
//         // Rotate the object continuously
//         transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Player"))
//         {
//             // Handle teleportation
//             var playerMovement = collision.GetComponent<playerMovement>();
//             var objectCollision = collision.GetComponent<ObjectCollision>();

//             if (playerMovement != null)
//             {
//                 // Handle Fake object collision logic if applicable
//                 if (gameObject.CompareTag("Fake") && objectCollision != null)
//                 {
//                     objectCollision.count -= 2;
//                     Debug.Log("Count decreased due to teleportation. Current count: " + objectCollision.count);

//                     objectCollision.UpdateCountDisplay();
//                     objectCollision.CheckGameOver();
//                      // Teleport the player
//                     collision.transform.position = playerMovement.startingPosition;
//                     playerMovement.ResetMovement();

//                 Debug.Log("Player teleported to starting position.");
//                 }

               
//             }
//         }
//     }
// }


//stage 2
using UnityEngine;


public class ContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private bool isTeleporting = false;  // Flag to prevent immediate teleportation
    public GameObject player;

    private void Update()
    {
        // Rotate the object continuously
        //transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player") && !isTeleporting)
    {
        Debug.Log($"Player collided with: {gameObject.name}");

        var playerMovement = collision.GetComponent<playerMovement>();
        var objectCollision = collision.GetComponent<ObjectCollision>();

        if (playerMovement != null)
        {
            if (gameObject.CompareTag("Fake"))
            {
                Debug.Log("Handling Fake object logic.");

                // Decrease the count and handle related logic
                if (objectCollision != null)
                {
                    objectCollision.count -= 2;
                    Debug.Log("Count decreased due to Fake object. Current count: " + objectCollision.count);
                    Debug.Log("fake portal position: "+ collision.gameObject.transform.position);
                    Debug.Log("player position: "+ player.transform.position);

                    objectCollision.UpdateCountDisplay();
                    objectCollision.CheckGameOver();
                }

                // Teleport the player to the starting position
                collision.transform.position = playerMovement.startingPosition;
                playerMovement.ResetMovement();
                Debug.Log("Player teleported to starting position.");
            }
            else if (gameObject.CompareTag("Teleport"))
            {
                Debug.Log("Handling Teleport object logic.");

                // Find all objects with the "Teleport" tag
                GameObject[] teleportObjects = GameObject.FindGameObjectsWithTag("Teleport");

                // Find another teleport object that is not this one
                foreach (GameObject teleportObject in teleportObjects)
                {
                    if (teleportObject != gameObject)
                    {
                        // Teleport the player to the new object's position with an offset in the Y-axis
                        Vector3 teleportPosition = teleportObject.transform.position + new Vector3(0, 0.5f, 0);
                        collision.transform.position = teleportPosition;
                        playerMovement.ResetMovement();
                        Debug.Log("Player teleported to another Teleport object with offset.");
                        break;
                    }
                }
            }

            // Prevent teleportation spam
            isTeleporting = true;
            Invoke(nameof(ResetTeleportFlag), 1f); // Adjust delay time if needed
        }
        else
        {
            Debug.LogWarning("PlayerMovement component not found on collided object.");
        }
    }
}


    // Reset the teleport flag after a delay
    private void ResetTeleportFlag()
    {
        isTeleporting = false;
    }
}
