using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 0;

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => SceneLoader.LoadScene(sceneIndex));
    }
}
