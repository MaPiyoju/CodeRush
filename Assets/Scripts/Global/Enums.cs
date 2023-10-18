using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum Scenes
    {
        Home,
        SingleMain,
        BattleMain,
        StatsMain,
        ProfileMain,
        Quiz
    }

    public enum QuestionType
    {
        Open,
        Closed
    }

    public enum QuestionDifficulty
    {
        Easy,
        Medium,
        Hard,
        Pro
    }

    public enum QuestionCategory
    {
        Logic,
        python_syntax,
        Performance,
        Algorithms
    }
	
	public enum QuizTypes
    {
        Practice,
        Rush,
        Battle
    }
}
