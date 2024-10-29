using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knight : MonoBehaviour
{
    public int health;
    public Transform player;
    public float attackDistance = 2.0f;
    public float chaseDistance = 5.0f;
    public int damage = 10;
    public float attackCooldown = 2.0f;

    [SerializeField] LifeSys lifeSys;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] private BoxCollider attackCollider;

    private enum State { Idle, Chase, Attack, TakeDamage, Death }
    private State currentState;
    private State previousState; // État précédent pour revenir après TakeDamage
    private float lastAttackTime;
    private bool isAttackActive;
    private float takeDamageCooldown = 1.0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.stoppingDistance = attackDistance;
        agent.updateRotation = false;
        attackCollider.isTrigger = true;
        currentState = State.Idle;
        lifeSys.onDieDel += Die;

        EndAttack();
    }

    private void Update()
    {
        if (currentState == State.Death) return;

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
                break;
        }
    }

    private void IdleState(float distanceToPlayer)
    {
        animator.SetBool("Run", false);
        AudioManager.Instance.PlayScreamSound();

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
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;

            Invoke(nameof(StartAttack), 0.5f);
            AudioManager.Instance.PlaySwordSound();
            Invoke(nameof(EndAttack), 1.0f);
        }

        if (distanceToPlayer > attackDistance)
        {
            agent.isStopped = false;
            TransitionToState(State.Chase);
        }
    }

    private void DeathState()
    {
        agent.isStopped = true;
        rb.velocity = Vector3.zero;
        animator.SetBool("Run", false);
        animator.SetTrigger("Die");
        AudioManager.Instance.PlayDeathSound();
        Destroy(gameObject, 2f);
    }

    private void TransitionToState(State newState)
    {
        if (currentState == newState) return;

        Debug.Log("Transition vers l'état : " + newState);

        currentState = newState;

        switch (newState)
        {
            case State.Idle:
            case State.Chase:
                EndAttack();
                break;
            case State.Death:
                EndAttack();
                DeathState();
                break;
        }
    }

    private void StartAttack()
    {
        isAttackActive = true;
        attackCollider.enabled = true;
    }

    private void EndAttack()
    {
        isAttackActive = false;
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttackActive) return;

        LifeSys lifeSystem = player.GetComponent<LifeSys>();
        if (lifeSystem != null)
        {
            lifeSystem.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        lifeSys.healthbar.UpdateHealth();

        if (health <= 0 && currentState != State.Death)
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