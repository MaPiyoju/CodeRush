using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtn : MonoBehaviour
{
    public Types.Scenes targetScene;
    public GameObject selectedBG;

    
    void Awake()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex.ToString());
        Debug.Log(targetScene.ToString());
        if ((Types.Scenes)SceneManager.GetActiveScene().buildIndex != targetScene)
        {
            selectedBG.SetActive(false);
        }
        else
        {
            selectedBG.SetActive(true);
        }
    }

    public void BtnClick()
    {
        if ((Types.Scenes)SceneManager.GetActiveScene().buildIndex != targetScene)
            ScenesManager.Instance.LoadScene(targetScene);
    }
}
