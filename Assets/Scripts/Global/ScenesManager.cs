using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Types.Scenes targetScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

    public void LoadScene(Types.Scenes newScene)
    {
        SceneManager.LoadScene(newScene.ToString());
    }
}
