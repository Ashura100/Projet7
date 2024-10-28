using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knight : MonoBehaviour
{
    public int health;
    public Transform player;
    public float attackDistance = 2.0f;
    public float chaseDistance = 0.0f;
    public int damage = 10;
    public float attackCooldown = 1.5f; // Temps entre les attaques

    [SerializeField] LifeSys lifeSys;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;

    [SerializeField] private BoxCollider attackCollider;
    public LayerMask playerLayer;

    private enum State { Idle, Chase, Attack, Death }
    private State currentState;
    private float lastAttackTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.stoppingDistance = attackDistance;
        agent.updateRotation = false;
        attackCollider.isTrigger = true;
        currentState = State.Idle;
        lifeSys.onDieDel += Die;
    }

    private void Update()
    {
        if (currentState == State.Death) return; // Ne rien faire si en état de mort

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        health = lifeSys.currentHealth;

        switch (currentState)
        {
            case State.Idle:
                IdleState(distanceToPlayer);
                break;
            case State.Chase:
                ChaseState(distanceToPlayer);
                break;
            case State.Attack:
                AttackState(distanceToPlayer);
                break;
            case State.Death:
                //DeathState();
                break;
        }
    }

    private void IdleState(float distanceToPlayer)
    {
        animator.SetBool("Run", false);

        if (distanceToPlayer <= chaseDistance)
        {
            TransitionToState(State.Chase);
        }
    }

    private void ChaseState(float distanceToPlayer)
    {
        animator.SetBool("Run", true);
        LookAtPlayer();
        agent.SetDestination(player.position);

        if (distanceToPlayer <= attackDistance)
        {
            TransitionToState(State.Attack);
        }
        else if (distanceToPlayer > chaseDistance)
        {
            TransitionToState(State.Idle);
        }
    }

    private void AttackState(float distanceToPlayer)
    {
        agent.isStopped = true;
        rb.velocity = Vector3.zero;
        animator.SetBool("Run", false);

        LookAtPlayer();

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            AudioManager.Instance.PlaySwordSound();
            animator.SetTrigger("Attack");
            StartAttack();
            lastAttackTime = Time.time;
        }

        if (distanceToPlayer > attackDistance)
        {
            EndAttack();
            agent.isStopped = false;
            TransitionToState(State.Chase);
        }
    }

    private void DeathState()
    {
        // Stop toutes les actions et animations
        agent.isStopped = true;
        rb.velocity = Vector3.zero;
        animator.SetBool("Run", false);
        animator.SetTrigger("Die"); // Déclenche l'animation de mort
        AudioManager.Instance.PlayDeathSound();
        Destroy(gameObject, 2f); // Détruit le chevalier après 2 secondes
    }

    private void TransitionToState(State newState)
    {
        if(currentState == newState) return;

        Debug.Log("Transition vers l'état : " + newState);
        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                EndAttack();
                break;
            case State.Chase:
                EndAttack();
                break;
            case State.Attack:
                break;
            case State.Death:
                EndAttack();
                DeathState();
                break;
        }
    }

    public void StartAttack()
    {
        attackCollider.isTrigger = true;
    }

    public void EndAttack()
    {
        attackCollider.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            LifeSys lifeSystem = other.GetComponent<LifeSys>();
            if (lifeSystem != null)
            {
                lifeSystem.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        lifeSys.healthbar.UpdateHealth();

        if (health <= 0 && currentState != State.Death) // Evite ré-activation de Death
        {
            Die();
        }
    }

    private void Die()
    {
        TransitionToState(State.Death);
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}