using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioMixer MainVolume;
    Slider Volume_Slider;

    [SerializeField]Sprite[] Volume_Image;
    Image Volume_Icon;

    private void Awake()
    {
        Volume_Slider = GetComponentInChildren<Slider>();
        Volume_Icon = GetComponentInChildren<Button>().image;
    }

    void Update()
    {
        MainVolume.SetFloat("MainVolume", Volume_Slider.value);

        if(Volume_Slider.value <= -10f)
        {
            MainVolume.SetFloat("MainVolume", -80f);
            Volume_Icon.sprite = Volume_Image[1];
            Music_Off = true;
        }
        else
        {
            Volume_Icon.sprite = Volume_Image[0];
            Music_Off = false;
        }
    }

    bool Music_Off;

    public void Volume_Button()
    {
        if (Music_Off = Music_Off != true)
            Volume_Slider.value = -10f;
        else
            Volume_Slider.value = -9f;
    }
}
