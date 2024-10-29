using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInterface : MonoBehaviour
{
    public GameObject parametre;
    public GameObject Infos;

    private bool isActive = false;

    public void chargerScene(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
    }

    public void InfosButton()
    {
        isActive = !isActive; // Inverse la valeur de isActive
        Infos.SetActive(isActive);
    }

    public void ParameterButton()
    {
        parametre.SetActive(true);
    }

    // fonction du bouton quitter option désactive le canvas paramètre
    public void ReturnInterface()
    {
        parametre.SetActive(false);
    }
}