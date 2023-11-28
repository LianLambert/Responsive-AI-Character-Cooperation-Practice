using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeKnight : Knight
{
    [SerializeField] private GameObject attackSphere;
    private float attackRadius = 2;

    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
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

    protected override bool CanAttack()
    {
        return Distance2D(minotaur, this.gameObject) <= attackRadius;
    }

    protected override void Attack()
    {
        // can only attack if already cooled down
        if (attackCooldownTimer >= attackCooldown)
        {
            myAnimator.SetTrigger("Attack");
            attackCooldownTimer = 0f;
            minotaur.GetComponent<Minotaur>().knightAttacking = this.gameObject;
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
