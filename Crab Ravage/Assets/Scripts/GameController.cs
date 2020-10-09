using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int lives { get; private set; }
    public int crabHealth { get; private set; }

    public PlayerDamage playerDam;

    public static GameController Instance { get; private set; }

    public bool canTakeDamage = true;
    public float immunityTime;

    public DifficultyLevel[] difficultyLevels;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lives = 3;
        crabHealth = 200;
        StartCoroutine(HandleDifficulties());
    }

    #region damage

    public void LoseLife()
    {
        lives--;
        Debug.Log("TookDamage");

        UIController.Instance.SetLives(lives);
        PlayerDamage.Instance.TakeDamage();

        if(lives<=0)
        {
            Die();
        }

    }
    void Die()
    {
        Debug.Log("You ded");
        UIController.Instance.Die();
    }

    #endregion

    IEnumerator HandleDifficulties()
    {
        for (int i = 0; i < difficultyLevels.Length; i++)
        {
            difficultyLevels[i].SetDifficulty();
            yield return new WaitForSeconds(difficultyLevels[i].duration);
        }
    }

    public void StopTime()
    {
        Time.timeScale = 0.0f;
    }

    public void StartTime()
    {
        Time.timeScale = 1.0f;
    }
}
