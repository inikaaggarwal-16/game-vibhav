using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class InteractiveObject : MonoBehaviour
{
    public GameObject buttonUI; // Assign the UI Button GameObject in the inspector.
    public TMP_Text textUI;     // Assign the TMP_Text GameObject in the inspector.
    public Transform player;    // Assign the player's transform in the inspector.
    public float interactionRange = 5f; // Distance at which the button becomes active.

    private bool isTextVisible = false; // Tracks whether the text is currently visible.

    void Start()
    {
        // Ensure the button and text UI are hidden at the start.
        if (buttonUI != null)
        {
            buttonUI.SetActive(false);
        }
        if (textUI != null)
        {
            textUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Check the player's distance from the object.
        if (Vector3.Distance(player.position, transform.position) <= interactionRange)
        {
            // If close enough, show the button UI.
            if (buttonUI != null && !buttonUI.activeSelf)
            {
                buttonUI.SetActive(true);
            }
        }
        else
        {
            // If out of range, hide the button UI and text UI.
            if (buttonUI != null && buttonUI.activeSelf)
            {
                buttonUI.SetActive(false);
            }
            if (textUI != null && textUI.gameObject.activeSelf)
            {
                textUI.gameObject.SetActive(false);
                isTextVisible = false;
            }
        }
    }

    public void OnButtonPressed()
    {
        // Toggle the visibility of the text UI.
        if (textUI != null)
        {
            isTextVisible = !isTextVisible;
            textUI.gameObject.SetActive(isTextVisible);
      }
      
    }
}
