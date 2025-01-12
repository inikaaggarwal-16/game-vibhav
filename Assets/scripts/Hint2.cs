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

    private List<Text> activatedTexts = new List<Text>(); // List to store activated texts.
    public int passdigit2 = 0;
    public int finaldigit;
    public static List<int> binaryList = new List<int>();

    private bool isTextVisible = false; // Track if the texts are currently visible.
    private bool isActivationLocked = false; // Track if activation is locked.

    void Start()
    {
        objectTransform = transform;
        MergeAndShuffleTextObjects();
        DeactivateAllTextObjects();

        if (activateButton != null)
        {
            activateButton.gameObject.SetActive(false);
        }
    }

    void MergeAndShuffleTextObjects()
    {
        textObjects.Clear();
        textObjects.AddRange(relatedTextObjects);
        textObjects.AddRange(irrelatedTextObjects);
        ShuffleList(textObjects);
        Debug.Log("Merged and Shuffled TextObjects Count: " + textObjects.Count);
    }

    void ShuffleList(List<Text> list)
    {
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
            if (activateButton != null && !activateButton.gameObject.activeSelf)
            {
                activateButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (activateButton != null && activateButton.gameObject.activeSelf)
            {
                activateButton.gameObject.SetActive(false);
            }
        }
    }

    bool IsNearObject()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
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
        if (isActivationLocked)
        {
            Debug.Log("Texts have already been activated and deactivated. Further activation is locked.");
            return; // Do nothing if activation is locked.
        }

        if (IsNearObject())
        {
            if (isTextVisible)
            {
                // If texts are visible, hide them and lock further activations.
                DeactivateAllTextObjects();
                isTextVisible = false;
                isActivationLocked = true; // Lock activation after texts are hidden.
                Debug.Log("Texts are now hidden, and activation is locked.");
            }
            else
            {
                // If texts are not visible, show them.
                ActivateRandomTexts();
                isTextVisible = true;
            }

            UpdatePassdigit2();
        }
        else
        {
            Debug.Log("Player is not near the object.");
        }
    }

    void ActivateRandomTexts()
    {
        int textsToActivate = Mathf.Min(5, textObjects.Count);
        List<Text> selectedTexts = new List<Text>();

        while (selectedTexts.Count < textsToActivate)
        {
            Text randomText = textObjects[Random.Range(0, textObjects.Count)];
            if (!selectedTexts.Contains(randomText))
            {
                selectedTexts.Add(randomText);
            }
        }

        DeactivateAllTextObjects();

        foreach (var text in selectedTexts)
        {
            text.gameObject.SetActive(true);
            if (!activatedTexts.Contains(text))
            {
                activatedTexts.Add(text);
            }
        }

        ArrangeActivatedTexts();
        Debug.Log("Activated " + selectedTexts.Count + " Texts.");
    }

    void ArrangeActivatedTexts()
    {
        if (activatedTexts.Count > 0)
        {
            if (emptyObjectForFirstText != null)
            {
                activatedTexts[0].transform.position = emptyObjectForFirstText.transform.position;
            }

            for (int i = 1; i < activatedTexts.Count; i++)
            {
                Vector3 newPosition = activatedTexts[i - 1].transform.position;
                newPosition.y -= 50;
                activatedTexts[i].transform.position = newPosition;
            }
        }
    }

    void UpdatePassdigit2()
    {
        passdigit2 = CountRelatedActivatedTexts();
        Debug.Log("Updated passdigit2: " + passdigit2);
        finaldigit = passdigit2 * 10 + 3;
        Debug.Log("final digit " + finaldigit);
        ConvertToBinaryList(finaldigit);
    }

    public void ConvertToBinaryList(int number)
    {
        binaryList.Clear();
        while (number > 0)
        {
            int bit = number % 2;
            binaryList.Add(bit);
            number /= 2;
        }
        binaryList.Reverse();
        Debug.Log(binaryList);
        Debug.Log("Binary Representation: " + string.Join(", ", binaryList));
    }

    int CountRelatedActivatedTexts()
    {
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
}