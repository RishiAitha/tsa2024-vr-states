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

    public void Start()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);
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

    public void RequireOars()
    {
        int currentOars = PlayerPrefs.GetInt("RequireOars");

        if (currentOars == 0)
        {
            PlayerPrefs.SetInt("RequireOars", 1);
        }

        if (currentOars == 1)
        {
            PlayerPrefs.SetInt("RequireOars", 0);
        }
    }

    public void QuitButton()
    {
        Debug.Log("this should have closed the game");
    } 
}
