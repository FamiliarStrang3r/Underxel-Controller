using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
