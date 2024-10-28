using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualVisit : MonoBehaviour
{
    public Material skybox;
    public Texture2D defaultSkyboxTexture; // Ajoutez une texture par d�faut

    private void Start()
    {
        // Assurez-vous que la skybox commence avec la texture par d�faut
        if (defaultSkyboxTexture != null)
        {
            SetSkybox(defaultSkyboxTexture);
        }
    }

    public void SetSkybox(Texture2D texture2D)
    {
        skybox.SetTexture("_MainTex", texture2D);
    }
}
