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
    public float fieldOfViewAngle = 45f;  // Champ de vision en degrés
    public float idleBlendSpeed = 0.1f;   // Vitesse de transition entre animations d'Idle

    [SerializeField] LifeSys lifeSys;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] private BoxCollider attackCollider;

    private enum State { Idle, Chase, Attack, Death }
    private State currentState;
    private float lastAttackTime;
    private bool isAttackActive;
    private bool isPlayerInSight;

    private void Start()
    {
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
        isPlayerInSight = IsPlayerInFieldOfView();

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

        // Activer le Blend Tree d'Idle
        animator.SetFloat("IdleBlend", Random.Range(0f, 1f), idleBlendSpeed, Time.deltaTime);

        if (isPlayerInSight && distanceToPlayer <= chaseDistance)
        {
            AudioManager.Instance.PlayScreamSound();
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

        // Si on quitte l'état Idle, arrêter le blend tree en fixant IdleBlend à 0
        if (currentState == State.Idle)
        {
            animator.SetFloat("IdleBlend", 0f);  // Réinitialiser l'IdleBlend
        }

        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                EndAttack();
                break;
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

    private bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle < fieldOfViewAngle / 2f;
    }
}