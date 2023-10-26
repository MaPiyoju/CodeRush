using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtn : MonoBehaviour
{
    public Enums.Scenes targetScene;
    public GameObject selectedBG;

    
    void Awake()
    {
        if ((Enums.Scenes)SceneManager.GetActiveScene().buildIndex != targetScene)
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
        if ((Enums.Scenes)SceneManager.GetActiveScene().buildIndex != targetScene) {
            ScenesManager test = new ScenesManager();
            test.LoadScene(targetScene);
        }
    }
}
