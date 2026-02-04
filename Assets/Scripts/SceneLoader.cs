using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Loader Settings")]
    [SerializeField] private bool useAsync = true;

    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("SceneLoader.LoadNextScene: No next scene in Build Settings.");
            return;
        }

        if (useAsync)
            SceneManager.LoadSceneAsync(nextIndex);
        else
            SceneManager.LoadScene(nextIndex);
    }
    public void LoadScene1()
    {
        const int targetIndex = 1;
        if (targetIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning($"SceneLoader.LoadScene1: Scene index {targetIndex} is not in Build Settings.");
            return;
        }

        GameManager.Instance.Initialize();

        if (useAsync)
            SceneManager.LoadSceneAsync(targetIndex);
        else
            SceneManager.LoadScene(targetIndex);
    }
    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}