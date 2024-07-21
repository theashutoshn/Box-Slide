using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string[] levleScenes;
    private int currentLevelIndex = 0;


    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    void LoadLevel(int index)
    {
        if(index >= 0 && index < levleScenes.Length)
        {
            SceneManager.LoadSceneAsync(levleScenes[index], LoadSceneMode.Additive);
        }
    }

    void UnloadLevel(int index)
    {
        if(index >= 0 && index < levleScenes.Length)
        {
            SceneManager.UnloadSceneAsync(levleScenes[index]);
        }
    }

    public void OnLevelComplete()
    {
        UnloadLevel(currentLevelIndex);

        currentLevelIndex++;

        if(currentLevelIndex < levleScenes.Length)
        {
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("All levels Completed");
        }
    }
}