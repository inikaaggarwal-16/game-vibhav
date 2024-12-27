using UnityEngine;
using TMPro;

public class ObjectCollision : MonoBehaviour
{
    private int count = 3; 
    
    public TextMeshProUGUI countText;   
    public TextMeshProUGUI gameOverText; 

    void Start()
    {
        
        if (gameOverText != null)
        {
            gameOverText.enabled = false;  
        }

    
        UpdateCountDisplay();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Heart"))
        {
            count += 1;
            Debug.Log("Count increased. Current count: " + count);

            
            Destroy(collision.gameObject);  
        }

        
        else if (collision.gameObject.CompareTag("Fake"))
        {
            
            //transform.position = new Vector3(-19.09f, 0.67f, 0); 
            //Debug.Log("Player moved to new position: (-19.09, 0.67, 0)");

            
            count -= 2;
            Debug.Log("Count decreased. Current count: " + count);
            UpdateCountDisplay();
        }
        else if(collision.gameObject.CompareTag("Leap")){
            count -= 1;
            Debug.Log("Count decreased. Current count: " + count);
        }
        
        UpdateCountDisplay();

        
        if (count <= 0)
        {
            GameOver();
        }
    }

    void UpdateCountDisplay()
    {
        
        if (countText != null)
        {
            countText.text = " " + count;  
        }
    }

    void GameOver()
    {
        
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over";  
            gameOverText.enabled = true;  
        }

        
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
}