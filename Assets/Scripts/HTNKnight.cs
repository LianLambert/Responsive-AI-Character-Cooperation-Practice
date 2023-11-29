using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class HTNKnight : MonoBehaviour
{
    [SerializeField] private GameObject minotaur;
    [SerializeField] private GameObject treasure;
    [SerializeField] private GameObject gameOutcomeManager;
    private List<GameObject> knights = new List<GameObject>();
    private GameObject thief;
    private bool thiefSelected = false;
    private bool meleeKnightsDead = false;
    public UnityEvent StartedHTN = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("knight"))
        {
            knight.GetComponent<Knight>().knightDied.AddListener(StartHTN);
            knight.GetComponent<Knight>().treasureDropped.AddListener(StartHTN);
            knights.Add(knight);
        }

        StartHTN();
    }


    private void StartHTN()
    {
        // Stop any existing coroutine and start the HTN logic
        StopAllCoroutines();
        StartCoroutine(HTN());
    }


    private IEnumerator HTN()
    {
        // only run when the game is not over
        while (Time.timeScale != 0)
        {
            EvaluateConditions();
            ExecuteHTN();
            yield return new WaitForSeconds(2f);
        }
    }

    private void EvaluateConditions()
    {
        thiefSelected = !(thief == null);

        // check if there are ny melee knights
        meleeKnightsDead = true;

        knights.RemoveAll(knight => knight == null);

        foreach (GameObject knight in knights)
        {
            if (knight.GetComponent<KnightMelee>() != null)
            {
                meleeKnightsDead = false;
            }
        }
    }

    private void ExecuteHTN()
    {
        // if the thief is not selected, select it
        if (!thiefSelected)
        {
            SelectThief();
        }

        // update everyone's task based on if they are the thief or not
        UpdateAdventurerRoles();
    }

    private void SelectThief()
    {
        // if there is a knight with the treasure already, pick that one
        if(minotaur.GetComponent<Minotaur>().KnightWithTreasure() != null)
        {
            thief = minotaur.GetComponent<Minotaur>().KnightWithTreasure();
        }

        // if there are no more melee knights, pick any knight
        else if (meleeKnightsDead)
        {
            thief = knights[0];
        }

        // if there are melee knights, pick one of them
        else
        {
            float currentMaxAttackCooldown = 0f;

            foreach (GameObject knight in knights)
            {
                if (HelperMethods.IsMeleeKnight(knight) && knight.GetComponent<KnightMelee>().attackCooldownTimer > currentMaxAttackCooldown)
                {
                    thief = knight;
                    currentMaxAttackCooldown = knight.GetComponent<KnightMelee>().attackCooldownTimer;
                }
            }
        }
    }

    private void UpdateAdventurerRoles()
    {
        // let each adventurer know if they are a thief or not
        foreach (GameObject knight in knights)
        {
            if (HelperMethods.IsMeleeKnight(knight))
            {
                if (knight == thief)
                {
                    knight.GetComponent<KnightMelee>().isThief = true;
                    knight.GetComponent<KnightMelee>().ExecuteTask(new HTNCompositeTask.StealTreasureTask());
                }
                else
                {
                    knight.GetComponent<KnightMelee>().ExecuteTask(new HTNCompositeTask.DistractMinotaurTask());
                }
            }
            else
            {
                if (knight == thief)
                {
                    knight.GetComponent<KnightDistance>().isThief = true;
                    knight.GetComponent<KnightDistance>().ExecuteTask(new HTNCompositeTask.StealTreasureTask());
                }
                else
                {
                    knight.GetComponent<KnightDistance>().ExecuteTask(new HTNCompositeTask.DistractMinotaurTask());
                }
            }
        }
    }
}
