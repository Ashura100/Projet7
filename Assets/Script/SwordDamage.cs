using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damageAmount = 10; // Montant de d�g�ts inflig�s
    public float attackRange = 1.5f; // Port�e de l'attaque
    public Collider swordCollider;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // V�rifie si l'objet touch� a un composant LifeSys
        LifeSys targetLifeSys = other.GetComponent<LifeSys>();
        if (targetLifeSys != null)
        {
            // Appliquer les d�g�ts
            targetLifeSys.TakeDamage(damageAmount);
        }
    }
}