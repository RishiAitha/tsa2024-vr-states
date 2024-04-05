using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class EndlessManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI countdownText;

    public TextMeshProUGUI victoryText;

    public int score;

    public float countdownTime;
    private float countdownTimeCounter;

    public GameObject scoreDisplay;

    public GameObject countdownDisplay;

    public GameObject pauseMenu;

    public GameObject victoryMenu;

    public GameObject leftGrabRay;
    public GameObject rightGrabRay;

    public bool gameRunning;

    public bool gameStarted;

    private bool pauseOpen;

    public InputActionProperty menuInteraction;

    private DetectRowing rowScript;

    public GameObject[] blocks;

    public GameObject rockWall;

    public int initialMoves;

    private void Start()
    {
        initialMoves = 2;
        rowScript = FindObjectOfType<DetectRowing>();
        countdownTimeCounter = countdownTime;
        score = 0;
        leftGrabRay.SetActive(false);
        rightGrabRay.SetActive(false);
        gameRunning = false;
        gameStarted = false;
        pauseOpen = false;
        countdownDisplay.SetActive(true);
        scoreDisplay.SetActive(false);
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

            scoreText.text = score.ToString();
        }
        else
        {
            if (countdownTimeCounter > 0f)
            {
                countdownTimeCounter -= Time.deltaTime;
                countdownText.text = ((int)countdownTimeCounter + 1).ToString();
            }
            else
            {
                gameStarted = true;
                gameRunning = true;
                countdownDisplay.SetActive(false);
                scoreDisplay.SetActive(true);
            }
        }
    }

    public void Victory()
    {
        victoryMenu.SetActive(true);
        pauseMenu.SetActive(false);
        leftGrabRay.SetActive(true);
        rightGrabRay.SetActive(true);
        scoreDisplay.SetActive(false);
        FreezeGame();

        victoryText.text = "Congrats! Your score is: " + score.ToString();
    }

    public void Pause()
    {
        if (gameRunning)
        {
            pauseOpen = true;
            pauseMenu.SetActive(true);
            leftGrabRay.SetActive(true);
            rightGrabRay.SetActive(true);
            scoreDisplay.SetActive(false);
            FreezeGame();
        }
    }

    public void UnPause()
    {
        pauseOpen = false;
        pauseMenu.SetActive(false);
        leftGrabRay.SetActive(false);
        rightGrabRay.SetActive(false);
        scoreDisplay.SetActive(true);
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

    public void MoveBlocks()
    {
        if (initialMoves <= 0)
        {
            score++;
            for (int i = 1; i < blocks.Length; i++)
            {
                GameObject swapBlock = blocks[i - 1];
                blocks[i - 1] = blocks[i];
                blocks[i] = swapBlock;
            }
            Transform lastBlockTransform = blocks[blocks.Length - 1].transform;
            lastBlockTransform.position = new Vector3(lastBlockTransform.position.x, lastBlockTransform.position.y, lastBlockTransform.position.z + 300f);
            Transform rockWallTransform = rockWall.GetComponent<Transform>();
            rockWallTransform.SetParent(blocks[0].GetComponent<Transform>(), false);
        }
        else
        {
            initialMoves--;
        }
    }
}