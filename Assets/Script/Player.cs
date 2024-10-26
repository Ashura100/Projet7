using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    [SerializeField] LifeSys lifeSys;
    public int health;

    void Update()
    {
        health = lifeSys.currentHealth;
    }

    // M�thode pour infliger des d�g�ts au joueur
    public void TakeDamage(int damage)
    {
        lifeSys.TakeDamage(damage);

        // Ici, vous pouvez ajouter d'autres effets visuels ou sonores lors de la prise de d�g�ts
        if (!lifeSys.isAlife)
        {
            Die(); // G�rer la mort du joueur
        }
    }

    void Die()
    {

    }
}
