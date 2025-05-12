using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayButtonScript : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
