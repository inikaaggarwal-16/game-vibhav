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

public class ObjectCollision : MonoBehaviour
{
    public int count = 3; // Made public for debugging
    public TextMeshProUGUI countText;
    public TextMeshProUGUI gameOverText;

    private void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.enabled = false;
        }

        UpdateCountDisplay();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Heart collision
        if (collision.gameObject.CompareTag("Heart"))
        {
            count += 1;
            Debug.Log("Heart collected! Count increased. Current count: " + count);

            // Destroy the heart object
            Destroy(collision.gameObject);

            UpdateCountDisplay();
        }
        // Fake object collision                           //not working so directly write down on rotation script
        else if (collision.gameObject.CompareTag("Fake"))
        {
            count -= 2;
            Debug.Log("Fake object hit! Count decreased. Current count: " + count);

            UpdateCountDisplay();
            CheckGameOver();
        }
        // Leap object collision
        else if (collision.gameObject.CompareTag("Leap"))
        {
            count -= 1;
            Debug.Log("Leap object hit! Count decreased. Current count: " + count);

            UpdateCountDisplay();
            CheckGameOver();
        }
    }

    public void UpdateCountDisplay()
    {
        if (countText != null)
        {
            countText.text = " " + count;
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
        Debug.Log("Game Over");
    }
}
