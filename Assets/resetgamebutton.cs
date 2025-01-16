using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    public TextMeshProUGUI targetText;       // The text element to monitor
    public string triggerText = "Game Over"; // The text that triggers the button
    public GameObject buttonPrefab;          // The button prefab to instantiate
    private GameObject buttonInstance;       // The instantiated button

    void Update()
    {
        // Check if the target text matches the trigger text and the button is not already instantiated
        if (targetText != null && targetText.text == triggerText && buttonInstance == null)
        {
            CreateRestartButton();
        }
    }

    void CreateRestartButton()
    {
        // Instantiate the button and set its parent to the Canvas
        buttonInstance = Instantiate(buttonPrefab, FindObjectOfType<Canvas>().transform);
        Button restartButton = buttonInstance.GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);
    }

    void RestartGame()
    {
        // Reset the game time and reload the current scene
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
