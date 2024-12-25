// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.InputSystem;
// public class playerMovement : MonoBehaviour
// {
//     private Vector2 movement;
//     private Rigidbody2D rb;
//     private int speed = 4;

//     private void Awaik(){
//         rb = GetComponent<Rigidbody2D>();
//     }
//     private void OnMovement(InputValue value){
//         movement = value.Get<Vector2>();

//     }

//     private void FixedUpdate(){
        
        
//         //rb.MovePosition(rb.position + movement * Time.fixedDeltaTime * speed ); 
//         if(movement.x !=0 || movement.y !=0){
//         rb.linearVelocity = movement * speed;
//         }
//     }


// }
/////////////////////////////////////////////////////////////////////////////////
// using UnityEngine;
// using System.Collections;
// using UnityEngine.InputSystem;

// public class playerMovement : MonoBehaviour
// {
//     private Vector2 movement;
//     private Rigidbody2D rb;
//     public int speed = 2;
   
//     private Vector2 targetPosition; // Position the player moves to
//     private bool isMoving = false;  // To track if a move is ongoing

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         targetPosition = rb.position; // Start at the player's initial position
//     }

//     private void OnMovement(InputValue value)
//     {
//         if (isMoving) return; // Ignore input if already moving

//         movement = value.Get<Vector2>();

//         // Only accept one direction at a time (horizontal or vertical)
//         if (movement.x != 0 && movement.y != 0)
//             movement.y = 0;

//         if (movement != Vector2.zero)
//         {
//             targetPosition = (rb.position + movement); // Calculate the next cell position
//             isMoving = true;
//         }
//     }

//     private void FixedUpdate()
//     {
//         if (isMoving)
//         {
//             // Move towards the target position
//             rb.position = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

//             // Check if the player reached the target position
//             if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
//             {
//                 rb.position = targetPosition; // Snap to the exact position
//                 isMoving = false;            // Mark the movement as complete
                
//                 movement = Vector2.zero;     // Reset movement
//             }
//         }
//     }
//}
//////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private int speed = 4;
    private Vector2 targetPosition; // Position the player moves to
    private bool isMoving = false;  // To track if a move is ongoing
    private float cellSize = 0.5f;  // Half a unit per move

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position; // Start at the player's initial position
        animator = GetComponent<Animator>();
    }

    private void OnMovement(InputValue value)
    {
        if (isMoving) return; // Ignore input if already moving

        movement = value.Get<Vector2>();

        // Only accept one direction at a time (horizontal or vertical)
        if (movement.x != 0 && movement.y != 0)
            movement.y = 0;

        if (movement != Vector2.zero)
        {
            targetPosition = rb.position + movement.normalized * cellSize; // Move by half a cell
            isMoving = true;
        }
        animator.SetFloat("X", movement.x);
        animator.SetFloat("Y", movement.y);
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // Move towards the target position
            rb.position = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

            // Check if the player reached the target position
            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition; // Snap to the exact position
                isMoving = false;            // Mark the movement as complete
                movement = Vector2.zero;     // Reset movement
            }
        }
    }
}

// using UnityEngine;
// using UnityEngine.InputSystem;

// public class playerMovement : MonoBehaviour
// {
//     private Vector2 movement;
//     private Rigidbody2D rb;
//     private Animator animator;
//     private int speed = 4;
//     private Vector2 targetPosition; // Position the player moves to
//     private bool isMoving = false;  // To track if a move is ongoing
//     private float cellSize = 0.33f;  // Half a unit per move

//     public float oscillationSpeed = 2f; // Speed of height change
//     public float heightVariation = 0.1f; // Maximum variation in height
//     private Vector3 originalScale; // Original scale of the player

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         targetPosition = rb.position; // Start at the player's initial position
//         animator = GetComponent<Animator>();
//         originalScale = transform.localScale; // Save the original scale
//     }

//     private void Update()
//     {
//         // Oscillate the height of the player
//         float newHeight = originalScale.y + Mathf.Sin(Time.time * oscillationSpeed) * heightVariation;
//         transform.localScale = new Vector3(originalScale.x, newHeight, originalScale.z);
//     }

//     private void OnMovement(InputValue value)
//     {
//         if (isMoving) return; // Ignore input if already moving

//         movement = value.Get<Vector2>();

//         // Only accept one direction at a time (horizontal or vertical)
//         if (movement.x != 0 && movement.y != 0)
//             movement.y = 0;

//         if (movement != Vector2.zero)
//         {
//             targetPosition = rb.position + movement.normalized * cellSize; // Move by half a cell
//             isMoving = true;
//         }
//         animator.SetFloat("X", movement.x);
//         animator.SetFloat("Y", movement.y);
//     }

//     private void FixedUpdate()
//     {
//         if (isMoving)
//         {
//             // Move towards the target position
//             rb.position = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

//             // Check if the player reached the target position
//             if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
//             {
//                 rb.position = targetPosition; // Snap to the exact position
//                 isMoving = false;            // Mark the movement as complete
//                 movement = Vector2.zero;     // Reset movement
//             }
//         }
//     }
// }
