using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    public GameObject life0, life1, life2;

    public GameObject pausePanel, deathPanel;

    public Text deathLevel, deathTime;

    public Text levelText, timeText;
    float t = 0;
    int level;
    public int Level
    {
        get { return level; }
        set { level = value; levelText.text = value.ToString(); }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        level = 0;
    }

    private void Update()
    {
        t += Time.deltaTime;
        timeText.text = ParseTime((int)t, t % 1.0f);

        if (Input.GetKeyDown(KeyCode.Escape)) { TogglePausePanel(); }
    }

    string ParseTime(int seconds, float milliseconds)
    {
        int minutes = (seconds / 60);
        seconds = seconds % 60;

        string ofTheJedi = "";
        ofTheJedi += minutes.ToString().PadLeft(2, '0');
        ofTheJedi += ":";
        ofTheJedi += seconds.ToString().PadLeft(2, '0'); ;
        ofTheJedi += ":";
        ofTheJedi += ((int)(milliseconds * 100)).ToString().PadLeft(3, '0');

        return ofTheJedi;
    }

    public void SetLives(int lives)
    {
        if (lives < 3)
        {
            life2.SetActive(false);
        }
        if (lives < 2)
        {
            life1.SetActive(false);
        }
        if (lives < 1)
        {
            life0.SetActive(false);
        }
    }

    public void Die()
    {
        GameController.Instance.StopTime();
        deathLevel.text = level.ToString();
        deathTime.text = ParseTime((int)t, t % 1.0f);
        deathPanel.SetActive(true);
    }

    public void ToMainMenu()
    {
        GameController.Instance.StartTime();
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        GameController.Instance.StartTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePausePanel()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        if (pausePanel.activeSelf) { GameController.Instance.StopTime(); return; }
        GameController.Instance.StartTime();
    }
}
