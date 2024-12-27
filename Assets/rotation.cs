// using UnityEngine;

// public class ContinuousRotation : MonoBehaviour
// {
    
//     public float rotationSpeed = 50f;

//     void Update()
//     {
        
//         transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
//     }
// }


// using UnityEngine;

// public class ContinuousRotation : MonoBehaviour
// {
//     public float rotationSpeed = 50f;

//     void Update()
//     {
//         // Rotate the object continuously
//         transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Player"))
//         {
//             // Get the player's movement script
//             var playerMovement = collision.GetComponent<playerMovement>();
//             if (playerMovement != null)
//             {
//                 // Teleport the player to their starting position
//                 collision.transform.position = playerMovement.startingPosition;

//                 // Reset the player's movement to avoid unintended displacement
//                 playerMovement.ResetMovement();

//                 Debug.Log("Player teleported to their starting position and movement reset.");
//             }
//         }
//     }
// }

//fake tag count
using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private void Update()
    {
        // Rotate the object continuously
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
                }

                // Teleport the player
                collision.transform.position = playerMovement.startingPosition;
                playerMovement.ResetMovement();

                Debug.Log("Player teleported to starting position.");
            }
        }
    }
}
