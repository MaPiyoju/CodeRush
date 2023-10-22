using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionHandler : MonoBehaviour
{
    public bool correctOption;
    public string txtCorrectOption;
    public GameObject quizManager;

    public AudioClip[] audioEffects;

    public bool enterEnabled = true;

    public void OptionSelected()
    {
        quizManager.GetComponent<QuizManager>().Answered(correctOption == true);

        if (correctOption == true)
            Camera.main.GetComponent<AudioSource>().clip = audioEffects[0];
        else
            Camera.main.GetComponent<AudioSource>().clip = audioEffects[1];

        Camera.main.GetComponent<AudioSource>().Play();
    }

    public void OptionInput()
    {
        string inputText = GetComponentsInChildren<TextMeshProUGUI>()[1].text.ToLower().Trim();


        quizManager.GetComponent<QuizManager>().Answered(txtCorrectOption.ToLower() == inputText);

        if (txtCorrectOption.ToLower() == inputText)
            Camera.main.GetComponent<AudioSource>().clip = audioEffects[0];
        else
            Camera.main.GetComponent<AudioSource>().clip = audioEffects[1];

        GetComponentsInChildren<TextMeshProUGUI>()[0].SetText("");
        GetComponentsInChildren<TextMeshProUGUI>()[1].SetText("");
        this.gameObject.SetActive(false);
        Camera.main.GetComponent<AudioSource>().Play();
    }
}
