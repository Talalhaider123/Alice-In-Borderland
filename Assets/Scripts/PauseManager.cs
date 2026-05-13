using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Ensure game runs normally
    }

    // ⏸ Pause Game
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freeze game
    }

    // ▶ Resume Game
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Resume game
    }

    // 🔄 Restart Level
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Reset time before reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🏠 Go to Menu Scene
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu"); // Make sure your scene name is correct
    }
}