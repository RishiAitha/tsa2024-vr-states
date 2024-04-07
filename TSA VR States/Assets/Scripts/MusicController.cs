using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource music;

    public void UpdateMusic()
    {
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.GetInt("MuteMusic") == 0)
        {
            music.mute = false;
        }
        else
        {
            music.mute = true;
        }
    }
}
