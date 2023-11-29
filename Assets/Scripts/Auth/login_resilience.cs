using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class login_resilience : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Resilience();
    }

    // Update is called once per frame
    void Update()
    {
        //Resilience();
    }

    private async void Resilience()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            Debug.Log("Current is: "+auth.CurrentUser.UserId);
            FirestoreManager dbManager = new FirestoreManager();
            List<Dictionary<string, object>> items = await dbManager.ReadDataByIdAsync("users", auth.CurrentUser.UserId);
            List<MatchModel> history = new List<MatchModel>();
            foreach (var item in items)
            {
                foreach (Dictionary<string, object> hst in (IEnumerable)item["history"])
                {
                    var timestamp = (Timestamp)hst["date"];
                    var currDt = timestamp.ToDateTime();

                    MatchModel matchTmp = new()
                    {
                        date = currDt,
                        score = int.Parse(hst["score"].ToString()),
                        time = int.Parse(hst["time"].ToString()),
                        type = (Enums.QuizTypes)int.Parse(hst["type"].ToString()),
                        win = bool.Parse(hst["win"].ToString()),
                        questions = int.Parse(hst["questions"].ToString()),
                        correctQuestions = int.Parse(hst["correctQuestions"].ToString())
                    };
                    history.Add(matchTmp);
                }
                UserModel usuario = new UserModel();
                usuario.name = item["name"].ToString();
                usuario.alias = item["alias"].ToString();
                var ts = (Timestamp)item["first_login"];
                var Date = ts.ToDateTime();
                usuario.first_login = Date;
                usuario.exp = int.Parse(item["exp"].ToString());
                usuario.streak = int.Parse(item["streak"].ToString());
                usuario.history = history;
                Globals.usuario = usuario;
                Globals.lives = int.Parse(item["lives"].ToString());
                Globals.exp = int.Parse(item["exp"].ToString());
                //LifeHandler.UpdateLife();
            }

            SceneManager.LoadScene("Home");
        }
        else {
            Debug.Log("NO Current is: ");
        }
    }
}
