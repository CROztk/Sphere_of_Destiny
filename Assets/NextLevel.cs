using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    // animator
    private Animator animator;

    private void Awake()
    {
        // get the animator
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("PlayerTriggered");
            bool isAllEnemiesDied = IsAllEnemiesDied();
            animator.SetBool("IsAllEnemiesDied", isAllEnemiesDied);
            if(!isAllEnemiesDied)
            {
                CharacterEvents.characterMessage.Invoke(gameObject ,"Defeat all enemies first!");
            }
        }
    }

    public void LoadNextLevel()
    {
        bool hasNextLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1 < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        if (hasNextLevel && IsAllEnemiesDied())
        {
            // Load the next level
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
        

    }

    private bool IsAllEnemiesDied()
    {
        // Find all the enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // Check if all the enemies are dead
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    
}
