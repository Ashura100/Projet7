using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VRV
{
    public class DisplayStats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI display_Text; // Affichage FPS
        [SerializeField] private TextMeshProUGUI triangle_Text; // Affichage triangles

        private float avgFrameRate;

        private void Start()
        {
            InvokeRepeating("DisplayFrameRate", 1f, 1f);  // Appel toutes les secondes pour le framerate
            InvokeRepeating("CountAllTriangles", 1f, 5f); // Appel toutes les 5 secondes pour les triangles
        }

        private void DisplayFrameRate()
        {
            float current = 1f / Time.unscaledDeltaTime; // Calcul du FPS instantané
            avgFrameRate = (int)current; // Conversion en entier pour un affichage propre
            display_Text.text = avgFrameRate.ToString() + " FPS"; // Affichage des FPS dans le TextMeshPro
        }

        private void CountAllTriangles()
        {
            int totalTriangles = 0;

            // Trouver tous les MeshFilters dans la scène
            MeshFilter[] allMeshFilters = UnityEngine.Object.FindObjectsOfType<MeshFilter>();
            foreach (MeshFilter meshFilter in allMeshFilters)
            {
                // Vérifier si le mesh est lisible
                if (meshFilter.sharedMesh != null && meshFilter.sharedMesh.isReadable)
                {
                    totalTriangles += meshFilter.sharedMesh.triangles.Length / 3;
                }
                else
                {
                    Debug.LogWarning(meshFilter.name + " mesh is not readable.");
                }
            }

            // Affichage du total de triangles dans le TextMeshPro
            triangle_Text.text = "Total triangles: " + totalTriangles;
        }
    }
}