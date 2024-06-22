using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldGripAnimation : MonoBehaviour
{
    private Animator handAnimator;

    void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        handAnimator.SetFloat("Grip", 1);
    }
}
