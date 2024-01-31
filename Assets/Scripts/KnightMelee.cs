using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KnightMelee : Knight
{
    [SerializeField] private GameObject attackSphere;
    private float attackRadius = 6f;

    // Start is called before the first frame update
    void Awake()
    {
        myAnimator = GetComponent<Animator>();

        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("knight"))
        {
            if (knight != this)
            {
                knight.GetComponent<Knight>().knightDied.AddListener(OnOtherKnightDied);
                otherKnights.Add(knight);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update timers and animation
        UpdateTimers();
        myAnimator.SetBool("isMoving", agent.velocity.magnitude > 0.05f);

        if (attacked)
        {
            attacked = false;
            Attacked();
        }
    }

    public override bool CanAttack()
    {
        return (Distance2D(minotaur, gameObject) <= attackRadius) && attackCooldownTimer >= attackCooldown;
    }

    public override void Attack()
    {
        // can only attack if already cooled down
        if (attackCooldownTimer >= attackCooldown)
        {
            myAnimator.SetTrigger("Attack");
            attackCooldownTimer = 0f;
            minotaur.GetComponent<Minotaur>().knightAttacking = gameObject;
            minotaur.GetComponent<Minotaur>().attackedThisFrame = true;
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        // activate the attackSphere for 1s
        attackSphere.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackSphere.SetActive(false);
    }
}
