using UnityEngine;

public class PopUpScreenScript : MonoBehaviour
{
    [SerializeField] private GameObject[] allPopups;
    [SerializeField] private GameObject mainMenuObjects;

    public void ShowPopup(GameObject popupToShow)
    {
        
        foreach (GameObject popup in allPopups)
        {
            popup.SetActive(false);
        }

        
        if (popupToShow != null)
        {
            popupToShow.SetActive(true);
            mainMenuObjects.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Popup to show is null.");
        }
    }

    public void HidePopupScreen()
    {
        foreach (GameObject popup in allPopups)
        {
            popup.SetActive(false);
        }

        mainMenuObjects.SetActive(true);
    }
}
