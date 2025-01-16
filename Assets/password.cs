using TMPro; // Add this for TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement; // Add this for scene management

public class DoorKeypad : MonoBehaviour
{
    public GameObject keypadUI; // Assign the Keypad UI GameObject in the inspector.
    public string correctPassword = "potential"; // Set the correct word password.
    public float activationRange = 5f; // Distance at which the keypad becomes active.
    public Transform player; // Assign the player's transform in the inspector.
    public Transform door; // Assign the door's transform in the inspector.
    public TMP_InputField inputField; // Assign the TMP_InputField in the inspector.

    private string inputPassword = ""; // Stores the player's input password.

    void Start()
    {
        // Ensure the keypad UI is hidden at the start.
        if (keypadUI != null)
        {
            keypadUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check the player's distance from the door.
        if (Vector3.Distance(player.position, door.position) <= activationRange)
        {
            // If close enough, show the keypad UI.
            if (keypadUI != null && !keypadUI.activeSelf)
            {
                keypadUI.SetActive(true);
                inputField.text = ""; // Clear the InputField when activated.
            }
        }
        else
        {
            // If out of range, hide the keypad UI.
            if (keypadUI != null && keypadUI.activeSelf)
            {
                keypadUI.SetActive(false);
                ResetKeypad(); // Reset keypad input when the UI is hidden.
            }
        }
    }

    public void OnSubmitButtonPressed()
    {
        // Get the entered text from the TMP_InputField.
        inputPassword = inputField.text;

        // Check if the input password matches the correct password.
        if (inputPassword.ToLower() == correctPassword.ToLower()) // Case-insensitive check.
        {
            Debug.Log("Correct Password! You Win!");
            // Load the next scene.
            SceneManager.LoadScene("next"); // Replace "next" with your desired scene name.
        }
        else
        {
            Debug.Log("Incorrect Password. Game Over!");
            // Trigger game over logic here.
            GameOver();
        }

        // Reset the keypad input and hide the keypad UI.
        ResetKeypad();
        if (keypadUI != null)
        {
            keypadUI.SetActive(false);
        }
    }

    private void GameOver()
    {
        // Implement your game over logic here.
        Debug.Log("Game Over. Restarting level...");
        // Example: Restart the current level.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetKeypad()
    {
        // Clear the input password and reset the TMP_InputField.
        inputPassword = "";
        if (inputField != null)
        {
            inputField.text = "";
        }
    }
}