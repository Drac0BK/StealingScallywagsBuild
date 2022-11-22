using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    PlayerConfigManager playerConfigManager;
    public GameObject LoadingScreen;

    public void Menu()
    {
        Time.timeScale = 1;
        PlayerConfigManager playerConfigManager;
        playerConfigManager = FindObjectOfType<PlayerConfigManager>();
        if (playerConfigManager != null)
            Destroy(GameObject.Find("PlayerConfigurationManager"));
        SceneManager.LoadScene("MainMenu");
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
        //SceneManager.LoadScene("Lvl1Scene");
        StartCoroutine(LoadSceneAsync("Lvl1Scene"));
    }
    public void Level2Load()
    {
        //SceneManager.LoadScene("Lvl2Scene");
        StartCoroutine(LoadSceneAsync("Lvl2Scene"));
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

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress/ 0.9f);

            //Loading bar

            yield return null;
        }
    }


}