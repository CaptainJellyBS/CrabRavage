using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public static PlayerDamage Instance { get; private set; }
    Renderer[] renderers;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void TakeDamage()
    {
        StartCoroutine(TakeDamageC());
    }

    IEnumerator TakeDamageC()
    {
        GameController.Instance.canTakeDamage = false;
        
        for (float t = GameController.Instance.immunityTime; t >= 0; t-= Time.deltaTime)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = false;
            }

            yield return new WaitForSeconds(GameController.Instance.immunityTime / 10);
            t -= GameController.Instance.immunityTime/10;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = true;
            }

            yield return new WaitForSeconds(GameController.Instance.immunityTime / 10);
            t -= GameController.Instance.immunityTime/10;
        }

        GameController.Instance.canTakeDamage = true;
    }
}
