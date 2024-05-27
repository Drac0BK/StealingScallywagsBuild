using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //changes the scene to a corresponding one 
    PlayerConfigManager playerConfigManager;
    public GameObject LoadingScreen;
    AudioManager audioManager;

    public void Menu()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
            Destroy(audioManager.gameObject);
        Time.timeScale = 1;
        playerConfigManager = FindObjectOfType<PlayerConfigManager>();
        if (playerConfigManager != null)
            Destroy(GameObject.Find("PlayerConfigurationManager"));
        SceneManager.LoadScene("MusicMainMenu");
    }
    public void CharacterSelect()
    {
        playerConfigManager = FindObjectOfType<PlayerConfigManager>();
        if(playerConfigManager != null)
            Destroy(playerConfigManager);
        SceneManager.LoadScene("PlayerSelect");
    }
    public void Level1Load()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
            Destroy(audioManager.gameObject);
        SceneManager.LoadScene("Lvl1Scene");
        //StartCoroutine(LoadSceneAsync("Lvl1Scene"));
    }
    public void Level2Load()
    {
        SceneManager.LoadScene("Lvl2Scene");
        //StartCoroutine(LoadSceneAsync("Lvl2Scene"));
    }
    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LevelSelectScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelect");
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneAsync(string sceneId)
    {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        //if(LoadingScreen!= null)
        //LoadingScreen.SetActive(true);
        //
        //while (!operation.isDone)
        //{
        //    float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
        //
        //    //Loading bar
        //
        //    yield return null;
        //}
        yield return new WaitForSecondsRealtime(0f);
    }
   
}
