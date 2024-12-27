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
//     private float cellSize = 0.5f;  // Half a unit per move

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         targetPosition = rb.position; // Start at the player's initial position
//         animator = GetComponent<Animator>();
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
//     private float cellSize = 0.5f;  // Half a unit per move

//     public void MoveUp() => Move(Vector2.up);
//     public void MoveDown() => Move(Vector2.down);
//     public void MoveLeft() => Move(Vector2.left);
//     public void MoveRight() => Move(Vector2.right);

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         targetPosition = rb.position; // Start at the player's initial position
//         animator = GetComponent<Animator>();
//     }

//       public void Move(Vector2 direction)
//     {
//         if (isMoving) return;

//         TriggerMovement(direction);
//     }

//     private void TriggerMovement(Vector2 direction)
//     {
//         if (direction != Vector2.zero)
//         {
//             targetPosition = rb.position + direction.normalized * cellSize;
//             isMoving = true;
//         }

//         animator.SetFloat("X", direction.x);
//         animator.SetFloat("Y", direction.y);
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

//                 // Notify TimeLeapOnCollision to record position
//                 FindObjectOfType<TimeLeapOnCollision>()?.RecordPosition(rb.position);
//             }
//         }
//     }

//     // Reset the movement state
//     public void ResetMovement()
//     {
//         isMoving = false;
//         movement = Vector2.zero;
//         targetPosition = rb.position;
//         animator.SetFloat("X", 0);
//         animator.SetFloat("Y", 0);
//     }
// }



// using UnityEngine;
// using UnityEngine.InputSystem;

// public class playerMovement : MonoBehaviour
// {
//     private Vector2 movement;
//     private Rigidbody2D rb;
//     private Animator animator;
//     private int speed = 4;
//     private Vector2 targetPosition;
//     public Vector2 startingPosition;
//     private bool isMoving = false;
//     private float cellSize = 0.5f;

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         targetPosition = rb.position;
//         startingPosition = rb.position;
//         animator = GetComponent<Animator>();
//     }

//     public void MoveUp() => Move(Vector2.up);
//     public void MoveDown() => Move(Vector2.down);
//     public void MoveLeft() => Move(Vector2.left);
//     public void MoveRight() => Move(Vector2.right);

//     public void Move(Vector2 direction)
//     {
//         if (isMoving) return;

//         TriggerMovement(direction);
//     }

//     private void TriggerMovement(Vector2 direction)
//     {
//         if (direction != Vector2.zero)
//         {
//             targetPosition = rb.position + direction.normalized * cellSize;
//             isMoving = true;
//         }

//         animator.SetFloat("X", direction.x);
//         animator.SetFloat("Y", direction.y);
//     }

//     private void OnMovement(InputValue value)
//     {
//         if (isMoving) return;

//         movement = value.Get<Vector2>();

//         if (movement.x != 0 && movement.y != 0)
//             movement.y = 0;

//         if (movement != Vector2.zero)
//         {
//             targetPosition = rb.position + movement.normalized * cellSize;
//             isMoving = true;
//         }

//         animator.SetFloat("X", movement.x);
//         animator.SetFloat("Y", movement.y);
//     }

//     private void FixedUpdate()
//     {
//         if (isMoving)
//         {
//             rb.position = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

//             if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
//             {
//                 rb.position = targetPosition;
//                 isMoving = false;
//                 movement = Vector2.zero;

//                 // Notify TimeLeapOnCollision to record position
//                 FindObjectOfType<TimeLeapOnCollision>()?.RecordPosition(rb.position);
//             }
//         }
//     }

//     public void ResetMovement()
//     {
//         isMoving = false;
//         movement = Vector2.zero;
//         targetPosition = rb.position;
//         animator.SetFloat("X", 0);
//         animator.SetFloat("Y", 0);
//     }

// }

// //trying to add boundary and collision


using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private int speed = 4;
    private Vector2 targetPosition;
    public Vector2 startingPosition;
    private bool isMoving = false;
    private float cellSize = 0.5f;

    public LayerMask solidLayerMask;  // Layer mask for solid objects (walls and sprite colliders)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
        startingPosition = rb.position;
        animator = GetComponent<Animator>();
    }

    public void MoveUp() => Move(Vector2.up);
    public void MoveDown() => Move(Vector2.down);
    public void MoveLeft() => Move(Vector2.left);
    public void MoveRight() => Move(Vector2.right);

    public void Move(Vector2 direction)
    {
        if (isMoving || !IsValidMove(direction)) return;

        TriggerMovement(direction);
    }

    private void TriggerMovement(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            targetPosition = rb.position + direction.normalized * cellSize;
            isMoving = true;
        }

        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

    private void OnMovement(InputValue value)
    {
        if (isMoving) return;

        movement = value.Get<Vector2>();

        if (movement.x != 0 && movement.y != 0)
            movement.y = 0;

        if (movement != Vector2.zero && IsValidMove(movement))
        {
            targetPosition = rb.position + movement.normalized * cellSize;
            isMoving = true;
        }

        animator.SetFloat("X", movement.x);
        animator.SetFloat("Y", movement.y);
    }

    private bool IsValidMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction.normalized, cellSize, solidLayerMask);
        return hit.collider == null;  // If no collision, valid move
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.position = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition;
                isMoving = false;
                movement = Vector2.zero;

                // Notify TimeLeapOnCollision to record position
                FindObjectOfType<TimeLeapOnCollision>()?.RecordPosition(rb.position);
            }
        }
    }

    public void ResetMovement()
    {
        isMoving = false;
        movement = Vector2.zero;
        targetPosition = rb.position;
        animator.SetFloat("X", 0);
        animator.SetFloat("Y", 0);
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger randomization when the player collides with a teleportation object
        if (collision.CompareTag("Fake"))
        {
          //FindObjectOfType<RandomObjectManager>()?.DistributeObjects("Leap");
          //  FindObjectOfType<RandomObjectManager>()?.RandomizeObjects("Heart");
        //    FindObjectOfType<RandomObjectManager>()?.RandomizeObjects("Solid");
        }
    }
}
