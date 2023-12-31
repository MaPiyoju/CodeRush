using Firebase.Extensions;
using Firebase.Firestore;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ingreso : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField inEmail;
    public TMP_InputField inPass;
    public void ingreso()
    {
        StartCoroutine(this.LogIn());
    }
    public IEnumerator LogIn()
    {
        var email = inEmail.text;
        var password = inPass.text;
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        var login = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => login.IsCompleted);

        if (login.IsCanceled)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
        }
        if (login.IsFaulted)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + login.Exception);
            ColorBlock color = inPass.colors;
            color.normalColor = new Color(1, 0.8f, 0.8f);
            inPass.colors = color;
            inEmail.colors = color;
        }
        else
        {
            SceneManager.LoadScene("Home");
            Firebase.Auth.AuthResult result = login.Result;
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference docRef = db.Collection("users").Document(auth.CurrentUser.UserId);

            Dictionary<string, object> last = new Dictionary<string, object>
            {
                { "last_login",DateTimeOffset.FromUnixTimeMilliseconds((long)auth.CurrentUser.Metadata.LastSignInTimestamp).LocalDateTime }
            };
            docRef.UpdateAsync(last).ContinueWithOnMainThread(task => {
                Debug.Log("Updated last login");
            });

            LoadCurrDataAsync(auth);

            Debug.LogFormat("User signed in successfully: {0} ({1})",
            result.User.DisplayName, result.User.UserId);
        }
        
        
    }

    public async void LoadCurrDataAsync(Firebase.Auth.FirebaseAuth auth)
    {
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
            LifeHandler.UpdateLife();
        }
    }

}
