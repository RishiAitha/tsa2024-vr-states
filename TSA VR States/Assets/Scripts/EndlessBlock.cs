using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBlock : MonoBehaviour
{
    private EndlessManager endless;

    public GameObject obstacles;

    public int index;

    public float leftBound1;
    public float leftBound2;
    public float leftBound3;
    public float rightBound1;
    public float rightBound2;
    public float rightBound3;

    void Start()
    {
        endless = FindObjectOfType<EndlessManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Rigidbody>().velocity.z >= 0f && index != 0)
        {
            endless.MoveBlocks();
        }
    }

    public void Randomize()
    {
        for (int i = obstacles.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(obstacles.transform.GetChild(i).gameObject);
        }

        CreateObstacle(leftBound1, rightBound1, -7f);
        CreateObstacle(leftBound2, rightBound2, 23f);
        CreateObstacle(leftBound3, rightBound3, 53f);
    }

    public void CreateObstacle(float leftBound, float rightBound, float z)
    {
        GameObject obstacle = Instantiate(endless.obstaclePrefabs[Random.Range(0, endless.obstaclePrefabs.Length)]);
        obstacle.transform.SetParent(obstacles.transform);
        if (obstacle.GetComponent<SpeedRing>() != null || obstacle.GetComponent<SlowRing>() != null)
        {
            obstacle.transform.localPosition = new Vector3(Random.Range(leftBound, rightBound), -1.3f, z);
        }
        else
        {
            obstacle.transform.localPosition = new Vector3(Random.Range(leftBound, rightBound), -2.3f, z);
        }
    }
}
