using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleRotation : MonoBehaviour
{
    public bool isLeft;
    private DetectRowing rowScript;
    private Quaternion origRotation;

    void Start()
    {
        rowScript = FindObjectOfType<DetectRowing>();
        origRotation = transform.localRotation;
    }

    void Update()
    {
        if (isLeft && !rowScript.grabbingLeftPaddle)
        {
            transform.localRotation = origRotation;
        }
        else if (!isLeft && !rowScript.grabbingRightPaddle)
        {
            transform.localRotation = origRotation;
        }
    }
}
