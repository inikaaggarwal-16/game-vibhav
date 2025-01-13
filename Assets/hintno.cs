using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationHandler : MonoBehaviour
{
    public Button button1; // Drag Button 1 in the inspector
    public Button button2; // Drag Button 2 in the inspector

    private bool isButton1Clicked = false;
    private bool isButton2Clicked = false;

    void Start()
    {
        if (button1 != null)
        {
            button1.onClick.AddListener(OnButton1Clicked);
        }

        if (button2 != null)
        {
            button2.onClick.AddListener(OnButton2Clicked);
        }

        // Initially deactivate the GameObject this script is attached to
        DeactivateObject();
    }

    void OnButton1Clicked()
    {
        isButton1Clicked = true;
        Debug.Log("Button 1 clicked");
        CheckActivation();
    }

    void OnButton2Clicked()
    {
        isButton2Clicked = true;
        Debug.Log("Button 2 clicked");
        CheckActivation();
    }

    void CheckActivation()
    {
        if (isButton1Clicked && isButton2Clicked)
        {
            Debug.Log("Both buttons clicked at least once, activating the object");
            ActivateObject();
        }
    }

    void DeactivateObject()
    {
        // Deactivate the GameObject this script is attached to
        gameObject.SetActive(false);
        Debug.Log($"Deactivated the object: {gameObject.name}");
    }

    void ActivateObject()
    {
        // Activate the GameObject this script is attached to
        gameObject.SetActive(true);
        Debug.Log($"Activated the object: {gameObject.name}");
    }
}
