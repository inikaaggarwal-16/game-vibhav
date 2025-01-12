using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private int speed = 4;
    private Vector2 targetPosition;
    public static Vector2 startingPosition;
    private bool isMoving = false;
    private float cellSize = 0.5f;

    public LayerMask solidLayerMask;  // Layer mask for solid objects (walls and sprite colliders)

    public void Awake()
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
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction.normalized, cellSize +0.1f, solidLayerMask);
        return hit.collider == null;  // If no collision, valid move
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger randomization when the player collides with a teleportation object
        if (collision.CompareTag("Fake"))
        {
            
            FindObjectOfType<RandomObjectManager>()?.RandomizeObjects("Leap");
            FindObjectOfType<RandomObjectManager>()?.RandomizeObjects("Heart");
            FindObjectOfType<RandomObjectManager>()?.RandomizeObjects("Solid");
        }
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

     
}


