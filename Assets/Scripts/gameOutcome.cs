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
    private bool gameLost = false;
    private int initialKnightCount = 4;

    // Start is called before the first frame update
    void Start()
    {
        gameLostText.enabled = false;
        gameWonText.enabled = false;

        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("knight"))
        {
            knight.GetComponent<Knight>().gameWon.AddListener(OnGameWon);
            knight.GetComponent<Knight>().knightDied.AddListener(OnKnightDied);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // when the game finishes, pause all characters and display appropriate message
        if (gameLost)
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

    private void OnKnightDied()
    {
        initialKnightCount -= 1;

        // game is lost if every knight dies
        if (initialKnightCount == 0)
        {
            gameLost = true;
        }
    }

    private void PauseNavMeshAgents()
    {
        NavMeshAgent[] navMeshAgents = FindObjectsOfType<NavMeshAgent>();

        // pause each navmesh agent
        foreach (NavMeshAgent agent in navMeshAgents)
        {
            if (agent != null)
            {
                agent.isStopped = true;
            }
        }
    }
}
