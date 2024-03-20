using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject settingsMenu;
    public Toggle requireHoldingPaddlesInput;
    public Toggle hideRowingPointsInput;
    public Toggle muteMusicInput;
    public Slider musicVolumeInput;

    public DetectRowing rowScript;
    public LevelManager level;

    public void Start()
    {
        startMenu.SetActive(true);
        settingsMenu.SetActive(false);

        rowScript = FindObjectOfType<DetectRowing>();
        level = FindObjectOfType<LevelManager>();

        InitializeSettings();
    }

    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void UnPause()
    {
        level.UnPause();
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
        if (!PlayerPrefs.HasKey("MandatoryPaddles"))
        {
            PlayerPrefs.SetInt("MandatoryPaddles", 0);
        }

        if (!PlayerPrefs.HasKey("HidePoints"))
        {
            PlayerPrefs.SetInt("HidePoints", 0);
        }

        if (!PlayerPrefs.HasKey("MuteMusic"))
        {
            PlayerPrefs.SetInt("MuteMusic", 0);
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        }

        int mandatoryPaddles = PlayerPrefs.GetInt("MandatoryPaddles");
        if (mandatoryPaddles == 0)
        {
            requireHoldingPaddlesInput.isOn = false;
        }
        else
        {
            requireHoldingPaddlesInput.isOn = true;
        }

        int hidePoints = PlayerPrefs.GetInt("HidePoints");
        if (hidePoints == 0)
        {
            hideRowingPointsInput.isOn = false;
        }
        else
        {
            hideRowingPointsInput.isOn = true;
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
    }

    public void ChangeRequirePaddles()
    {
        if (requireHoldingPaddlesInput.isOn)
        {
            PlayerPrefs.SetInt("MandatoryPaddles", 1);
            rowScript.mandatoryPaddles = true;
        }
        else
        {
            PlayerPrefs.SetInt("MandatoryPaddles", 0);
            rowScript.mandatoryPaddles = false;
        }
    }

    public void ChangeHidePoints()
    {
        if (hideRowingPointsInput.isOn)
        {
            PlayerPrefs.SetInt("HidePoints", 1);
            rowScript.rowPointsVisible = false;
        }
        else
        {
            PlayerPrefs.SetInt("HidePoints", 0);
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
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeInput.value);
        rowScript.musicVolume = musicVolumeInput.value;
    }

    public void QuitButton()
    {
        Debug.Log("this should have closed the game");
    }
}
