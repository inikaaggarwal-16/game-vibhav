using UnityEngine;
using System.Collections.Generic;

public class TimeLeapOnCollision : MonoBehaviour
{
    public float timeInterval = 1f;
    private float timer = 0f;
    private List<Vector3> positionHistory = new List<Vector3>();
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            timer += Time.deltaTime;
            if (timer >= timeInterval)
            {
                positionHistory.Add(player.transform.position);
                timer = 0f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (positionHistory.Count >= 5)
            {
                Vector3 positionToRewind = positionHistory[positionHistory.Count - 3];
                player.transform.position = positionToRewind;
            }
        }
    }
}
