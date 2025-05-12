using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillExperienceBar : MonoBehaviour
{
    public Slider experiencebar;
    public TextMeshProUGUI leveluptext;
    private int level;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        experiencebar.maxValue=100f;

    }

    // Update is called once per frame
    void Update()
    {
        addexpereince();
    }

    void addexpereince(){
        experiencebar.value+=2f;
        if(experiencebar.value==experiencebar.maxValue){
            level+=1;
            leveluptext.text=level.ToString();
            experiencebar.value=experiencebar.minValue;
        }
    }
}
