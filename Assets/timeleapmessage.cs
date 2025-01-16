using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollisionMessageHandler : MonoBehaviour
{
    public GameObject leapMessagePanel; // UI panel with image and text for "Leap"
    public GameObject fakeMessagePanel; // UI panel with image and text for "Fake"
    private bool hasShownLeapMessage = false; // Flag to ensure "Leap" message shows only once
    private bool hasShownFakeMessage = false; // Flag to ensure "Fake" message shows only once

    void Start()
    {
        if (leapMessagePanel != null)
        {
            leapMessagePanel.SetActive(false); // Ensure the "Leap" panel is hidden initially
        }

        if (fakeMessagePanel != null)
        {
            fakeMessagePanel.SetActive(false); // Ensure the "Fake" panel is hidden initially
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Leap") && !hasShownLeapMessage)
        {
            ShowMessagePanel(leapMessagePanel, ref hasShownLeapMessage);
        }
        else if (collision.gameObject.CompareTag("Fake") && !hasShownFakeMessage)
        {
            ShowMessagePanel(fakeMessagePanel, ref hasShownFakeMessage);
        }
    }

    private void ShowMessagePanel(GameObject messagePanel, ref bool hasShownMessage)
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true); // Show the message panel
            hasShownMessage = true; // Set flag to true to prevent future displays

            // Optionally hide the panel after a few seconds
            StartCoroutine(HideMessageAfterDelay(messagePanel, 3f)); // 3-second delay
        }
    }

    private IEnumerator HideMessageAfterDelay(GameObject messagePanel, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (messagePanel != null)
        {
            messagePanel.SetActive(false); // Hide the message panel
        }
    }
}
