using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndRing : MonoBehaviour
{
    public string levelToUnlock;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerPrefs.SetInt(levelToUnlock, 1);
            SceneManager.LoadScene("Main Menu");
        }
    }
}
