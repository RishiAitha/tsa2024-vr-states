using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI countdownText;

    public TextMeshProUGUI victoryText;

    public float levelTime;
    private float levelTimeCounter;

    public float countdownTime;
    private float countdownTimeCounter;

    public float currentTime;

    public GameObject timerDisplay;

    public GameObject countdownDisplay;

    public GameObject gameOverMenu;

    public GameObject pauseMenu;

    public GameObject victoryMenu;

    public GameObject leftGrabRay;
    public GameObject rightGrabRay;

    public bool gameRunning;

    public bool gameStarted;

    private bool pauseOpen;

    public InputActionProperty menuInteraction;

    private DetectRowing rowScript;

    private void Start()
    {
        rowScript = FindObjectOfType<DetectRowing>();
        countdownTimeCounter = countdownTime;
        levelTimeCounter = levelTime;
        currentTime = 0f;
        leftGrabRay.SetActive(false);
        rightGrabRay.SetActive(false);
        gameRunning = false;
        gameStarted = false;
        pauseOpen = false;
        countdownDisplay.SetActive(true);
        timerDisplay.SetActive(false);
        victoryMenu.SetActive(false);
    }

    private void Update()
    {
        if (gameStarted)
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
                if (levelTimeCounter > 1f)
                {
                    levelTimeCounter -= Time.deltaTime;
                    currentTime += Time.deltaTime;
                    timerText.text = ((int)levelTimeCounter).ToString();
                }
                else
                {
                    GameOver();
                }
            }
        }
        else
        {
            if (countdownTimeCounter > 1f)
            {
                countdownTimeCounter -= Time.deltaTime;
                countdownText.text = ((int) countdownTimeCounter).ToString();
            }
            else
            {
                gameStarted = true;
                gameRunning = true;
                countdownDisplay.SetActive(false);
                timerDisplay.SetActive(true);
            }
        }
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        victoryMenu.SetActive(false);
        pauseMenu.SetActive(false);
        leftGrabRay.SetActive(true);
        rightGrabRay.SetActive(true);
        timerDisplay.SetActive(false);
        FreezeGame();
    }

    public void Victory()
    {
        victoryMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        leftGrabRay.SetActive(true);
        rightGrabRay.SetActive(true);
        timerDisplay.SetActive(false);
        FreezeGame();

        victoryText.text = "Congrats! Your time is: " + ((int)currentTime).ToString() + " seconds";
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
