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
    private List<string> skyboxScenes = new List<string> { "VisitScene", "VisitGameScene" }; // Sc�nes avec skybox
    private string videoScene = "VisitVideoScene"; // Sc�ne avec vid�o

    [SetUp]
    public void Setup()
    {
        // Cr�ation des objets de test
        testGameObject = new GameObject("TestObject");
        virtualVisit = testGameObject.AddComponent<VirtualVisit>();
        virtualVisitVideo = testGameObject.AddComponent<VirtualVisitVideo>();
        playerInterface = testGameObject.AddComponent<PlayerInterface>();

        // Configuration des d�pendances
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
            // Chargement de la sc�ne
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return asyncLoad;

            // Attente d'une frame pour s'assurer du chargement
            yield return null;

            // Test du changement de Skybox
            Texture2D newTexture = new Texture2D(1024, 512);
            virtualVisit.SetSkybox(newTexture);
            Texture currentTexture = virtualVisit.skybox.GetTexture("_MainTex");
            Assert.AreEqual(newTexture, currentTexture, $"La skybox n'a pas chang� dans la sc�ne {sceneName}");
            Object.Destroy(newTexture);

            // Nettoyage de la sc�ne charg�e
            yield return SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    [UnityTest]
    public IEnumerator TestVideoScene()
    {
        // Chargement de la sc�ne avec vid�o
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(videoScene, LoadSceneMode.Additive);
        yield return asyncLoad;

        // Attente d'une frame pour s'assurer du chargement
        yield return null;

        // Test du changement de vid�o
        VideoClip newVideoClip = Resources.Load<VideoClip>("Videos/Video_VR_02"); // Assurez-vous que la vid�o est dans le dossier Resources.
        Assert.IsNotNull(newVideoClip, "La vid�o n'a pas �t� charg�e. V�rifiez que le fichier Video_VR_02.mp4 est bien dans le dossier Resources.");

        VideoClip initialClip = virtualVisitVideo.videoPlayer.clip;

        // Assigne le nouveau clip et attend une seconde pour le chargement de la vid�o
        virtualVisitVideo.SetSkyboxVideo(newVideoClip);
        yield return new WaitForSeconds(1f);  // Ajoutez un d�lai pour laisser le temps au VideoPlayer de d�marrer

        Assert.AreNotEqual(initialClip, virtualVisitVideo.videoPlayer.clip, "La vid�o n'a pas chang� dans la sc�ne de vid�o");
        Assert.AreEqual(newVideoClip, virtualVisitVideo.videoPlayer.clip, "La nouvelle vid�o n'a pas �t� correctement assign�e dans la sc�ne de vid�o");
        Assert.IsTrue(virtualVisitVideo.videoPlayer.isPlaying, "La vid�o devrait �tre en cours de lecture dans la sc�ne de vid�o");

        // Nettoyage de la sc�ne charg�e
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
        Assert.IsTrue(infosObj.activeSelf, "L'interface Infos devrait �tre activ�e apr�s le premier clic");

        // Test de la d�sactivation
        playerInterface.InfosButton();
        Assert.IsFalse(infosObj.activeSelf, "L'interface Infos devrait �tre d�sactiv�e apr�s le second clic");

        // Cleanup
        Object.Destroy(infosObj);
    }
}