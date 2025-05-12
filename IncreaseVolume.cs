using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseVolume : MonoBehaviour
{
    public Slider volumeslider;
    public TextMeshProUGUI volumetext;
    public AudioSource audioSource;

    void Start()
    {
        // Set initial values
        volumeslider.value = audioSource.volume;
        UpdateVolumeUI(audioSource.volume);

        // Add listener for slider value change
        volumeslider.onValueChanged.AddListener(increasevolume);
    }

    void increasevolume(float value)
    {
        audioSource.volume = value;
        UpdateVolumeUI(value);
    }

    void UpdateVolumeUI(float value)
    {
        int percent = Mathf.RoundToInt(value * 100f);
        volumetext.text = "Volume: " + percent.ToString();
    }
}
