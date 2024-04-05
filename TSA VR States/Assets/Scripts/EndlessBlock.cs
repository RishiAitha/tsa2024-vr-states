using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBlock : MonoBehaviour
{
    private EndlessManager endless;

    void Start()
    {
        endless = FindObjectOfType<EndlessManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Rigidbody>().velocity.z >= 0f)
        {
            endless.MoveBlocks();
        }
    }
}
