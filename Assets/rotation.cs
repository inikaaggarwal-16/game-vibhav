
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
//                 }

//                 // Teleport the player
//                 collision.transform.position = playerMovement.startingPosition;
//                 playerMovement.ResetMovement();

//                 Debug.Log("Player teleported to starting position.");
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

    private void Update()
    {
        // Rotate the object continuously
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTeleporting)
        {
            // Handle teleportation
            var playerMovement = collision.GetComponent<playerMovement>();
            var objectCollision = collision.GetComponent<ObjectCollision>();

            if (playerMovement != null)
            {
                // Handle Fake object collision logic if applicable
                if (gameObject.CompareTag("Fake") && objectCollision != null)
                {
                    objectCollision.count -= 2;
                    Debug.Log("Count decreased due to teleportation. Current count: " + objectCollision.count);

                    objectCollision.UpdateCountDisplay();
                    objectCollision.CheckGameOver();
                    collision.transform.position = playerMovement.startingPosition;
                    playerMovement.ResetMovement();

//                 Debug.Log("Player teleported to starting position.");
                }

                // Prevent teleportation spam
                isTeleporting = true;

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

                // Optionally reset the teleportation flag after a short delay
                Invoke(nameof(ResetTeleportFlag), 1f); // Adjust delay time if needed
            }
        }
    }

    // Reset the teleport flag after a delay
    private void ResetTeleportFlag()
    {
        isTeleporting = false;
    }
}
