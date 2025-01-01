// using UnityEngine;
// using TMPro;

// public class ObjectCollision : MonoBehaviour
// {
//     private int count = 3; 
    
//     public TextMeshProUGUI countText;   
//     public TextMeshProUGUI gameOverText; 

//     void Start()
//     {
        
//         if (gameOverText != null)
//         {
//             gameOverText.enabled = false;  
//         }

    
//         UpdateCountDisplay();
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
        
//         if (collision.gameObject.CompareTag("Heart"))
//         {
//             count += 1;
//             Debug.Log("Count increased. Current count: " + count);

            
//             Destroy(collision.gameObject);  
//         }

        
//         else if (collision.gameObject.CompareTag("Fake"))
//         {
            
//             //transform.position = new Vector3(-19.09f, 0.67f, 0); 
//             //Debug.Log("Player moved to new position: (-19.09, 0.67, 0)");

            
//             count -= 2;
//             Debug.Log("Count decreased. Current count: " + count);
            
//         }
//         else if(collision.gameObject.CompareTag("Leap")){
//             count -= 1;
//             Debug.Log("Count decreased. Current count: " + count);
//         }
        
//         UpdateCountDisplay();

        
//         if (count <= 0)
//         {
//             GameOver();
//         }
//     }

//     void UpdateCountDisplay()
//     {
        
//         if (countText != null)
//         {
//             countText.text = " " + count;  
//         }
//     }

//     void GameOver()
//     {
        
//         if (gameOverText != null)
//         {
//             gameOverText.text = "Game Over";  
//             gameOverText.enabled = true;  
//         }

        
//         Time.timeScale = 0;
//         Debug.Log("Game Over");
//     }
// }

//fake tag count
using UnityEngine;
using TMPro;
using System.Collections.Generic;  // Add this line to resolve the List<T> issue

public class ObjectCollision : MonoBehaviour
{
    public int count = 3; // Starting count value
    public TextMeshProUGUI countText; // Reference to the count display UI
    public TextMeshProUGUI gameOverText; // Reference to the Game Over display UI
    public GameObject player; // Reference to the player GameObject
    
    private void Start()
    {
        // Disable the Game Over text at the start
        if (gameOverText != null)
        {
            gameOverText.enabled = false;
        }

        // Update the count display at the start
        UpdateCountDisplay();
    }

    private void OnTriggerEnter2D(Collider2D collider)
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    // List to store the count values (from trigger interactions)
    private List<int> countList = new List<int>();

    // Convert the count list to a string for debugging purposes
    private string ListToString()
    {
        string result = "";
        foreach (int value in countList)
        {
            result += value + " ";
        }
        return result.Trim();
    }
}