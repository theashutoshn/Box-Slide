using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string[] levelScenes;
    private int currentLevelIndex = 0;
    private int gameEndIndex = 7;

    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    void LoadLevel(int index)
    {
        if(index >= 0 && index < levelScenes.Length)
        {
            SceneManager.LoadSceneAsync(levelScenes[index], LoadSceneMode.Additive);
        }
    }

    void UnloadLevel(int index)
    {
        if(index >= 0 && index < levelScenes.Length)
        {
            SceneManager.UnloadSceneAsync(levelScenes[index]);
        }
    }

    public void OnLevelComplete()
    {
        UnloadLevel(currentLevelIndex);

        currentLevelIndex++;

        if(currentLevelIndex < levelScenes.Length)
        {
            LoadLevel(currentLevelIndex);
        }
        else
        {
            SceneManager.LoadSceneAsync(gameEndIndex, LoadSceneMode.Additive);
            Debug.Log("All levels Completed");
        }

       
    }

    public string GetCurrentSceneName()
    {
        if(currentLevelIndex >= 0 && currentLevelIndex < levelScenes.Length)
        {
            return levelScenes[currentLevelIndex];
        }
        return string.Empty;
    }

    public void ReloadCurrentLevel()
    {
        AudioManager.instance.PlayButtonClick();
        string currentScene = GetCurrentSceneName();
        if (!string.IsNullOrEmpty(currentScene))
        {
            SceneManager.UnloadSceneAsync(currentScene).completed += (AsyncOperation op) => {
                SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
            };
        }
    }
}