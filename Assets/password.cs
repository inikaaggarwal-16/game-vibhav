using TMPro; // Add this for TextMeshPro
using UnityEngine;

public class DoorKeypad : MonoBehaviour
{
    public GameObject keypadUI; // Assign the Keypad UI GameObject in the inspector.
    public string correctPassword = "potential"; // Set the correct word password.
    public float activationRange = 5f; // Distance at which the keypad becomes active.
    public Transform player; // Assign the player's transform in the inspector.
    public Transform door; // Assign the door's transform in the inspector.
    public Vector3 teleportPosition = new Vector3(-17, 0, 0); // Teleport position for incorrect password.

    private string inputPassword = ""; // Stores the player's input password.
    public TMP_InputField inputField; // Assign the TMP_InputField in the inspector.

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
            // Optionally, implement winning logic here.
        }
        else
        {
            Debug.Log("Incorrect Password. Teleporting...");
            // Teleport the player to the specified position.
            player.position = teleportPosition;
        }

        // Reset the keypad input and hide the keypad UI.
        ResetKeypad();
        if (keypadUI != null)
        {
            keypadUI.SetActive(false);
        }
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
