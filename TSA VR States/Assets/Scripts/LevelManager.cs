using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float levelTime;
    private float levelTimeCounter;

    public GameObject timerDisplay;

    public GameObject gameOverMenu;

    public GameObject pauseMenu;

    public GameObject leftGrabRay;
    public GameObject rightGrabRay;

    public bool gameRunning;

    private bool pauseOpen;

    public InputActionProperty menuInteraction;

    private DetectRowing rowScript;

    private void Start()
    {
        rowScript = FindObjectOfType<DetectRowing>();
        levelTimeCounter = levelTime;
        leftGrabRay.SetActive(false);
        rightGrabRay.SetActive(false);
        gameRunning = true;
        pauseOpen = false;
    }

    private void Update()
    {
        if (menuInteraction.action.triggered)
        {
            if (pauseOpen)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
        }
        if (gameRunning)
        {
            if (levelTimeCounter > 0f)
            {
                levelTimeCounter -= Time.deltaTime;
                timerText.text = ((int) levelTimeCounter).ToString();
            }
            else
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        leftGrabRay.SetActive(true);
        rightGrabRay.SetActive(true);
        timerDisplay.SetActive(false);
        FreezeGame();
    }

    public void Pause()
    {
        if (gameRunning)
        {
            pauseOpen = true;
            pauseMenu.SetActive(true);
            leftGrabRay.SetActive(true);
            rightGrabRay.SetActive(true);
            timerDisplay.SetActive(false);
            FreezeGame();
        }
    }

    public void UnPause()
    {
        pauseOpen = false;
        pauseMenu.SetActive(false);
        leftGrabRay.SetActive(false);
        rightGrabRay.SetActive(false);
        timerDisplay.SetActive(true);
        UnFreezeGame();
    }

    public void FreezeGame()
    {
        gameRunning = false;
    }

    public void UnFreezeGame()
    {
        gameRunning = true;
        rowScript.StartMotion();
    }
}
