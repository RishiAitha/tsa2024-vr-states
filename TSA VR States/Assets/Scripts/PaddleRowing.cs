using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleRowing : MonoBehaviour
{
    public bool isLeft;

    public DetectRowing rowScript;

    public bool isGrabbed;

    public LevelManager level;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        rowScript = FindObjectOfType<DetectRowing>();
    }

    void Update()
    {
        if (level.gameRunning)
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
        if (level.gameRunning)
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
