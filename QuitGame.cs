using UnityEngine;

public class QuitGame : MonoBehaviour
{
     public void EndGame()
    {
        Debug.Log("Game is ending...");

#if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
#else
        
        Application.Quit();
#endif
    }
}
