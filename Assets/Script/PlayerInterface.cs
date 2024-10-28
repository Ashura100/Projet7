using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInterface : MonoBehaviour
{
    public GameObject parametre;
    public GameObject Infos;

    public bool isActive = false;

    public void chargerScene(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
    }

    public void InfosButton()
    {
        if (isActive)
        {
            Infos.SetActive(true);
            isActive = true;
        }
        else
        {
            Infos.SetActive(false);
            isActive = false;
        }
        
    }

    public void ParameterButton()
    {
        parametre.SetActive(true);
    }
    //fonction du bouton quitter option d�sactive le canvas param�tre
    public void ReturnInterface()
    {
        parametre.SetActive(false);
    }
}
