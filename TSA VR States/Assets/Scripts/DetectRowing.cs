using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRowing : MonoBehaviour
{
    // Track if motions have been started
    public bool leftStarted;
    public bool rightStarted;

    // Track if motions have been finished
    public bool leftFinished;
    public bool rightFinished;

    // Get Rigidbody for motion
    public Rigidbody myRB;

    // Get Rigidbodies of start and end positions
    public Rigidbody leftStartBody;
    public Rigidbody leftEndBody;
    public Rigidbody rightStartBody;
    public Rigidbody rightEndBody;

    // Get Rigidbodies of boat and paddles
    public Rigidbody boatBody;
    public Rigidbody leftPaddleBody;
    public Rigidbody rightPaddleBody;

    // Set the amount of acceleration when rowing
    public float rowAcceleration;

    // Set the amount of time the player has to finish their motion
    public float rowFinishDelay;

    // Set the amount of time the player has to row with the other hand after using one
    public float rowMatchDelay;

    // Set the amount of time it takes for the player to decelerate
    public float deceleration;

    // Set player's terminal velocity
    public float maxVelocity;

    // Store current forward velocity
    public float currentVelocity;

    // Set amount of degrees to rotate
    public float rotationAmount;

    // Set amount of time that rotation should occur on
    public float rotationTime;

    // Set amount of time it takes to start a turn
    public float turnDelay;

    // Set water velocity
    public float waterVelocity;

    // Track when the player is grabbing the left paddle
    public bool grabbingLeftPaddle;

    // Track when the player is grabbing the right paddle
    public bool grabbingRightPaddle;

    // Set if grabbing the paddles is mandatory
    public bool mandatoryPaddles;

    // Set if row points are visible
    public bool rowPointsVisible;

    // Set if music should be muted
    public bool musicMuted;

    // Set desired music volume
    public float musicVolume;

    // Get level manager script
    private LevelManager level;

    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        myRB = GetComponent<Rigidbody>();
        // Initialize state variables
        ResetLeft();
        ResetRight();
        currentVelocity = 0.0f;
        InitializeSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (level.gameRunning)
        {
            if (currentVelocity > 0)
            {
                currentVelocity -= deceleration * Time.deltaTime;
            }

            if (leftFinished && rightFinished)
            {
                // Cancel previous match motion timers
                CancelInvoke("ResetLeft");
                CancelInvoke("ResetRight");

                // Cancel all turning
                CancelInvoke("TurnRight");
                CancelInvoke("TurnLeft");

                ResetLeft();
                ResetRight();

                if (currentVelocity < maxVelocity)
                {
                    currentVelocity += rowAcceleration;
                }

                StartMotion();
            }
        }
        else
        {
            myRB.velocity = Vector3.zero;
            MatchVelocities();
        }
    }

    public void InitializeSettings()
    {
        if (PlayerPrefs.GetInt("MandatoryPaddles") == 1)
        {
            mandatoryPaddles = true;
        }
        else
        {
            mandatoryPaddles = false;
        }

        if (PlayerPrefs.GetInt("HidePoints") == 1)
        {
            rowPointsVisible = false;
        }
        else
        {
            rowPointsVisible = true;
        }

        if (PlayerPrefs.GetInt("MuteMusic") == 1)
        {
            musicMuted = true;
        }
        else
        {
            musicMuted = false;
        }

        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void StartMotion()
    {
        myRB.velocity = (transform.forward * currentVelocity) - (Vector3.forward * waterVelocity);
        MatchVelocities();
    }

    // Call when left start trigger is entered
    public void LeftStart()
    {
        if (level.gameRunning)
        {
            if ((mandatoryPaddles && grabbingLeftPaddle) || !mandatoryPaddles)
            {
                leftStarted = true;

                // Wait some time for the user to finish their motion
                Invoke("ResetLeft", rowFinishDelay);
            }
        }
    }

    // Call when left end trigger is entered
    public void LeftEnd()
    {
        if (level.gameRunning)
        {
            if ((mandatoryPaddles && grabbingLeftPaddle) || !mandatoryPaddles)
            {
                // If the motion has already started
                if (leftStarted)
                {
                    leftFinished = true;

                    // Cancel the previous finish motion timer
                    CancelInvoke("ResetLeft");

                    Invoke("TurnRight", turnDelay);

                    // Wait some time for the user
                    // If they don't row with the other hand in time
                    // The motion will reset before they can move forward
                    Invoke("ResetLeft", rowMatchDelay);
                }
            }
        }
    }

    private void ResetLeft()
    {
        if (level.gameRunning)
        {
            leftStarted = false;
            leftFinished = false;
        }
    }

    public void RightStart()
    {
        if (level.gameRunning)
        {
            if ((mandatoryPaddles && grabbingRightPaddle) || !mandatoryPaddles)
            {
                rightStarted = true;

                Invoke("ResetRight", rowMatchDelay);
            }
        }
    }

    public void RightEnd()
    {
        if (level.gameRunning)
        {
            if ((mandatoryPaddles && grabbingRightPaddle) || !mandatoryPaddles)
            {
                if (rightStarted)
                {
                    rightFinished = true;

                    CancelInvoke("ResetLeft");

                    Invoke("TurnLeft", turnDelay);

                    Invoke("ResetRight", rowMatchDelay);
                }
            }
        }
    }

    private void ResetRight()
    {
        if (level.gameRunning)
        {
            rightStarted = false;
            rightFinished = false;
        }
    }

    private void MatchVelocities()
    {
        leftStartBody.velocity = myRB.velocity;
        leftEndBody.velocity = myRB.velocity;
        rightStartBody.velocity = myRB.velocity;
        rightEndBody.velocity = myRB.velocity;
        boatBody.velocity = myRB.velocity;
        leftPaddleBody.velocity = myRB.velocity;
        rightPaddleBody.velocity = myRB.velocity;
    }

    private void TurnRight()
    {
        if (level.gameRunning)
        {
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAmount, 0);
            StartCoroutine(RotationLerp(newRotation, rotationTime));
        }
    }

    private void TurnLeft()
    {
        if (level.gameRunning)
        {
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y - rotationAmount, 0);
            StartCoroutine(RotationLerp(newRotation, rotationTime));
        }
    }

    IEnumerator RotationLerp(Quaternion newRotation, float duration)
    {
        if (level.gameRunning)
        {
            float time = 0.0f;
            Quaternion oldRotation = transform.rotation;
            while (time < duration && !(leftFinished && rightFinished))
            {
                transform.rotation = Quaternion.Lerp(oldRotation, newRotation, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.rotation = newRotation;
            StartMotion();
        }
    }
    public void GrabLeftPaddle()
    {
        if (level.gameRunning)
        {
            grabbingLeftPaddle = true;
        }
    }

    public void UnGrabLeftPaddle()
    {
        if (level.gameRunning)
        {
            grabbingLeftPaddle = false;
        }
    }

    public void GrabRightPaddle()
    {
        if (level.gameRunning)
        {
            grabbingRightPaddle = true;
        }
    }

    public void UnGrabRightPaddle()
    {
        if (level.gameRunning)
        {
            grabbingRightPaddle = false;
        }
    }
}
