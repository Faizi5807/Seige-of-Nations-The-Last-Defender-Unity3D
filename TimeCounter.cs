using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private float elapsedtime;
    public TextMeshProUGUI timetext;
    

    // Update is called once per frame
    void Update()
    {
        updatetime();
    }
    void updatetime(){
        elapsedtime+=Time.deltaTime;
        int min=Mathf.FloorToInt(elapsedtime/60f);
        int secs=Mathf.FloorToInt(elapsedtime%60f);
       timetext.text = string.Format("{0:00}:{1:00}", min, secs);

    }
}
