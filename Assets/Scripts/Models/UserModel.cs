using Firebase.Firestore;
using System;
using System.Collections.Generic;

[FirestoreData]
public class UserModel
{
    [FirestoreProperty]
    public string name { get; set; }
    [FirestoreProperty]
    public string alias { get; set; }

    [FirestoreProperty]
    public DateTime last_login { get; set; }

    [FirestoreProperty]
    public DateTime first_login { get; set; }

    [FirestoreProperty]
    public int lives { get; set; }
    [FirestoreProperty]
    public int exp { get; set; }
    [FirestoreProperty]
    public int streak { get; set; }
    [FirestoreProperty]
    public string pic { get; set; }
    
    [FirestoreProperty]
    public List<MatchModel> history { get; set; }
}
