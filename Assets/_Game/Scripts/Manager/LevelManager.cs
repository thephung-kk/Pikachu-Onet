using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevel;
    public int spriteCount;

    [SerializeField]
    private BackgroundManager backgroundManager;

    void Start()
    {
        // Load saved level (default = 1)
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        // Setup sprite count based on level
        spriteCount = GetSpriteCountForLevel(currentLevel);
        // Set background based on level
        if (backgroundManager != null)
        {
            backgroundManager.SetBackground(currentLevel.ToString());
            Debug.Log("Loaded");
        }
    }

    public void CompleteLevel()
    {
        if (currentLevel < 7)
        {
            currentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.Save();

            // Reload scene or load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Player wins
            Debug.Log("YOU WIN!");
            Time.timeScale = 0; // Stop game ?
        }
    }

    private int GetSpriteCountForLevel(int level)
    {
        switch (level)
        {
            case 1:
                return 24;
            case 2:
                return 30;
            default:
                return 36; // Level 3+ will have 36 sprites
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("CurrentLevel");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
