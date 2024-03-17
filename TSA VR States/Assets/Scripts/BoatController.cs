using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public Transform origin;
    public float xOffset;
    public float yOffset;
    public float zOffset;

    private void Start()
    {
        transform.position = new Vector3(origin.position.x + xOffset, origin.position.y + yOffset, origin.position.z + zOffset);
        transform.rotation = origin.rotation;
    }
}
