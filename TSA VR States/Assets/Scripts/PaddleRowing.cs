using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleRowing : MonoBehaviour
{
    public bool isLeft;

    public DetectRowing rowScript;

    public bool isGrabbed;

    void Start()
    {
        rowScript = FindObjectOfType<DetectRowing>();
    }

    void Update()
    {
        if (rowScript.GameRunning())
        {
            if (isLeft)
            {
                isGrabbed = rowScript.grabbingLeftPaddle;
            }
            else
            {
                isGrabbed = rowScript.grabbingRightPaddle;
            }
        }    
    }

    void OnTriggerEnter(Collider other)
    {
        if (rowScript.GameRunning())
        {
            if (isGrabbed)
            {
                if (isLeft)
                {
                    if (other.gameObject.tag == "LeftStart")
                    {
                        rowScript.LeftStart();
                    }

                    if (other.gameObject.tag == "LeftEnd")
                    {
                        rowScript.LeftEnd();
                    }
                }
                else
                {
                    if (other.gameObject.tag == "RightStart")
                    {
                        rowScript.RightStart();
                    }

                    if (other.gameObject.tag == "RightEnd")
                    {
                        rowScript.RightEnd();
                    }
                }
            }
        }
    }
}
