using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public void ReplayLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public IEnumerator ExitSequence(string sceneName)
    {
        FindObjectOfType<FadeScreen>().FadeOut();
        yield return new WaitForSeconds(FindObjectOfType<FadeScreen>().fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    public void MainMenu()
    {
        IntroInfo.PerformIntro = false;
        StartCoroutine(ExitSequence("Main Menu"));
    }
}
