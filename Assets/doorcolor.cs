// using UnityEngine;
// public class DoorManager : MonoBehaviour
// {
//     public GameObject player; // Reference to the player

//     public void ManipulateDoorColor()
//     {
//         PlayerPickup playerPickup = player.GetComponent<PlayerPickup>();
//         if (playerPickup != null && playerPickup.GetPickedObjectOrigin() != Vector2.zero)
//         {
//             Vector2 origin = playerPickup.GetPickedObjectOrigin();
//             Debug.Log($"Object was picked from {origin}. Use this to manipulate door colors.");

//             // Example: Find a door and change its color
//             GameObject door = GameObject.FindWithTag("Door"); // Assuming doors have the "Door" tag
//             if (door != null)
//             {
//                 SpriteRenderer spriteRenderer = door.GetComponent<SpriteRenderer>();
//                 if (spriteRenderer != null)
//                 {
//                     spriteRenderer.color = Color.red; // Example manipulation
//                 }
//             }
//         }
//     }
// }
