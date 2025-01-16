using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollisionMessageHandler : MonoBehaviour
{
    public GameObject leapMessagePanel; // UI panel with image and text for "Leap"
    public GameObject fakeMessagePanel; // UI panel with image and text for "Fake"

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
        if (collision.gameObject.CompareTag("Leap"))
        {
            ShowMessagePanel(leapMessagePanel);
        }
        else if (collision.gameObject.CompareTag("Fake"))
        {
            ShowMessagePanel(fakeMessagePanel);
        }
    }

    private void ShowMessagePanel(GameObject messagePanel)
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true); // Show the message panel

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
