using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Slider volumeSlider;
    public float fadeTimeIn, fadeTimeJump;
    public Image fader;
    public GameObject settingsPanel, creditsPanel;

    private void Start()
    {
        volumeSlider.value = AudioListener.volume;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameC());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        creditsPanel.SetActive(false);
    }

    public void ToggleCredits()
    {
        creditsPanel.SetActive(!creditsPanel.activeSelf);
        settingsPanel.SetActive(false);
    }

    IEnumerator StartGameC()
    {
        fader.gameObject.SetActive(true);
        for(float t = 0; t<fadeTimeIn; t+= Time.deltaTime)
        {
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, t / fadeTimeIn);
            yield return null;
        }

        yield return new WaitForSeconds(fadeTimeJump);
        SceneManager.LoadScene(1);
    }
}
