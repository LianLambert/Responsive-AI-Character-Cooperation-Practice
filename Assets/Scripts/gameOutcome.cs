using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class gameOutcome : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameWonText;
    [SerializeField] private TextMeshProUGUI gameLostText;
    private bool gameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("knight"))
        {
            knight.GetComponent<Knight>().gameWon.AddListener(OnGameWon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLost())
        {
            PauseNavMeshAgents();
            Time.timeScale = 0;
            gameLostText.enabled = true;
        }
        else if (gameWon)
        {
            PauseNavMeshAgents();
            Time.timeScale = 0;
            gameWonText.enabled = true;
        }
    }

    private void OnGameWon()
    {
        gameWon = true;
    }

    private bool gameLost()
    {
        GameObject[] knightsArray = GameObject.FindGameObjectsWithTag("knight");

        if (knightsArray == null || knightsArray.Length == 0)
        {
            return true;
        }

        return false;
    }

    private void PauseNavMeshAgents()
    {
        NavMeshAgent[] navMeshAgents = FindObjectsOfType<NavMeshAgent>();

        foreach (NavMeshAgent agent in navMeshAgents)
        {
            agent.isStopped = true;
        }
    }
}
