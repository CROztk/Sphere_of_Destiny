using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        bool hasNextLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1 < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        if (other.CompareTag("Player") && hasNextLevel)
        {
            // Load the next level
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
