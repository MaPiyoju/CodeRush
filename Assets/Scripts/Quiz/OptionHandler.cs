using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionHandler : MonoBehaviour
{
    public bool correctOption;
    public GameObject quizManager;

    public AudioClip[] audioEffects;

    public void OptionSelected()
    {
        quizManager.GetComponent<QuizManager>().Answered(correctOption == true);

        if (correctOption == true)
            Camera.main.GetComponent<AudioSource>().clip = audioEffects[0];
        else
            Camera.main.GetComponent<AudioSource>().clip = audioEffects[1];

        Camera.main.GetComponent<AudioSource>().Play();
    }

}
