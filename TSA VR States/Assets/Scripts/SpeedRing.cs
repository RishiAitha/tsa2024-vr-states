using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRing : MonoBehaviour
{
    private DetectRowing rowScript;

    void Update()
    {
        rowScript = FindObjectOfType<DetectRowing>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rowScript.currentVelocity *= 1.5f;
        }
    }
}
