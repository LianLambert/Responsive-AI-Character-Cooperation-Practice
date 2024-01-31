using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KnightDistance : Knight
{
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        maxHits = (int)Mathf.Floor(maxHits / 2);
        myAnimator = transform.GetComponent<Animator>();

        // set up line renderer to show attack
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.material.color = Color.blue;

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
        bool canSee = false;
        // cast a ray
        Vector3 direction = minotaur.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
        {
            // if the ray hits the minotaur, it is visible
            if (hit.collider.gameObject == minotaur)
            {
                canSee = true;
            }
        }

        return canSee && (attackCooldownTimer >= attackCooldown);
    }


    public override void Attack()
    {
        // can only attack if already cooled down
        if (attackCooldownTimer >= attackCooldown)
        {
            myAnimator.SetTrigger("Attack");
            attackCooldownTimer = 0f;
            minotaur.GetComponent<Minotaur>().knightAttacking = this.gameObject;
            minotaur.GetComponent<Minotaur>().attackedThisFrame = true;
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        // draw a blue line from knight to minotaur for 1s
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, minotaur.transform.position);
        yield return new WaitForSeconds(1f);
        lineRenderer.enabled = false;
    }
}
