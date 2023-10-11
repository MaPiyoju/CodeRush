using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionHandler : MonoBehaviour
{
    public bool correctOption;
    public GameObject quizManager;

    public void OptionSelected()
    {
        quizManager.GetComponent<QuizManager>().Answered(correctOption == true);
    }

}
