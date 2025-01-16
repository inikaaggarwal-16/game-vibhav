using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayImageOnClick : MonoBehaviour
{
    public Image imageToDisplay; // Reference to the Image component
    public float displayDuration = 3f; // Duration the image will be displayed

    void Start()
    {
        // Ensure the image is initially hidden
        if (imageToDisplay != null)
        {
            imageToDisplay.gameObject.SetActive(false);
        }
    }

    public void OnButtonClick()
    {
        // Show the image and start the coroutine to hide it after a delay
        if (imageToDisplay != null)
        {
            imageToDisplay.gameObject.SetActive(true);
            StartCoroutine(HideImageAfterDelay());
        }
    }

    private IEnumerator HideImageAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        if (imageToDisplay != null)
        {
            imageToDisplay.gameObject.SetActive(false);
        }
    }
}

