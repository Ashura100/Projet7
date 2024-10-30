using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] LifeSys lifeSys;
    public GameObject death;
    public int health;

    private void Start()
    {
        lifeSys.onDieDel += Die;
    }
    void Update()
    {
        health = lifeSys.currentHealth;
    }

    // M�thode pour infliger des d�g�ts au joueur
    public void TakeDamage(int damage)
    {
        lifeSys.TakeDamage(damage);

        // Ici, vous pouvez ajouter d'autres effets visuels ou sonores lors de la prise de d�g�ts
        if (health <= 0)
        {
            Die(); // G�rer la mort du joueur
        }
    }

    void Die()
    {
        death.SetActive(true);
        AudioManager.Instance.PlayGameOverSound();
    }
}
