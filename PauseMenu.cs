using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;

    public void PauseGame()
    {
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; 
        isPaused = false;
    }
}
