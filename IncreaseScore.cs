using TMPro;
using UnityEngine;

public class IncreaseScore : MonoBehaviour
{
    public TextMeshProUGUI scoretext;
    private int score;

    // Update is called once per frame
    void Update()
    {
        updatescore();
    }

    void updatescore(){
        score+=1;
        scoretext.text="Score:"+score.ToString();
    }
}
