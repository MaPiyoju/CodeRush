using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizMode : MonoBehaviour
{
    public Enums.QuizTypes targetMode;

    public void SetGameMode()
    {
        Globals.gameMode = targetMode;
    }
}
