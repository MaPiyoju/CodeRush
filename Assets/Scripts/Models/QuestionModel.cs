public class QuestionModel
{
    public QuestionModel(string question, Enums.QuestionType type, string[] answers, int correct, Enums.QuestionDifficulty difficulty, Enums.QuestionCategory category)
    {
        this.Question = question;
        this.Type = type;
        this.Answers = answers;
        this.Correct = correct;
        this.Difficulty = difficulty;
        this.Category = category;
    }

    public string Question { get; set; }

    public Enums.QuestionType Type { get; set; }

    public string[] Answers { get; set; }

    public int Correct { get ; set; }

    public Enums.QuestionDifficulty Difficulty { get; set; }

    public Enums.QuestionCategory Category { get; set; }
}
