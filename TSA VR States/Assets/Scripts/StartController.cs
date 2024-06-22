using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public GameObject startText;
    void Start()
    {
        StartCoroutine(OpeningCoroutine(startText));
    }

    public IEnumerator OpeningCoroutine(GameObject startText)
    {
        startText.GetComponent<FadeText>().FadeIn();
        yield return new WaitForSeconds(startText.GetComponent<FadeText>().fadeTime);
        IntroInfo.PerformIntro = true;
        SceneManager.LoadScene("Main Menu");
    }
}
