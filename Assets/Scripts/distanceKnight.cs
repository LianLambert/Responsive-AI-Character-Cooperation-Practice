using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceKnight : Knight
{
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
        maxHits = (int)Mathf.Floor(maxHits / 2);
        myAnimator = GetComponent<Animator>();

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

    protected override bool CanAttack()
    {
        // cast a ray
        Vector3 direction = minotaur.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
        {
            // if the ray hits the minotaur, it is visible
            if (hit.collider.gameObject == minotaur)
            {
                return true;
            }
        }

        return false;
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
        // draw a blue line from knight to minotaur for 1s
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, minotaur.transform.position);
        yield return new WaitForSeconds(1f);
        lineRenderer.enabled = false;
    }
}
