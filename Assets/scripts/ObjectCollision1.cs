
/*
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using JetBrains.Annotations;  // Add this line to resolve the List<T> issue

public class ObjectCollision : MonoBehaviour
{
    public int count = 3; // Starting count value
    public TextMeshProUGUI countText; // Reference to the count display UI
    public TextMeshProUGUI gameOverText; // Reference to the Game Over display UI
    public GameObject player; // Reference to the player GameObject
    public List<int> fixedList = new List<int>();
    public List<int> countList = new List<int>();
    public GameObject teleportobject2;

    void Start()
    {
        // Disable the Game Over text at the start
        if (gameOverText != null)
        {
            gameOverText.enabled = false;
        }

        // Update the count display at the start
        UpdateCountDisplay();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Detect trigger collisions and log the name of the object
        Debug.Log("Triggered by: " + collider.gameObject.name);

        // Handle collisions based on object tags
        if (collider.CompareTag("0"))
        {
            countList.Add(0); // Add to the list if tagged as "0"
            Debug.Log("Player touched 0. Updated list: " + ListToString());
        }
        else if (collider.CompareTag("1"))
        {
            countList.Add(1); // Add to the list if tagged as "1"
            Debug.Log("Player touched 1. Updated list: " + ListToString());
        }
        else if (collider.CompareTag("2"))
        {
            countList.Add(2); // Add to the list if tagged as "2"
            Debug.Log("Player touched 2. Updated list: " + ListToString());
        }
        fixedList = RandomTextActivator.binaryList;
        CompareLists();
    }

    void CompareLists()
    {
        // Ensure comparison is only within the bounds of the shorter list
        int minLength = Mathf.Min(fixedList.Count, countList.Count);

        // Compare index-wise
        for (int i = 0; i < minLength; i++)
        {
            if (fixedList[i] != countList[i])
            {
                Debug.Log($"Mismatch at index {i}: Fixed = {fixedList[i]}, Dynamic = {countList[i]}");
                ResetGame(); // Reset the game instead of teleporting the player
                return;
            }
        }

        // If all elements up to minLength match
        if (fixedList.Count > countList.Count)
        {
            Debug.Log("Dynamic List matches up to its length but is shorter than Fixed List.");
        }
        else if (fixedList.Count < countList.Count)
        {
            Debug.Log("Dynamic List matches Fixed List but has extra elements.");
        }
        else
        {
            Debug.Log("Dynamic List matches Fixed List completely.");
        }
    }

    private void ResetGame()
    {
        // Game reset logic
        Debug.Log("Game reset due to mismatch. Restarting level...");
        // Example: Restart the current level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Heart object collision - count increases
        if (collision.gameObject.CompareTag("Heart"))
        {
            count += 1;
            Debug.Log("Heart collected! Count increased. Current count: " + count);

            // Destroy the heart object after collection
            Destroy(collision.gameObject);

            // Update the UI with the new count
            UpdateCountDisplay();
        }
        // Fake object collision - count decreases
        else if (collision.gameObject.CompareTag("Fake"))
        {
            count -= 2;
            Debug.Log("Fake object hit! Count decreased. Current count: " + count);
            Debug.Log("Fake object position: " + collision.gameObject.transform.position);
            Debug.Log("Player position: " + player.transform.position);

            // Update the UI with the new count
            UpdateCountDisplay();

            // Check if the game is over
            CheckGameOver();
        }
        // Leap object collision - count decreases
        else if (collision.gameObject.CompareTag("Leap"))
        {
            count -= 1;
            Debug.Log("Leap object hit! Count decreased. Current count: " + count);

            // Update the UI with the new count
            UpdateCountDisplay();

            // Check if the game is over
            CheckGameOver();
        }
    }

    // Update the UI display of the count
    public void UpdateCountDisplay()
    {
        if (countText != null)
        {
            countText.text = "" + count;
        }
    }

    // Check if the game is over
    public void CheckGameOver()
    {
        if (count <= 0)
        {
            GameOver();
        }
    }

    // Game Over logic
    private void GameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over";
            gameOverText.enabled = true;
        }

        Time.timeScale = 0; // Stop the game
        Debug.Log("Game Over");
    }

    // Convert the count list to a string for debugging purposes
    public string ListToString()
    {
        string result = "";
        foreach (int value in countList)
        {
            result += value + " ";
        }
        return result.Trim();
    }
}

*/




using UnityEngine;
using TMPro;
using UnityEngine.UI;  // For Button functionality
using UnityEngine.SceneManagement;  // For scene management
using System.Collections.Generic;  // Required for List<T>

public class ObjectCollision : MonoBehaviour
{
    public int count = 3; // Starting count value
    public TextMeshProUGUI countText; // Reference to the count display UI
    public TextMeshProUGUI gameOverText; // Reference to the Game Over display UI
    public GameObject player; // Reference to the player GameObject
    public List<int> fixedList = new List<int>();
    public List<int> countList = new List<int>();
    public GameObject restartButtonPrefab;  // Reference to the restart button prefab

    private GameObject restartButtonInstance;

    void Start()
    {
        // Disable the Game Over text and restart button at the start
        if (gameOverText != null)
        {
            gameOverText.enabled = false;
        }

        if (restartButtonPrefab != null)
        {
            restartButtonPrefab.SetActive(false);
        }

        // Update the count display at the start
        UpdateCountDisplay();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Handle collisions based on object tags
        if (collider.CompareTag("0"))
        {
            countList.Add(0);
        }
        else if (collider.CompareTag("1"))
        {
            countList.Add(1);
        }
        else if (collider.CompareTag("2"))
        {
            countList.Add(2);
        }

        fixedList = RandomTextActivator.binaryList;
        CompareLists();
    }

    void CompareLists()
    {
        int minLength = Mathf.Min(fixedList.Count, countList.Count);

        for (int i = 0; i < minLength; i++)
        {
            if (fixedList[i] != countList[i])
            {
                Debug.Log($"Mismatch at index {i}: Fixed = {fixedList[i]}, Dynamic = {countList[i]}");
                ResetGame();
                return;
            }
        }
    }

    private void ResetGame()
    {
        Debug.Log("Game reset due to mismatch. Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Heart"))
        {
            count += 1;
            Destroy(collision.gameObject);
            UpdateCountDisplay();
        }
        else if (collision.gameObject.CompareTag("Fake"))
        {
            count -= 2;
            UpdateCountDisplay();
            CheckGameOver();
        }
        else if (collision.gameObject.CompareTag("Leap"))
        {
            count -= 1;
            UpdateCountDisplay();
            CheckGameOver();
        }
    }

    public void UpdateCountDisplay()
    {
        if (countText != null)
        {
            countText.text = count.ToString();
        }
    }

    public void CheckGameOver()
    {
        if (count <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over";
            gameOverText.enabled = true;
        }

        Time.timeScale = 0; // Stop the game

        // Instantiate and activate the restart button
        if (restartButtonPrefab != null && restartButtonInstance == null)
        {
            restartButtonInstance = Instantiate(restartButtonPrefab, FindObjectOfType<Canvas>().transform);
            Button restartButton = restartButtonInstance.GetComponent<Button>();
            restartButton.onClick.AddListener(RestartGame);
            restartButtonInstance.SetActive(true);
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public string ListToString()
    {
        return string.Join(" ", countList);
    }
}
