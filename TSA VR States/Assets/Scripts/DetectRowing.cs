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
    public bool allowHands;

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

    private MusicController music;

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

        if (GameRunning() && !knockback)
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
        if (PlayerPrefs.GetInt("AllowHands") == 1)
        {
            allowHands = true;
        }
        else
        {
            allowHands = false;
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
            if ((!allowHands && grabbingLeftPaddle) || allowHands)
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
        if (GameRunning() && !knockback)
        {
            if ((!allowHands && grabbingLeftPaddle) || allowHands)
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
        if (GameRunning())
        {
            leftStarted = false;
            leftFinished = false;
        }
    }

    public void RightStart()
    {
        if (GameRunning() && !knockback)
        {
            if ((!allowHands && grabbingRightPaddle) || allowHands)
            {
                rightStarted = true;

                Invoke("ResetRight", rowMatchDelay);
            }
        }
    }

    public void RightEnd()
    {
        if (GameRunning() && !knockback)
        {
            if ((!allowHands && grabbingRightPaddle) || allowHands)
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
        if (GameRunning())
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
        if (GameRunning() && !knockback)
        {
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAmount, 0);
            StartCoroutine(RotationLerp(newRotation, rotationTime));
        }
    }

    private void TurnLeft()
    {
        if (GameRunning() && !knockback)
        {
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y - rotationAmount, 0);
            StartCoroutine(RotationLerp(newRotation, rotationTime));
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
