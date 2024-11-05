using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRV
{
    public class Player : MonoBehaviour
    {
        [SerializeField] LifeSys lifeSys;
        public GameObject death;
        public int health;

        private void Start()
        {
            Time.timeScale = 1;
            lifeSys.onDieDel += Die;
        }
        void Update()
        {
            health = lifeSys.currentHealth;
        }

        // Méthode pour infliger des dégâts au joueur
        public void TakeDamage(int damage)
        {
            lifeSys.TakeDamage(damage);

            // Ici, vous pouvez ajouter d'autres effets visuels ou sonores lors de la prise de dégâts
            if (health <= 0)
            {
                Die(); // Gérer la mort du joueur
            }
        }

        void Die()
        {
            Time.timeScale = 0;
            death.SetActive(true);
            AudioManager.Instance.PlayGameOverSound();
        }
    }

}