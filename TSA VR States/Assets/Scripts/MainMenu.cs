using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject levelMenu;
    public GameObject settingsMenu;
    public GameObject[] tutorialPages;
    public GameObject[] endlessPages;
    public GameObject playerCamera;
    public Button[] levelButtons;
    public TextMeshProUGUI[] levelTimeTexts;
    public Toggle allowUsingHandsInput;
    public Toggle showRowingPointsInput;
    public Toggle muteMusicInput;
    public Slider musicVolumeInput;
    public float introTime;
    public GameObject origin;
    public GameObject[] introObjects;

    private MusicController music;

    public int tutorialPage;
    public int endlessPage;

    public void Start()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }
        foreach (GameObject page in endlessPages)
        {
            page.SetActive(false);
        }

        tutorialPage = 0;

        music = FindObjectOfType<MusicController>();

        InitializeSettings();
        InitializeLevels();

        StartCoroutine("IntroSequence");
    }

    public IEnumerator IntroSequence()
    {
        if (!IntroInfo.PerformIntro)
        {
            origin.transform.position = Vector3.zero;
        }

        FindObjectOfType<FadeScreen>().FadeIn();

        yield return new WaitForSeconds(FindObjectOfType<FadeScreen>().fadeTime);

        if (IntroInfo.PerformIntro)
        {
            float counter = 0f;
            float distance = Mathf.Abs(Vector3.Distance(origin.transform.position, Vector3.zero));

            while (counter < introTime)
            {
                counter += Time.deltaTime;

                origin.transform.position = Vector3.MoveTowards(origin.transform.position, Vector3.zero, (distance / introTime) * Time.deltaTime);

                int length = introObjects.Length;
                for (int i = 0; i < length; i++)
                {
                    float startingTime = (introTime / length) * i;
                    float endingTime = startingTime + (introTime / length);
                    if (counter > startingTime && counter < endingTime)
                    {
                        if (!introObjects[i].GetComponent<Animator>().GetBool("rise"))
                        {
                            introObjects[i].GetComponent<Animator>().SetBool("rise", true);
                        }
                    }
                    else if (counter > endingTime)
                    {
                        if (introObjects[i].GetComponent<Animator>().enabled)
                        {
                            introObjects[i].GetComponent<Animator>().enabled = false;
                        }
                    }
                }

                yield return null;
            }
        }
    }

    public IEnumerator ExitSequence(string sceneName)
    {
        FindObjectOfType<FadeScreen>().FadeOut();
        yield return new WaitForSeconds(FindObjectOfType<FadeScreen>().fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    public void Update()
    {
        transform.position = new Vector3(transform.position.x, playerCamera.transform.position.y - 0.176f, transform.position.z);
    }

    public void OpenLevelSelect()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(true);
        settingsMenu.SetActive(false);

        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }
        foreach (GameObject page in endlessPages)
        {
            page.SetActive(false);
        }
    }

    public void OpenSettingsMenu()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(true);

        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }
        foreach (GameObject page in endlessPages)
        {
            page.SetActive(false);
        }
    }

    public void BackToMain()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }
        foreach (GameObject page in endlessPages)
        {
            page.SetActive(false);
        }
    }

    public void OpenTutorial()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        tutorialPage = 0;

        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }

        tutorialPages[tutorialPage].SetActive(true);
    }

    public void NextTutorial()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        tutorialPage++;

        if (tutorialPage >= tutorialPages.Length)
        {
            StartCoroutine(ExitSequence("Level Tutorial"));
        }
        else
        {
            foreach (GameObject page in tutorialPages)
            {
                page.SetActive(false);
            }

            tutorialPages[tutorialPage].SetActive(true);
        }
    }

    public void PreviousTutorial()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        tutorialPage--;

        if (tutorialPage < 0)
        {
            OpenLevelSelect();
        }
        else
        {
            foreach (GameObject page in tutorialPages)
            {
                page.SetActive(false);
            }

            tutorialPages[tutorialPage].SetActive(true);
        }
    }

    public void OpenEndless()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        endlessPage = 0;

        foreach (GameObject page in endlessPages)
        {
            page.SetActive(false);
        }

        endlessPages[endlessPage].SetActive(true);
    }

    public void NextEndless()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        endlessPage++;

        if (endlessPage >= endlessPages.Length)
        {
            StartCoroutine(ExitSequence("Endless"));
        }
        else
        {
            foreach (GameObject page in endlessPages)
            {
                page.SetActive(false);
            }

            endlessPages[endlessPage].SetActive(true);
        }
    }

    public void PreviousEndless()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);

        endlessPage--;

        if (endlessPage < 0)
        {
            OpenLevelSelect();
        }
        else
        {
            foreach (GameObject page in endlessPages)
            {
                page.SetActive(false);
            }

            endlessPages[endlessPage].SetActive(true);
        }
    }

    private void InitializeLevels()
    {
        if (!PlayerPrefs.HasKey("Endless"))
        {
            PlayerPrefs.SetInt("Endless", 0);
        }

        if (!PlayerPrefs.HasKey("Level1"))
        {
            PlayerPrefs.SetInt("Level1", 0);
        }

        if (!PlayerPrefs.HasKey("Level2"))
        {
            PlayerPrefs.SetInt("Level2", 0);
        }

        if (!PlayerPrefs.HasKey("Level3"))
        {
            PlayerPrefs.SetInt("Level3", 0);
        }

        foreach (Button button in levelButtons)
        {
            button.interactable = false;
        }

        if (PlayerPrefs.HasKey("TutorialTime"))
        {
            levelTimeTexts[0].text = levelTimeTexts[0].text + "\nBest Time: " + PlayerPrefs.GetInt("TutorialTime").ToString() + " seconds";
        }

        if (PlayerPrefs.GetInt("Level1") == 1)
        {
            levelButtons[0].interactable = true;
            if (PlayerPrefs.HasKey("Level1Time"))
            {
                levelTimeTexts[1].text = levelTimeTexts[1].text + "\nBest Time: " + PlayerPrefs.GetInt("Level1Time").ToString() + " seconds";
            }

            PlayerPrefs.SetInt("Endless", 1);
            levelButtons[3].interactable = true;
            if (PlayerPrefs.HasKey("EndlessScore"))
            {
                levelTimeTexts[4].text = "High Score: " + PlayerPrefs.GetInt("EndlessScore").ToString();
            }
        }

        if (PlayerPrefs.GetInt("Level2") == 1)
        {
            levelButtons[1].interactable = true;
            if (PlayerPrefs.HasKey("Level2Time"))
            {
                levelTimeTexts[2].text = levelTimeTexts[2].text + "\nBest Time: " + PlayerPrefs.GetInt("Level2Time").ToString() + " seconds";
            }
        }

        if (PlayerPrefs.GetInt("Level3") == 1)
        {
            levelButtons[2].interactable = true;
            if (PlayerPrefs.HasKey("Level3Time"))
            {
                levelTimeTexts[3].text = levelTimeTexts[3].text + "\nBest Time " + PlayerPrefs.GetInt("Level3Time").ToString() + " seconds";
            }
        }
    }

    public void LoadLevel1()
    {
        StartCoroutine(ExitSequence("Level 1"));
    }
    public void LoadLevel2()
    {
        StartCoroutine(ExitSequence("Level 2"));
    }
    public void LoadLevel3()
    {
        StartCoroutine(ExitSequence("Level 3"));
    }

    private void InitializeSettings()
    {
        if (!PlayerPrefs.HasKey("AllowHands"))
        {
            PlayerPrefs.SetInt("AllowHands", 0);
        }

        if (!PlayerPrefs.HasKey("ShowPoints"))
        {
            PlayerPrefs.SetInt("ShowPoints", 0);
        }

        if (!PlayerPrefs.HasKey("MuteMusic"))
        {
            PlayerPrefs.SetInt("MuteMusic", 0);
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        }

        int allowHands = PlayerPrefs.GetInt("AllowHands");
        if (allowHands == 0)
        {
            allowUsingHandsInput.isOn = false;
        }
        else
        {
            allowUsingHandsInput.isOn = true;
        }

        int showPoints = PlayerPrefs.GetInt("ShowPoints");
        if (showPoints == 0)
        {
            showRowingPointsInput.isOn = false;
        }
        else
        {
            showRowingPointsInput.isOn = true;
        }

        int muteMusic = PlayerPrefs.GetInt("MuteMusic");
        if (muteMusic == 0)
        {
            muteMusicInput.isOn = false;
        }
        else
        {
            muteMusicInput.isOn = true;
        }

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        musicVolumeInput.value = musicVolume;

        music.UpdateMusic();
    }

    public void ChangeAllowHands()
    {
        if (allowUsingHandsInput.isOn)
        {
            PlayerPrefs.SetInt("AllowHands", 1);
        }
        else
        {
            PlayerPrefs.SetInt("AllowHands", 0);
        }
    }

    public void ChangeShowPoints()
    {
        if (showRowingPointsInput.isOn)
        {
            PlayerPrefs.SetInt("ShowPoints", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ShowPoints", 0);
        }
    }

    public void ChangeMuteMusic()
    {
        if (muteMusicInput.isOn)
        {
            PlayerPrefs.SetInt("MuteMusic", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MuteMusic", 0);
        }

        music.UpdateMusic();
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeInput.value);

        music.UpdateMusic();
    }

    public void ResetSaveData()
    {
        PlayerPrefs.SetInt("Level1", 0);
        PlayerPrefs.SetInt("Level2", 0);
        PlayerPrefs.SetInt("Level3", 0);
        PlayerPrefs.SetInt("Endless", 0);
        PlayerPrefs.SetInt("AllowHands", 0);
        PlayerPrefs.SetInt("ShowPoints", 0);
        PlayerPrefs.SetInt("MuteMusic", 0);
        PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        PlayerPrefs.DeleteKey("TutorialTime");
        PlayerPrefs.DeleteKey("Level1Time");
        PlayerPrefs.DeleteKey("Level2Time");
        PlayerPrefs.DeleteKey("Level3Time");
        PlayerPrefs.DeleteKey("EndlessScore");
        StartCoroutine(ExitSequence("Start Menu"));
    }
}
