using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndRing : MonoBehaviour
{
    private LevelManager level;

    public string currentLevel;

    public string levelToUnlock;

    void Start()
    {
        currentLevel = currentLevel + "Time";
        level = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            level.Victory();
            PlayerPrefs.SetInt(levelToUnlock, 1);
            if (!PlayerPrefs.HasKey(currentLevel) || (int)level.currentTime < PlayerPrefs.GetInt(currentLevel))
            {
                PlayerPrefs.SetInt(currentLevel, (int)level.currentTime);
            }
        }
    }
}
