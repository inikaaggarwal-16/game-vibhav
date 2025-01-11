using UnityEngine;

public class DisappearAfterTime : MonoBehaviour
{
    // Time in seconds before the GameObject disappears
    public float disappearTime = 2f;

    void Start()
    {
        // Call the Disappear method after the specified time
        Invoke("Disappear", disappearTime);
    }

    void Disappear()
    {
        // Disable the GameObject to make it disappear
        gameObject.SetActive(false);
    }
}
