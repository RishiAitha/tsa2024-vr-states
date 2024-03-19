using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject levelMenu;
    public GameObject settingsMenu;
    public GameObject infoCanvas;
    public GameObject playerCamera;
    public Toggle requireHoldingPaddlesInput;
    public Toggle hideRowingPointsInput;
    public Toggle muteMusicInput;
    public Slider musicVolumeInput;

    public void Start()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        InitializeSettings();
    }

    public void Update()
    {
        transform.position = new Vector3(transform.position.x, playerCamera.transform.position.y - 0.176f, transform.position.z);
        infoCanvas.transform.position = new Vector3(infoCanvas.transform.position.x, playerCamera.transform.position.y - 0.176f, infoCanvas.transform.position.z);
    }

    public void OpenLevelSelect()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackToMain()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Level Tutorial");
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level 3");
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
        }
        else
        {
            PlayerPrefs.SetInt("MandatoryPaddles", 0);
        }
    }

    public void ChangeHidePoints()
    {
        if (hideRowingPointsInput.isOn)
        {
            PlayerPrefs.SetInt("HidePoints", 1);
        }
        else
        {
            PlayerPrefs.SetInt("HidePoints", 0);
        }
    }

    public void ChangeMuteMusic()
    {
        if (muteMusicInput.isOn)
        {
            PlayerPrefs.SetInt("MuteMusic", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MuteMusic", 0);
        }
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeInput.value);
    }

    public void QuitButton()
    {
        Debug.Log("this should have closed the game");
    }
}
