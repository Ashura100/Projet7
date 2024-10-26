using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damageAmount = 10; // Montant de dégâts infligés
    public float attackRange = 1.5f; // Portée de l'attaque
    public Collider swordCollider;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet touché a un composant LifeSys
        LifeSys targetLifeSys = other.GetComponent<LifeSys>();
        if (targetLifeSys != null)
        {
            // Appliquer les dégâts
            targetLifeSys.TakeDamage(damageAmount);
        }
    }
}