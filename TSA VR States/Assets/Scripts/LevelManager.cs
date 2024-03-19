using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        levelTimeCounter = levelTime;
        leftGrabRay.SetActive(false);
        rightGrabRay.SetActive(false);
    }

    private void Update()
    {
        if (levelTimeCounter > 0f)
        {
            levelTimeCounter -= Time.deltaTime;
            timerText.text = levelTimeCounter.ToString();
        }
        else
        {
            GameOver();
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

    public void FreezeGame()
    {
        // idk the game needs to stop running/moving here
    }
}
