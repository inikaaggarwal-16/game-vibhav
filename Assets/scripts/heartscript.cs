using UnityEngine;
using UnityEngine.UI;

public class ObjectCollision : MonoBehaviour
{
    private int count = 2;

    // Reference to the UI Text element for the "Game Over" message
    public Text gameOverText;

    // Reference to the UI Text element for displaying the count
    public Text countText;

    void Start()
    {
        // Ensure the "Game Over" message is initially hidden
        if (gameOverText != null)
        {
            gameOverText.enabled = false;
        }

        // Update the count display at the start
        UpdateCountDisplay();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Increase count by 1
            count += 1;
            Debug.Log("Count increased. Current count: " + count);
        }
        // Check if the collided object has the "Fake" tag
        else if (collision.gameObject.CompareTag("Fake"))
        {
            // Decrease count by 2
            count -= 2;
            Debug.Log("Count decreased. Current count: " + count);
        }

        // Update the count display every time it changes
        UpdateCountDisplay();

        // Check if count is 0 or less
        if (count <= 0)
        {
            // End the game
            GameOver();
        }
    }

    void UpdateCountDisplay()
    {
        // Update the count text in the UI
        if (countText != null)
        {
            countText.text = "Count: " + count;
        }
    }

    void GameOver()
    {
        // Display "Game Over" message
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over";
            gameOverText.enabled = true;  // Make sure the text is visible
        }

        // Pause the game to effectively end it
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
}
