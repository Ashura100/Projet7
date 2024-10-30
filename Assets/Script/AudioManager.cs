using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicGame;
    public AudioSource sfxPlayer;

    [SerializeField]
    private AudioClip clickSound, gameClickSound, HomeSound, themeSound, gameSound, swordSound, screamSound, deathSound, winSound, gameOverSound;

    private void Awake()
    {

        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        // Ajoute l'�couteur pour le changement de sc�ne
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Cette m�thode sera appel�e � chaque changement de sc�ne
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    // D�termine quelle musique jouer en fonction du nom de la sc�ne
    private void PlayMusicForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Home":
                Home();
                break;
            case "VisitScene":
                PlayTheme();
                break;
            case "VisitGameScene":
                PlayGameTheme();
                break;
            case "Win":
                PlayWinSound();
                break;
            default:
                StopCurrentSound();
                break;
        }
    }

    //joue le th�me de l'ui de cr�ation de compte
    public void Home()
    {
        musicGame.clip = HomeSound;
        musicGame.Play();
    }

    //joue le theme de l'ui du menu start
    public void PlayTheme()
    {
        musicGame.clip = themeSound;
        musicGame.Play();
    }

    //joue le th�me du jeu
    public void PlayGameTheme()
    {
        musicGame.clip = gameSound;
        musicGame.Play();
    }

    //joue les son click mineur
    public void PlayClickSound()
    {
        sfxPlayer.clip = clickSound;
        sfxPlayer.Play();
    }

    public void PlayScreamSound()
    {
        sfxPlayer.clip = screamSound;
        sfxPlayer.Play();
    }

    public void PlaySwordSound()
    {
        sfxPlayer.clip = swordSound;
        sfxPlayer.Play();
    }

    public void PlayDeathSound()
    {
        sfxPlayer.clip = deathSound;
        sfxPlayer.Play();
    }

    public void PlayWinSound()
    {
        sfxPlayer.clip = winSound;
        sfxPlayer.Play();
    }

    //joue le son de d�faite
    public void PlayGameOverSound()
    {
        sfxPlayer.clip = gameOverSound;
        sfxPlayer.Play();
    }

    //arr�te les sons
    public void StopCurrentSound()
    {
        musicGame.Stop();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}