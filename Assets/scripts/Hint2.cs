using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RandomTextActivator : MonoBehaviour
{
    public List<Text> relatedTextObjects; // Assign related text objects in the inspector.
    public List<Text> irrelatedTextObjects; // Assign irrelated text objects in the inspector.
    public List<Text> textObjects = new List<Text>(); // Final merged list.
    public Button activateButton; // Assign your button in the inspector.
    public float activationRange = 5f; // Set the range for activation.

    public GameObject emptyObjectForFirstText; // Assign the empty GameObject for positioning the first text.
    private Transform objectTransform; // Reference to the object's transform.
    
    // List to store activated texts.
    private List<Text> activatedTexts = new List<Text>();

    // Variable to store the count of related activated texts.
    public int passdigit2 = 0;
    public int finaldigit;
    public static List<int> binaryList = new List<int>();

    void Start()
    {
        // Store the object's transform.
        objectTransform = transform;

        // Merge related and irrelated text objects into the textObjects list and shuffle it.
        MergeAndShuffleTextObjects();

        // Deactivate all text objects at the start.
        DeactivateAllTextObjects();

        // Initially hide the button.
        if (activateButton != null)
        {
            activateButton.gameObject.SetActive(false);
        }
    }

    void MergeAndShuffleTextObjects()
    {
        // Clear the textObjects list before merging.
        textObjects.Clear();

        // Add related and irrelated text objects to the final list.
        textObjects.AddRange(relatedTextObjects);
        textObjects.AddRange(irrelatedTextObjects);

        // Shuffle the merged list.
        ShuffleList(textObjects);

        Debug.Log("Merged and Shuffled TextObjects Count: " + textObjects.Count);
    }

    void ShuffleList(List<Text> list)
    {
        // Fisher-Yates Shuffle algorithm (also known as Knuth shuffle)
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Text temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    void DeactivateAllTextObjects()
    {
        // Set all text objects to inactive.
        foreach (var text in textObjects)
        {
            text.gameObject.SetActive(false);
        }
        Debug.Log("All text objects deactivated.");
    }

    void Update()
    {
        if (IsNearObject())
        {
            // If the player is near, show the button.
            if (activateButton != null && !activateButton.gameObject.activeSelf)
            {
                activateButton.gameObject.SetActive(true);
            }
        }
        else
        {
            // If the player is not near, hide the button.
            if (activateButton != null && activateButton.gameObject.activeSelf)
            {
                activateButton.gameObject.SetActive(false);
            }
        }
    }

    bool IsNearObject()
    {
        // Find the player by its "Player" tag.
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Check if the player is within range of this object.
            return Vector2.Distance(player.transform.position, objectTransform.position) <= activationRange;
        }
        else
        {
            Debug.LogError("Player not found. Make sure the Player tag is assigned.");
            return false;
        }
    }

    public void OnActivateButtonClicked()
    {
        if (IsNearObject())
        {
            ActivateRandomTexts();
            UpdatePassdigit2(); // Update passdigit2 after activation.
        }
        else
        {
            Debug.Log("Player is not near the object.");
        }
    }

    void ActivateRandomTexts()
    {
        // Determine how many texts to activate (at least 5, or fewer if not enough texts).
        int textsToActivate = Mathf.Min(5, textObjects.Count);
        List<Text> selectedTexts = new List<Text>();

        // Ensure we get unique random texts.
        while (selectedTexts.Count < textsToActivate)
        {
            Text randomText = textObjects[Random.Range(0, textObjects.Count)];

            // Avoid duplicating the same text.
            if (!selectedTexts.Contains(randomText))
            {
                selectedTexts.Add(randomText);
            }
        }

        // Deactivate all text objects first.
        DeactivateAllTextObjects();

        // Activate the selected texts.
        foreach (var text in selectedTexts)
        {
            text.gameObject.SetActive(true);

            // Store the activated text if not already in the list.
            if (!activatedTexts.Contains(text))
            {
                activatedTexts.Add(text);
            }
        }

        // Arrange the activated texts in decreasing y position.
        ArrangeActivatedTexts();

        Debug.Log("Activated " + selectedTexts.Count + " Texts.");
    }

    void ArrangeActivatedTexts()
    {
        if (activatedTexts.Count > 0)
        {
            // First activated text gets position from empty GameObject.
            if (emptyObjectForFirstText != null)
            {
                activatedTexts[0].transform.position = emptyObjectForFirstText.transform.position;
            }

            // Arrange subsequent texts with decreasing y position.
            for (int i = 1; i < activatedTexts.Count; i++)
            {
                Vector3 newPosition = activatedTexts[i-1].transform.position;
                newPosition.y -= 50; // Decrease y position by 50 for each subsequent text.
                activatedTexts[i].transform.position = newPosition;
            }
        }
    }

    void UpdatePassdigit2()
    {
        // Update the count of related activated texts in passdigit2.
        passdigit2 = CountRelatedActivatedTexts();
        Debug.Log("Updated passdigit2: " + passdigit2);
        finaldigit = passdigit2 * 10 + 3 ;
        Debug.Log("final digit "+ finaldigit);
        ConvertToBinaryList(finaldigit);
    }

    public void ConvertToBinaryList(int number)
    {
        binaryList.Clear(); // Clear any existing data

        while (number > 0)
        {
            int bit = number % 2;
            binaryList.Add(bit);
            number /= 2;
        }

        binaryList.Reverse(); // Reverse to correct order
        Debug.Log(binaryList);
        Debug.Log("Binary Representation: " + string.Join(", ", binaryList));
    }

    int CountRelatedActivatedTexts()
    {
        // Count how many activated texts are in the relatedTextObjects list.
        int count = 0;
        foreach (var text in activatedTexts)
        {
            if (relatedTextObjects.Contains(text))
            {
                count++;
            }
        }
        return count;
    }

    public List<Text> GetActivatedTexts()
    {
        // Return the list of activated texts.
        return activatedTexts;
    }
}
