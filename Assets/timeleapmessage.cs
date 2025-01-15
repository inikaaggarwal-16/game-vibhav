using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeapCollisionMessage : MonoBehaviour
{
    public GameObject leapMessagePanel; // UI panel with image and text
    private bool hasShownLeapMessage = false; // Flag to ensure message shows only once

    void Start()
    {
        if (leapMessagePanel != null)
        {
            leapMessagePanel.SetActive(false); // Ensure the panel is hidden initially
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Leap") && !hasShownLeapMessage)
        {
            if (leapMessagePanel != null)
            {
                leapMessagePanel.SetActive(true); // Show the message panel
                hasShownLeapMessage = true; // Set flag to true to prevent future displays

                // Optionally hide the panel after a few seconds
                StartCoroutine(HideLeapMessageAfterDelay(3f)); // 3-second delay
            }
        }
    }

    private IEnumerator HideLeapMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (leapMessagePanel != null)
        {
            leapMessagePanel.SetActive(false); // Hide the message panel
        }
    }
}
