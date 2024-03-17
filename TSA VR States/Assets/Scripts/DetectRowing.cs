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


    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        // Initialize state variables
        ResetLeft();
        ResetRight();
        currentVelocity = 0.0f;
    }

    // Update is called once per frame
    void Update()
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
        //transform.position += transform.forward * Time.deltaTime * currentVelocity;
        //transform.position -= Vector3.forward * Time.deltaTime * waterVelocity;
        myRB.velocity = (transform.forward * currentVelocity) - (Vector3.forward * waterVelocity);
        MatchVelocities();
    }

    // Call when left start trigger is entered
    public void LeftStart()
    {
        if ((mandatoryPaddles && grabbingLeftPaddle) || !mandatoryPaddles)
        {
            leftStarted = true;

            // Wait some time for the user to finish their motion
            Invoke("ResetLeft", rowFinishDelay);
        }
    }

    // Call when left end trigger is entered
    public void LeftEnd()
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

    private void ResetLeft()
    {
        leftStarted = false;
        leftFinished = false;
    }

    public void RightStart()
    {
        if ((mandatoryPaddles && grabbingRightPaddle) || !mandatoryPaddles)
        {
            rightStarted = true;

            Invoke("ResetRight", rowMatchDelay);
        }
    }

    public void RightEnd()
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

    private void ResetRight()
    {
        rightStarted = false;
        rightFinished = false;
    }

    private void MatchVelocities()
    {
        leftStartBody.velocity = myRB.velocity;
        leftEndBody.velocity = myRB.velocity;
        rightStartBody.velocity = myRB.velocity;
        rightEndBody.velocity = myRB.velocity;
        boatBody.velocity = myRB.velocity;
        //leftPaddleBody.velocity = myRB.velocity;
        //rightPaddleBody.velocity = myRB.velocity;
    }

    private void TurnRight()
    {
        Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAmount, 0);
        StartCoroutine(RotationLerp(newRotation, rotationTime));
    }

    private void TurnLeft()
    {
        Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y - rotationAmount, 0);
        StartCoroutine(RotationLerp(newRotation, rotationTime));
    }

    IEnumerator RotationLerp(Quaternion newRotation, float duration)
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
    }
    public void GrabLeftPaddle()
    {
        grabbingLeftPaddle = true;
    }

    public void UnGrabLeftPaddle()
    {
        grabbingLeftPaddle = false;
    }

    public void GrabRightPaddle()
    {
        grabbingRightPaddle = true;
    }

    public void UnGrabRightPaddle()
    {
        grabbingRightPaddle = false;
    }
}
