using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject settingsMenu;
    public Toggle allowUsingHandsInput;
    public Toggle showRowingPointsInput;
    public Toggle muteMusicInput;
    public Slider musicVolumeInput;

    public DetectRowing rowScript;
    public LevelManager level;
    public EndlessManager endless;

    private MusicController music;

    public void Start()
    {
        startMenu.SetActive(true);
        settingsMenu.SetActive(false);

        rowScript = FindObjectOfType<DetectRowing>();
        level = FindObjectOfType<LevelManager>();
        endless = FindObjectOfType<EndlessManager>();
        music = FindObjectOfType<MusicController>();

        InitializeSettings();
    }

    public IEnumerator ExitSequence(string sceneName)
    {
        FindObjectOfType<FadeScreen>().FadeOut();
        yield return new WaitForSeconds(FindObjectOfType<FadeScreen>().fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        StartCoroutine(ExitSequence(currentScene.name));
    }

    public void MainMenu()
    {
        IntroInfo.PerformIntro = false;
        StartCoroutine(ExitSequence("Main Menu"));
    }

    public void UnPause()
    {
        if (level != null)
        {
            level.UnPause();
        }
        else
        {
            endless.UnPause();
        }
    }

    public void OpenSettingsMenu()
    {
        startMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackToMain()
    {
        startMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    private void InitializeSettings()
    {
        if (!PlayerPrefs.HasKey("AllowHands"))
        {
            PlayerPrefs.SetInt("AllowHands", 0);
        }

        if (!PlayerPrefs.HasKey("ShowPoints"))
        {
            PlayerPrefs.SetInt("ShowPoints", 0);
        }

        if (!PlayerPrefs.HasKey("MuteMusic"))
        {
            PlayerPrefs.SetInt("MuteMusic", 0);
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        }

        int allowHands = PlayerPrefs.GetInt("AllowHands");
        if (allowHands == 0)
        {
            allowUsingHandsInput.isOn = false;
        }
        else
        {
            allowUsingHandsInput.isOn = true;
        }

        int showPoints = PlayerPrefs.GetInt("ShowPoints");
        if (showPoints == 0)
        {
            showRowingPointsInput.isOn = false;
        }
        else
        {
            showRowingPointsInput.isOn = true;
        }

        int muteMusic = PlayerPrefs.GetInt("MuteMusic");
        if (muteMusic == 0)
        {
            muteMusicInput.isOn = false;
        }
        else
        {
            muteMusicInput.isOn = true;
        }

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        musicVolumeInput.value = musicVolume;

        music.UpdateMusic();
    }

    public void ChangeAllowHands()
    {
        if (allowUsingHandsInput.isOn)
        {
            PlayerPrefs.SetInt("AllowHands", 1);
            rowScript.allowHands = true;
        }
        else
        {
            PlayerPrefs.SetInt("AllowHands", 0);
            rowScript.allowHands = false;
        }
    }

    public void ChangeShowPoints()
    {
        if (showRowingPointsInput.isOn)
        {
            PlayerPrefs.SetInt("ShowPoints", 1);
            rowScript.rowPointsVisible = false;
        }
        else
        {
            PlayerPrefs.SetInt("ShowPoints", 0);
            rowScript.rowPointsVisible = true;
        }
    }

    public void ChangeMuteMusic()
    {
        if (muteMusicInput.isOn)
        {
            PlayerPrefs.SetInt("MuteMusic", 1);
            rowScript.musicMuted = true;
        }
        else
        {
            PlayerPrefs.SetInt("MuteMusic", 0);
            rowScript.musicMuted = false;
        }

        music.UpdateMusic();
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeInput.value);
        rowScript.musicVolume = musicVolumeInput.value;

        music.UpdateMusic();
    }
}
