using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Reference to the TextMeshProUGUI component
    public string fullText; // Full text to display
    public float delay = 0.1f; // Delay between each character

    private void Start()
    {
        if (textComponent != null)
        {
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        textComponent.text = ""; // Clear the initial text

        foreach (char letter in fullText)
        {
            textComponent.text += letter; // Add the next letter
            yield return new WaitForSeconds(delay); // Wait before adding the next one
        }
    }
}

