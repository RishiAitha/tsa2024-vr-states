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
    public Rigidbody rigidbody;

    // Get transform for rotation
    public Transform transform;

    // Get Rigidbodies of start and end positions
    public Rigidbody leftStartBody;
    public Rigidbody leftEndBody;
    public Rigidbody rightStartBody;
    public Rigidbody rightEndBody;

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


    // Start is called before the first frame update
    void Start()
    {
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
            MatchPoints();
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
                MatchPoints();
            }
        }
        transform.position += transform.forward * Time.deltaTime * currentVelocity;
        transform.position -= Vector3.forward * Time.deltaTime * waterVelocity;
    }

    // Call when left start trigger is entered
    public void LeftStart()
    {
        leftStarted = true;

        // Wait some time for the user to finish their motion
        Invoke("ResetLeft", rowFinishDelay);
    }

    // Call when left end trigger is entered
    public void LeftEnd()
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

    private void ResetLeft()
    {
        leftStarted = false;
        leftFinished = false;
    }

    public void RightStart()
    {
        rightStarted = true;

        Invoke("ResetRight", rowMatchDelay);
    }

    public void RightEnd()
    {
        if (rightStarted)
        {
            rightFinished = true;

            CancelInvoke("ResetLeft");

            Invoke("TurnLeft", turnDelay);

            Invoke("ResetRight", rowMatchDelay);
        }
    }

    private void ResetRight()
    {
        rightStarted = false;
        rightFinished = false;
    }

    private void MatchPoints()
    {
        leftStartBody.velocity = rigidbody.velocity;
        leftEndBody.velocity = rigidbody.velocity;
        rightStartBody.velocity = rigidbody.velocity;
        rightEndBody.velocity = rigidbody.velocity;
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
}
