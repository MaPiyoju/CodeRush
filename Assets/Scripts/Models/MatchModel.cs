using Firebase.Firestore;
using System;


[FirestoreData]
public class MatchModel
{
    [FirestoreProperty]
    public DateTime date { get; set; }
    [FirestoreProperty]
    public Enums.QuizTypes type { get; set; }
    [FirestoreProperty]
    public int score { get; set; }
    [FirestoreProperty]
    public int time { get; set; }
    [FirestoreProperty]
    public bool win { get; set; }
    [FirestoreProperty]
    public int questions { get; set; }
    [FirestoreProperty]
    public int correctQuestions { get; set; }
}
