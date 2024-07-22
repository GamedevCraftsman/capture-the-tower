using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] AudioSource[] musics;
    [SerializeField] AudioSource[] sounds;

    public void ChangeMusicVolume(Slider slider)
    {
        foreach(AudioSource audio in musics)
        {
            audio.volume = slider.value;
        }
    }

    public void ChangeSoundVolume(Slider slider)
    {
        foreach(AudioSource audio in sounds)
        {
            audio.volume = slider.value;
        }
    }
}
