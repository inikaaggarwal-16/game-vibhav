/*using UnityEngine;
using UnityEngine.UI;

public class PlayerPickup : MonoBehaviour
{
    public Transform carryPosition; // The position where the picked-up object will be held
    public Button actionButton; // Reference to the action button
    private GameObject carriedObject = null;

    void Start()
    {
        // Set up the button's onClick listener
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnActionButtonClick);
            actionButton.gameObject.SetActive(false); // Hide button by default
        }
    }

    void Update()
    {
        if (carriedObject == null)
        {
            CheckForNearbyObjects();
        }
    }

    private void CheckForNearbyObjects()
    {
        // Detect nearby objects with tags "Fire" or "Lamp"
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Fire") || collider.CompareTag("Lamp"))
            {
                actionButton.gameObject.SetActive(true); // Show button
                return;
            }
        }

        actionButton.gameObject.SetActive(false); // Hide button if no objects nearby
    }

    private void OnActionButtonClick()
    {
        if (carriedObject == null)
        {
            TryPickupObject();
        }
        else
        {
            DropObject();
        }
    }

    private void TryPickupObject()
    {
        // Detect nearby objects with tags "Fire" or "Lamp"
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Fire") || collider.CompareTag("Lamp"))
            {
                PickupObject(collider.gameObject);
                actionButton.GetComponentInChildren<Text>().text = "Drop"; // Change button text
                break;
            }
        }
    }

    private void PickupObject(GameObject obj)
    {
        carriedObject = obj;

        // Disable physics (optional)
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true;

        // Set the object's position and parent
        obj.transform.position = carryPosition.position;
        obj.transform.SetParent(carryPosition);

        // Additional customization: Change the layer to avoid collisions
        obj.layer = LayerMask.NameToLayer("CarriedObject");
    }

    private void DropObject()
    {
        if (carriedObject != null)
        {
            // Enable physics (optional)
            Rigidbody2D rb = carriedObject.GetComponent<Rigidbody2D>();
            if (rb != null) rb.isKinematic = false;

            // Remove the parent and drop at the current position
            carriedObject.transform.SetParent(null);

            // Reset the object's layer
            carriedObject.layer = LayerMask.NameToLayer("Default");

            carriedObject = null;
            actionButton.GetComponentInChildren<Text>().text = "Pickup"; // Reset button text
        }
    }
}*/

using System.Collections.Generic;

public class PlayerPickup : MonoBehaviour
{
    public Transform carryPosition;
    public Button actionButton;
    private GameObject carriedObject = null;

    private HashSet<string> pickedUpTags = new HashSet<string>();
    private ProceduralPropGenerator propGenerator;

    void Start()
    {
        // Initialize button and reference the ProceduralPropGenerator
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnActionButtonClick);
            actionButton.gameObject.SetActive(false);
        }

        propGenerator = FindObjectOfType<ProceduralPropGenerator>();
    }

    private void OnActionButtonClick()
    {
        if (carriedObject == null)
        {
            TryPickupObject();
        }
        else
        {
            DropObject();
        }

        // Check if the correct combination is picked up
        ValidateCombination();
    }

    private void PickupObject(GameObject obj)
    {
        carriedObject = obj;

        // Track the object's tag
        pickedUpTags.Add(obj.tag);

        // ... existing pickup logic
    }

    private void DropObject()
    {
        if (carriedObject != null)
        {
            // Remove the object's tag from the set
            pickedUpTags.Remove(carriedObject.tag);

            // ... existing drop logic
        }
    }

    private void ValidateCombination()
    {
        if (propGenerator != null && propGenerator.correctCombination.SetEquals(pickedUpTags))
        {
            ChangeDoorColors(Color.white); // Correct combination: set doors to visible
        }
        else
        {
            ChangeDoorColors(Color.black); // Incorrect combination: set doors to black
        }
    }

    private void ChangeDoorColors(Color color)
    {
        DoorColorManager[] doors = FindObjectsOfType<DoorColorManager>();
        foreach (var door in doors)
        {
            door.SetDoorColor(color);
        }
    }
}

