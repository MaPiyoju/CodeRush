using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum Scenes
    {
        SignIn,
        Home,
        SingleMain,
        BattleMain,
        StatsMain,
        ProfileMain,
        Quiz,
        Results,
        SignUp
    }

    public enum QuestionType
    {
        open,
        closed
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
        logic,
        python_syntax,
        performance,
        algorithms
    }
	
	public enum QuizTypes
    {
        Practice,
        Rush,
        Battle
    }
}
