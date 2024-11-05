using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.Video;
using VRV;

public class VRTests
{
    private GameObject testGameObject;
    private VirtualVisit virtualVisit;
    private VirtualVisitVideo virtualVisitVideo;
    private PlayerInterface playerInterface;
    private List<string> skyboxScenes = new List<string> { "VisitScene", "VisitGameScene" }; // Scènes avec skybox
    private string videoScene = "VisitVideoScene"; // Scène avec vidéo

    [SetUp]
    public void Setup()
    {
        // Création des objets de test
        testGameObject = new GameObject("TestObject");
        virtualVisit = testGameObject.AddComponent<VirtualVisit>();
        virtualVisitVideo = testGameObject.AddComponent<VirtualVisitVideo>();
        playerInterface = testGameObject.AddComponent<PlayerInterface>();

        // Configuration des dépendances
        virtualVisit.skybox = new Material(Shader.Find("Skybox/Panoramic"));
        virtualVisit.defaultSkyboxTexture = new Texture2D(1024, 512);
        virtualVisitVideo.videoPlayer = testGameObject.AddComponent<VideoPlayer>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(testGameObject);
    }

    [UnityTest]
    public IEnumerator TestSkyboxScenes()
    {
        foreach (var sceneName in skyboxScenes)
        {
            // Chargement de la scène
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return asyncLoad;

            // Attente d'une frame pour s'assurer du chargement
            yield return null;

            // Test du changement de Skybox
            Texture2D newTexture = new Texture2D(1024, 512);
            virtualVisit.SetSkybox(newTexture);
            Texture currentTexture = virtualVisit.skybox.GetTexture("_MainTex");
            Assert.AreEqual(newTexture, currentTexture, $"La skybox n'a pas changé dans la scène {sceneName}");
            Object.Destroy(newTexture);

            // Nettoyage de la scène chargée
            yield return SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    [UnityTest]
    public IEnumerator TestVideoScene()
    {
        // Chargement de la scène avec vidéo
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(videoScene, LoadSceneMode.Additive);
        yield return asyncLoad;

        // Attente d'une frame pour s'assurer du chargement
        yield return null;

        // Test du changement de vidéo
        VideoClip newVideoClip = Resources.Load<VideoClip>("Videos/Video_VR_02"); // Assurez-vous que la vidéo est dans le dossier Resources.
        Assert.IsNotNull(newVideoClip, "La vidéo n'a pas été chargée. Vérifiez que le fichier Video_VR_02.mp4 est bien dans le dossier Resources.");

        VideoClip initialClip = virtualVisitVideo.videoPlayer.clip;

        // Assigne le nouveau clip et attend une seconde pour le chargement de la vidéo
        virtualVisitVideo.SetSkyboxVideo(newVideoClip);
        yield return new WaitForSeconds(1f);  // Ajoutez un délai pour laisser le temps au VideoPlayer de démarrer

        Assert.AreNotEqual(initialClip, virtualVisitVideo.videoPlayer.clip, "La vidéo n'a pas changé dans la scène de vidéo");
        Assert.AreEqual(newVideoClip, virtualVisitVideo.videoPlayer.clip, "La nouvelle vidéo n'a pas été correctement assignée dans la scène de vidéo");
        Assert.IsTrue(virtualVisitVideo.videoPlayer.isPlaying, "La vidéo devrait être en cours de lecture dans la scène de vidéo");

        // Nettoyage de la scène chargée
        yield return SceneManager.UnloadSceneAsync(videoScene);
    }

    [Test]
    public void TestParameterInterface()
    {
        // Arrange
        GameObject infosObj = new GameObject("Infos");
        playerInterface.Infos = infosObj;
        infosObj.SetActive(false);

        // Act
        playerInterface.InfosButton();

        // Assert
        Assert.IsTrue(infosObj.activeSelf, "L'interface Infos devrait être activée après le premier clic");

        // Test de la désactivation
        playerInterface.InfosButton();
        Assert.IsFalse(infosObj.activeSelf, "L'interface Infos devrait être désactivée après le second clic");

        // Cleanup
        Object.Destroy(infosObj);
    }
}