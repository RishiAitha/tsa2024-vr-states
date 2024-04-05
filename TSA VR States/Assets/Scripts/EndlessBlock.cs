using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBlock : MonoBehaviour
{
    private EndlessManager endless;

    public GameObject obstacles;

    public int index;

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

        GameObject obstacle1 = Instantiate(endless.obstaclePrefabs[Random.Range(0, endless.obstaclePrefabs.Length)]);
        obstacle1.transform.SetParent(obstacles.transform);
        if (obstacle1.GetComponent<SpeedRing>() != null || obstacle1.GetComponent<SlowRing>() != null)
        {
            obstacle1.transform.localPosition = new Vector3(Random.Range(-27f, -15f), -1.3f, -7f);
        }
        else
        {
            obstacle1.transform.localPosition = new Vector3(Random.Range(-27f, -15f), -2.3f, -7f);
        }
        
        GameObject obstacle2 = Instantiate(endless.obstaclePrefabs[Random.Range(0, endless.obstaclePrefabs.Length)]);
        obstacle2.transform.SetParent(obstacles.transform);
        if (obstacle2.GetComponent<SpeedRing>() != null || obstacle2.GetComponent<SlowRing>() != null)
        {
            obstacle2.transform.localPosition = new Vector3(Random.Range(-27f, -15f), -1.3f, 23f);
        }
        else
        {
            obstacle2.transform.localPosition = new Vector3(Random.Range(-27f, -15f), -2.3f, 23f);
        }

        GameObject obstacle3 = Instantiate(endless.obstaclePrefabs[Random.Range(0, endless.obstaclePrefabs.Length)]);
        obstacle3.transform.SetParent(obstacles.transform);
        if (obstacle3.GetComponent<SpeedRing>() != null || obstacle3.GetComponent<SlowRing>() != null)
        {
            obstacle3.transform.localPosition = new Vector3(Random.Range(-27f, -15f), -1.3f, 53f);
        }
        else
        {
            obstacle3.transform.localPosition = new Vector3(Random.Range(-27f, -15f), -2.3f, 53f);
        }
    }
}
