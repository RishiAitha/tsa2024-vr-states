using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectRowing : MonoBehaviour
{
    // Track if motions have been started
    public bool leftStarted;
    public bool rightStarted;

    // Track if motions have been finished
    public bool leftFinished;
    public bool rightFinished;

    // Track if backwards motions have been started
    public bool leftBackStarted;
    public bool rightBackStarted;

    // Track if backwards motions have been finished
    public bool leftBackFinished;
    public bool rightBackFinished;

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

    // See if player is pressing triggers
    public InputActionProperty leftTrigger;
    public InputActionProperty rightTrigger;

    // Track if player is moving forwards or backwards
    public bool movingForwards;

    // Track if player is pressing a trigger
    public bool pressingTrigger;

    // Set amount of time player has between switching directions
    // public float directionSwapDelayForward;
    // public float directionSwapDelayBackward;
    // private float directionSwapDelayCounter;

    // Set water velocity
    public float waterVelocity;

    // Track when the player is grabbing the left paddle
    public bool grabbingLeftPaddle;

    // Track when the player is grabbing the right paddle
    public bool grabbingRightPaddle;

    // Set if the player is using their hands to row
    public bool usingHands;

    // Set if row points are visible
    public bool rowPointsVisible;

    // Set if music should be muted
    public bool musicMuted;

    // Set desired music volume
    public float musicVolume;

    // Get level manager script
    private LevelManager level;
    private EndlessManager endless;

    // Set if the player is facing knockback
    public bool knockback;

    // Amount of time player should be knocked back
    public float knockbackTime;

    // Multiplier of velocity for knockback
    public float knockbackForce;

    // List of all row points
    public GameObject[] rowPoints;

    // Object of the arrow direction indicator
    public GameObject directionArrow;

    // Game music manager
    private MusicController music;

    // List of all rowing sounds
    public AudioSource[] rowSounds;

    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        endless = FindObjectOfType<EndlessManager>();
        music = FindObjectOfType<MusicController>();
        myRB = GetComponent<Rigidbody>();
        // Initialize state variables
        ResetLeft();
        ResetRight();
        currentVelocity = 0.0f;
        knockback = false;
        InitializeSettings();
    }

    // Update is called once per frame
    void Update()
    {
        // directionSwapDelayCounter -= Time.deltaTime;
        pressingTrigger = leftTrigger.action.ReadValue<float>() > 0f || rightTrigger.action.ReadValue<float>() > 0f;
        
        if (!pressingTrigger)
        {
            if (!movingForwards)
            {
                movingForwards = true;
                ResetLeft();
                ResetRight();
            }
        }
        else
        {
            if (movingForwards)
            {
                movingForwards = false;
                ResetBackLeft();
                ResetBackRight();
            }
        }

        foreach (GameObject point in rowPoints)
        {
            if (rowPointsVisible)
            {
                point.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                point.GetComponent<Renderer>().enabled = false;
            }
        }

        if (rowPointsVisible)
        {
            if (movingForwards)
            {
                directionArrow.transform.localScale = Vector3.one;
            }
            else
            {
                directionArrow.transform.localScale = new Vector3(1f, -1f, 1f);
            }
        }

        if (GameRunning() && !knockback)
        {
            if (currentVelocity > 0)
            {
                currentVelocity -= deceleration * Time.deltaTime;
            }

            if ((leftFinished && rightFinished) || (leftBackFinished && rightBackFinished))
            {
                // Cancel previous match motion timers
                CancelInvoke("ResetLeft");
                CancelInvoke("ResetRight");
                CancelInvoke("ResetBackLeft");
                CancelInvoke("ResetBackRight");

                if (!pressingTrigger && (leftFinished && rightFinished) && currentVelocity < maxVelocity)
                {
                    CancelInvoke("TurnRight");
                    CancelInvoke("TurnLeft");
                    currentVelocity += rowAcceleration;
                    rowSounds[0].Play();
                    ResetLeft();
                    ResetRight();
                }
                else if (pressingTrigger && (leftBackFinished && rightBackFinished) && currentVelocity > -(maxVelocity / 2))
                {
                    CancelInvoke("TurnRight");
                    CancelInvoke("TurnLeft");
                    currentVelocity -= rowAcceleration;
                    rowSounds[1].Play();
                    ResetBackLeft();
                    ResetBackRight();
                }
            }

            StartMotion();
        }
        else if (!knockback)
        {
            myRB.velocity = Vector3.zero;
            MatchVelocities();
        }
    }

    public void InitializeSettings()
    {
        if (PlayerPrefs.GetInt("UsingHands") == 1)
        {
            usingHands = true;
        }
        else
        {
            usingHands = false;
        }

        if (PlayerPrefs.GetInt("ShowPoints") == 1)
        {
            rowPointsVisible = true;
        }
        else
        {
            rowPointsVisible = false;
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

        music.UpdateMusic();
    }

    public void StartMotion()
    {
        myRB.velocity = (transform.forward * currentVelocity) - (Vector3.forward * waterVelocity);
        MatchVelocities();
    }

    // Call when left start trigger is entered
    public void LeftStart()
    {
        if (GameRunning() && !knockback)
        {
            if ((!usingHands && grabbingLeftPaddle) || (usingHands && !grabbingLeftPaddle))
            {
                if (leftBackStarted)
                {
                    leftBackFinished = true;

                    CancelInvoke("ResetBackLeft");

                    Invoke("ResetBackLeft", rowMatchDelay);
                }
                leftStarted = true;

                // Wait some time for the user to finish their motion
                Invoke("ResetLeft", rowFinishDelay);
            }
        }
    }

    // Call when left end trigger is entered
    public void LeftEnd()
    {
        if (GameRunning() && !knockback)
        {
            if ((!usingHands && grabbingLeftPaddle) || (usingHands && !grabbingLeftPaddle))
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
                leftBackStarted = true;

                Invoke("ResetBackLeft", rowFinishDelay);
            }
        }
    }

    private void ResetLeft()
    {
        if (GameRunning())
        {
            leftStarted = false;
            leftFinished = false;
        }
    }

    private void ResetBackLeft()
    {
        if (GameRunning())
        {
            leftBackStarted = false;
            leftBackFinished = false;
        }
    }

    public void RightStart()
    {
        if (GameRunning() && !knockback)
        {
            if ((!usingHands && grabbingRightPaddle) || (usingHands && !grabbingRightPaddle))
            {
                if (rightBackStarted)
                {
                    rightBackFinished = true;

                    CancelInvoke("ResetBackRight");

                    Invoke("ResetBackRight", rowMatchDelay);
                }
                rightStarted = true;

                Invoke("ResetRight", rowFinishDelay);
            }
        }
    }

    public void RightEnd()
    {
        if (GameRunning() && !knockback)
        {
            if ((!usingHands && grabbingRightPaddle) || (usingHands && !grabbingRightPaddle))
            {
                if (rightStarted)
                {
                    rightFinished = true;

                    CancelInvoke("ResetRight");

                    Invoke("TurnLeft", turnDelay);

                    Invoke("ResetRight", rowMatchDelay);
                }
                rightBackStarted = true;

                Invoke("ResetBackRight", rowFinishDelay);
            }
        }
    }

    private void ResetRight()
    {
        if (GameRunning())
        {
            rightStarted = false;
            rightFinished = false;
        }
    }

    private void ResetBackRight()
    {
        if (GameRunning())
        {
            rightBackStarted = false;
            rightBackFinished = false;
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
        if (GameRunning() && !knockback)
        {
            ResetLeft();
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAmount, 0);
            StartCoroutine(RotationLerp(newRotation, rotationTime));
            rowSounds[2].Play();
        }
    }

    private void TurnLeft()
    {
        if (GameRunning() && !knockback)
        {
            ResetRight();
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y - rotationAmount, 0);
            StartCoroutine(RotationLerp(newRotation, rotationTime));
            rowSounds[3].Play();
        }
    }

    IEnumerator RotationLerp(Quaternion newRotation, float duration)
    {
        if (GameRunning() && !knockback)
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
        if (GameRunning())
        {
            grabbingLeftPaddle = true;
        }
    }

    public void UnGrabLeftPaddle()
    {
        if (GameRunning())
        {
            grabbingLeftPaddle = false;
        }
    }

    public void GrabRightPaddle()
    {
        if (GameRunning())
        {
            grabbingRightPaddle = true;
        }
    }

    public void UnGrabRightPaddle()
    {
        if (GameRunning())
        {
            grabbingRightPaddle = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Walls" && !knockback)
        {
            StartCoroutine("Knockback");
        }
    }

    public IEnumerator Knockback()
    {
        float currentForce = knockbackForce;
        Vector3 origDirection = Vector3.Normalize(myRB.velocity);

        knockback = true;

        for (int i = 0; i < 10; i++)
        {
            myRB.velocity = origDirection * -currentForce;
            MatchVelocities();
            currentForce -= knockbackForce / 10f;
            yield return new WaitForSeconds(knockbackTime);
        }

        myRB.velocity = Vector3.zero;
        currentVelocity = 0;
        MatchVelocities();
        
        knockback = false;
    }

    public bool GameRunning()
    {
        if (level != null)
        {
            return level.gameRunning;
        }
        else
        {
            return endless.gameRunning;
        }
    }
}
